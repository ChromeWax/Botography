using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Botography.Player.Dialogue
{
	/// <summary>
	/// Acts as a doubly linked list of DialogueSO objects
	/// </summary>
	[CreateAssetMenu(menuName = "Dialogue/ConvoSO")]
	public class ConvoSO : ScriptableObject
	{
		[SerializeField] private List<DialogueSO> dialogues;
		public string Name
		{
			get { return _convoName; }
		}

		[SerializeField] private string _convoName;
		private DialogueSO _head;
		private DialogueSO _ptr;
		private DialogueSO _tail;

		public void Initialize()
		{
			if (dialogues.Count == 0)
			{
				return;
			}

			_head = dialogues[0];
			DialogueSO curr = _head;
			curr.GenUnrichText();

			for (int i = 1; i < dialogues.Count; i++)
			{
				curr.Next = dialogues[i];
				dialogues[i].Prev = curr;
				curr = dialogues[i];
				curr.GenUnrichText();
			}

			_tail = curr;

			SetToHead();
		}

		public void SetToHead()
		{
			_ptr = _head;
		}

		/// <summary>
		/// Returns the current dialogue and sets the pointer to the next dialogue.
		/// </summary>
		/// <returns></returns>
		public DialogueSO Next()
		{
			if (_ptr == null)
			{
				return null;
			}

			DialogueSO temp = _ptr;
			_ptr = _ptr.Next;
			if (_ptr == null)
			{
				return temp;
			}
			
			return _ptr.Prev;
		}

		/// <summary>
		/// Returns the current dialogue and sets the pointer to the previous dialogue.
		/// </summary>
		/// <returns></returns>
		public DialogueSO Previous()
		{
			if (_ptr == null)
			{
				return null;
			}

			DialogueSO temp = _ptr;
			_ptr = _ptr.Prev;
			if (_ptr == null)
			{
				return temp;
			}
			return _ptr.Prev;
		}

		public void Append(DialogueSO newDialogue)
		{
			_tail.Next = newDialogue;
			newDialogue.Prev = _tail;
			newDialogue.Next = null;
			_tail = newDialogue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>Returns true if the head of the dialogue linked list is null</returns>
		public bool IsEmpty()
		{
			if (_head == null)
			{
				
			}
			return _head == null;
		}

		public bool Equals(ConvoSO comp)
		{
			return _convoName == comp.Name;
		}

		public static bool operator ==(ConvoSO comp1, ConvoSO comp2)
		{
			if ((comp1 is null && comp2 is not null) || (comp1 is not null && comp2 is null))
			{
				return false;
			}
			else if (comp1 is null && comp2 is null)
			{
				return true;
			}
			return comp1.Equals(comp2);
		}

		public static bool operator !=(ConvoSO comp1, ConvoSO comp2)
		{
			if ((comp1 is null && comp2 is not null) || (comp1 is not null && comp2 is null))
			{
				return true;
			}
			else if (comp1 is null && comp2 is null)
			{
				return false;
			}
			return !comp1.Equals(comp2);
		}

		private void OnDisable()
		{
			
		}
	}
}