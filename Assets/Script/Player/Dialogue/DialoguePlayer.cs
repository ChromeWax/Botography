using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;
using Botography.Player;
using System;
using System.Text.RegularExpressions;

namespace Botography.Player.Dialogue
{
	/// <summary>
	/// Handles the creation and presentation of dialogue from the character.
	/// </summary>
	public class DialoguePlayer : MonoBehaviour
	{
		public static DialoguePlayer Instance { get; private set; }

		[SerializeField] private DialogueDatabaseSO _db;
		[SerializeField, Range(0.1f, 3)] private float _dialogueSpeed;
		[SerializeField] private TMPro.TextMeshProUGUI _name;
		[SerializeField] private TMPro.TextMeshProUGUI _tmp;
		[SerializeField] private GameObject _bubble;
		private int _dialogueIndex;
		private float _charTime;
		private float _endTime;
		private ConvoSO _currentConvo;
		private DialogueSO _currentDialogue;
		private bool _playing;
		private bool _blocked; // Awaiting user input
		private ConvoSO _waiting = null;
		private Action _currCallback;
		private Action _waitCallback;
		private bool _inputBound = false;

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}

			if (!CheckValid())
			{
				Instance = null;
				Destroy(this);
			}
		}

		private void Start()
		{
			//PlayConvo("TestConvo");
		}

		// Update is called once per frame
		void Update()
		{
			if (_playing)
			{
				if (_blocked)
				{
					if (!_currentDialogue.IsAsync())
					{
						_endTime += Time.deltaTime;
						if (_endTime >= PlayerConstants.DIALOGUE_END_TIME_CONST * _currentDialogue.Text.Length)
						{
							EndDialogue();
						}
					}
				}
				else
				{
					if (_dialogueIndex >= _currentDialogue.Text.Length)
					{
						_blocked = true;
					}
					else
					{
						_charTime += Time.deltaTime;
						if (_charTime > 1 / _dialogueSpeed * PlayerConstants.DIALOGUE_S_BETWEEN_CHARS)
						{
							char nextChar = _currentDialogue.Text[_dialogueIndex];
							if (nextChar.Equals('\\') && _dialogueIndex < _currentDialogue.Text.Length - 1 && (_currentDialogue.Text[_dialogueIndex + 1].Equals('<') || _currentDialogue.Text[_dialogueIndex + 1].Equals('>')))
							{
								nextChar = _currentDialogue.Text[++_dialogueIndex];
							}
							else if (nextChar.Equals('<'))
							{
								int endOfTag = _dialogueIndex;
								for (; endOfTag < _currentDialogue.Text.Length && (!_currentDialogue.Text[endOfTag].Equals('>') && (endOfTag != 0 && !_currentDialogue.Text[endOfTag].Equals('\\'))); endOfTag++) { }
								_tmp.text = $"{_tmp.text}{_currentDialogue.Text.Substring(_dialogueIndex, endOfTag + 1 - _dialogueIndex)}";
								_dialogueIndex = endOfTag + 1;
								nextChar = _currentDialogue.Text[_dialogueIndex];
							}
							_tmp.text = $"{_tmp.text}{nextChar}";
							_charTime = 0;
							_dialogueIndex++;
						}
						
					}
				}
			}
		}

		private bool CheckValid()
		{
			List<string> dups = _db.GetDupConvos();
			if (dups.Count != 0)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("Invalid Convos. The following Convo names were found multiple times:\n");
				foreach (string d in dups)
				{
					sb.Append($"{d}\n");
				}
				sb.Append("\nPlease make sure all Convos in the database have unique names.");
				Debug.LogError(sb.ToString());
				return false;
			}

			return true;
		}

		/// <summary>
		/// If there is already a Convo playing, this will override it on the next Sync dialogue.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="callback">An action that is invoked when the Convo ends.</param>
		public void PlayConvo(string name, Action callback = null)
		{
			ConvoSO ctp = _db.GetConvoByName(name);
			if (ctp == null)
			{
				Debug.LogWarning($"Could not find Convo by the name of {name} in Dialogue Databse {_db.name}");
				return;
			}

			PlayConvo(ctp, callback);
		}

		private void PlayConvo(ConvoSO c, Action callback = null)
		{
			
			if (_playing)
			{
				if (_currentDialogue != null && _currentDialogue.IsAsync())
				{
					_waiting = c;
					_waitCallback = callback;
					return;
				}
			}

			WipeBubble();
			c.Initialize();
			if (c.IsEmpty())
			{
				Debug.LogWarning($"Could not play Convo \"{c.Name}\" because it is empty.");
				return;
			}

			_currentConvo = c;
			_currentDialogue = c.Next();
			_name.text = _currentDialogue.SpeakerName;
			_dialogueIndex = 0;
			_currCallback = callback;
			_blocked = false;
			OpenBubble();
			_playing = true;
			if (!_inputBound)
			{
				InputHandler.Instance.OnCursorLeftPressed += OnInput;
				_inputBound = true;
			}
			if (_currentDialogue.IsAsync())
			{
				PlayerStateMachine.Instance.UnbindControls();
			}
		}

		private void OnInput()
		{
			if (_blocked)
			{
				EndDialogue();
			}
			else
			{
				_tmp.text = _currentDialogue.Text;
				_dialogueIndex = _currentDialogue.Text.Length;
				_blocked = true;
			}
		}

		private void EndConvo()
		{
			CloseBubble();
			_currentDialogue = null;
			_currentConvo = null;
			_name.text = "";
			_playing = false;
			PlayerStateMachine.Instance.BindControls();
			if (_currCallback != null)
			{
				_currCallback.Invoke();
				_currCallback = null;
			}
			if (_inputBound)
			{
				InputHandler.Instance.OnCursorLeftPressed -= OnInput;
				_inputBound = false;
			}
		}

		private void EndDialogue()
		{
			
			WipeBubble();
			_currentDialogue = _currentConvo.Next();

			if (_waiting != null && (_currentDialogue == null || !_currentDialogue.IsAsync()))
			{
				EndConvo();
				ConvoSO c = _waiting;
				_waiting = null;
				Action call = _waitCallback;
				_waitCallback = null;
				PlayerStateMachine.Instance.BindControls();
				PlayConvo(c, call);
				return;
			}

			_dialogueIndex = 0;
			_blocked = false;
			_endTime = 0;
			if (_currentDialogue == null)
			{
				EndConvo();
			}
			else
			{
				_name.text = _currentDialogue.SpeakerName;
				if (_currentDialogue.IsAsync())
				{
					PlayerStateMachine.Instance.UnbindControls();
				}
                else
                {
					PlayerStateMachine.Instance.BindControls();
                }
            }
		}

		private void WipeBubble()
		{
			_tmp.text = string.Empty;
			_name.text = string.Empty;
		}

		private void CloseBubble()
		{
			WipeBubble();
			_bubble.SetActive(false);
		}

		private void OpenBubble()
		{
			_bubble.SetActive(true);
		}
		
//-----------------------------------------------------------------------
		
		public ConvoSO getCurConvo(){
			return _currentConvo;
		}
		
		public DialogueSO getCurDialogue(){
			return _currentDialogue;
		}
		
		public bool isPlaying(){
			return _playing;
		}
		
		public ConvoSO getWaiting(){
			return _waiting;
		}
		
		public Action getCurrCallback(){
			return _currCallback;
		}
		
		public Action getWaitCallback(){
			return _waitCallback;
		}
	}
}
