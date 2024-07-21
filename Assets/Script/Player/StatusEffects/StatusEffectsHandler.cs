using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Botography.Player.PlayerConstants;

namespace Botography.Player.StatusEffects
{
	/// <summary>
	/// Tracks all status effects on the character.
	/// </summary>
	public class StatusEffectsHandler : MonoBehaviour
	{
		public bool StatusEffectsEnabled = true;

		public static StatusEffectsHandler Instance { get; private set; }
		private Dictionary<StatusEffectUI, CoyoteTime> _timedCoyotes = new Dictionary<StatusEffectUI, CoyoteTime>();
		private Dictionary<StatusEffectUI, CoyoteTime> _posCoyotes = new Dictionary<StatusEffectUI, CoyoteTime>();
		private Dictionary<StatusEffectUI, CoyoteTime> _ubCoyotes = new Dictionary<StatusEffectUI, CoyoteTime>();
		private Dictionary<StatusEffectType, bool> _unbearables = new Dictionary<StatusEffectType, bool>()
		{
			{ StatusEffectType.Cooling, false },
			{ StatusEffectType.Heating, false },
			{ StatusEffectType.Illuminated, false },
			{ StatusEffectType.Underwater, false }
		};
		private Dictionary<StatusEffectType, float> _unbearableTimes = new Dictionary<StatusEffectType, float>()
		{
			{ StatusEffectType.Cooling, 0 },
			{ StatusEffectType.Heating, 0 },
			{ StatusEffectType.Illuminated, 0 },
			{ StatusEffectType.Underwater, 0 }
		};

		private TempLevel _prevEnvTemp;
		private float _tSinceTempCheck = 0;

		private bool _initialized = false;
		private bool _respawning;

		#region Do Not Reference
		/// <summary>
		/// Do not use directly. Use Temperature instead.
		/// </summary>
		private int _temp;
		/// <summary>
		/// Do not set directly. Use IsUnderWater instead.
		/// </summary>
		private bool _isUnderWater;
		/// <summary>
		/// Do not set directly. Use IsDark instead.
		/// </summary>
		private bool _isDark;
		/// <summary>
		/// Do not set directly. Use IsIlluminated instead.
		/// </summary>
		private bool _isIlluminated;
		/// <summary>
		/// Do not set directly. Use CanBreatheUnderWater instead.
		/// </summary>
		private bool _canBreatheUnderWater;
		#endregion Do Not Reference

		#region Properties
		public int Temperature
		{
			get { return _temp; }
			set
			{
				_temp = value;
				AssessTemp();
			}
		}

		public bool IsIlluminated
		{
			get { return _isIlluminated; }
			set
			{
				_isIlluminated = value;
				AssessLight();
			}
		}

		public bool CanBreatheUnderWater
		{
			get { return _canBreatheUnderWater; }
			set
			{
				_canBreatheUnderWater = value;
				AssessBreathing();
			}
		}

		public bool IsUnderWater
		{
			get { return _isUnderWater; }
			set
			{
				_isUnderWater = value;
				AssessBreathing();
			}
		}

		public bool IsDark
		{
			get { return _isDark; }
			set
			{
				_isDark = value;
				AssessLight();
			}
		}
		#endregion Properties

		#region UI
		[SerializeField] private StatusEffectUI Cold;
		[SerializeField] private StatusEffectUI Heat;
		[SerializeField] private StatusEffectUI Lumin;
		[SerializeField] private StatusEffectUI Underwater;
		[SerializeField] private Image _tempFill;

		[SerializeField] private GameObject _freezeFace;
		[SerializeField] private GameObject _sweatFace;
		[SerializeField] private GameObject _underwaterFace;
		[SerializeField] private GameObject _luminFace;
		[SerializeField] private GameObject _happyFace;
		[SerializeField] private GameObject _darkFace;
		[SerializeField] private GameObject _curFace;
		#endregion UI

		private void Awake()
		{
			if (Instance != null && Instance != this)
				Destroy(this);
			else
				Instance = this;
		}

		private void Start()
		{
			_initialized = false;
			Setup();
		}

		void Update()
		{
			UpdateUIs(Time.deltaTime);

			if (!StatusEffectsEnabled)
			{
				return;
			}

			foreach (KeyValuePair<StatusEffectType,bool> u in _unbearables)
			{
				StatusEffectUI ui = GetUi(u.Key);
				if (u.Value)
				{
					_unbearableTimes[u.Key] += Time.deltaTime;
					if (!_respawning && _unbearableTimes[u.Key] >= MaxTimeUnbearable && _ubCoyotes[ui] == null)
					{
						_ubCoyotes[ui] = CoyoteTimeHandler.Instance.InitiateCoyoteTime(UniversalCoyoteTime, InitiateRespawn);
					}
				}
				else
				{
					_ubCoyotes.TryGetValue(ui, out CoyoteTime c);
					if (c != null)
					{
						CoyoteTimeHandler.Instance.EndCoyoteTimePrematurely(_ubCoyotes[ui]);
						_ubCoyotes[ui] = null;
					}
					if (_unbearableTimes[u.Key] != 0)
					{
						_unbearableTimes[u.Key] = 0;
					}
				}
			}

			_tSinceTempCheck += Time.deltaTime;

			if (_tSinceTempCheck >= TIME_TO_CHECK_ENV_TEMP)
			{
				int curETemp = (int)PlayerManager.Instance.GetTempLevel();
				int tempdif = curETemp - (int)_prevEnvTemp;
				Temperature += tempdif;
				_prevEnvTemp = (TempLevel)curETemp;
				_tSinceTempCheck = 0;
				//IsDark = ;
			}
		}

		public void Setup()
		{
			if (!StatusEffectsEnabled)
			{
				return;
			}

			_timedCoyotes.Add(GetUi(StatusEffectType.Cooling), null);
			_timedCoyotes.Add(GetUi(StatusEffectType.Heating), null);
			_timedCoyotes.Add(GetUi(StatusEffectType.Illuminated), null);
			_timedCoyotes.Add(GetUi(StatusEffectType.Underwater), null);

			_posCoyotes.Add(GetUi(StatusEffectType.Cooling), null);
			_posCoyotes.Add(GetUi(StatusEffectType.Heating), null);
			_posCoyotes.Add(GetUi(StatusEffectType.Illuminated), null);
			_posCoyotes.Add(GetUi(StatusEffectType.Underwater), null);

			_ubCoyotes.Add(GetUi(StatusEffectType.Cooling), null);
			_ubCoyotes.Add(GetUi(StatusEffectType.Heating), null);
			_ubCoyotes.Add(GetUi(StatusEffectType.Illuminated), null);
			_ubCoyotes.Add(GetUi(StatusEffectType.Underwater), null);

			TempLevel temp = PlayerManager.Instance.GetTempLevel();
			if (!_respawning)
			{
				_prevEnvTemp = temp;
				if (temp == TempLevel.cool || temp == TempLevel.cold || temp == TempLevel.warm || temp == TempLevel.hot)
				{
					int mod = -1;
					int stacks = 0;
					if (temp == TempLevel.warm || temp == TempLevel.hot)
					{
						mod = 1;
					}

					stacks += Math.Abs((int)temp);

					Temperature += mod * stacks;
				}
				else
				{
					Temperature += 0;
				}
			}
			else
			{
				Temperature += (int)temp - (int)_prevEnvTemp;
				_prevEnvTemp = temp;
			}

			//IsDark = PlayerManager.Instance.GetLightLevel() == SunlightLevel.fullShade;

			_initialized = true;
			_respawning = false;
		}

		public bool IsNearDeath()
		{
			return _curFace != _happyFace && _curFace != _luminFace;
		}

		private StatusEffectUI GetUi(StatusEffectType type)
		{
			switch (type)
			{
				case StatusEffectType.Cooling:
					{
						return Cold;
					}
				case StatusEffectType.Heating:
					{
						return Heat;
					}
				case StatusEffectType.Illuminated:
					{
						return Lumin;
					}
				case StatusEffectType.Underwater:
					{
						return Underwater;
					}
				default:
					{
						return null;
					}
			}
		}

		#region Assessments
		private void AssessLight()
		{
			if (!StatusEffectsEnabled)
			{
				return;
			}

			if (!IsIlluminated && IsDark)
			{
				_unbearables[StatusEffectType.Illuminated] = true;
			}
			else
			{
				_unbearables[StatusEffectType.Illuminated] = false;
			}
			AssessFace();
		}

		private void AssessTemp()
		{
			if (!StatusEffectsEnabled)
			{
				return;
			}

			int tempFillKey = 0;
			if (Temperature >= MaxTemp)
			{
				_unbearables[StatusEffectType.Heating] = true;
				_unbearables[StatusEffectType.Cooling] = false;
				tempFillKey = MaxTemp;
			}
			else if (Temperature <= MinTemp)
			{
				_unbearables[StatusEffectType.Cooling] = true;
				_unbearables[StatusEffectType.Heating] = false;
				tempFillKey = MinTemp;
			}
			else
			{
				_unbearables[StatusEffectType.Cooling] = false;
				_unbearables[StatusEffectType.Heating] = false;
				tempFillKey = Temperature;
			}

			_tempFill.fillAmount = TEMP_FILL[tempFillKey];
			AssessFace();
		}

		private void AssessBreathing()
		{
			if (!StatusEffectsEnabled)
			{
				return;
			}

			if (!CanBreatheUnderWater && IsUnderWater)
			{
				_unbearables[StatusEffectType.Underwater] = true;
			}
			else
			{
				_unbearables[StatusEffectType.Underwater] = false;
			}
			AssessFace();
		}

		private void AssessFace()
		{
			if (!StatusEffectsEnabled)
			{
				return;
			}

			_curFace.SetActive(false);

			if (!CanBreatheUnderWater && IsUnderWater)
			{
				_curFace = _underwaterFace;
			}
			else if (Temperature >= MaxTemp)
			{
				_curFace = _sweatFace;
			}
			else if (Temperature <= MinTemp)
			{
				_curFace = _freezeFace;
			}
			else if (IsIlluminated)
			{
				_curFace = _luminFace;
			}
			else if (!IsIlluminated && IsDark)
			{
				_curFace = _darkFace;
			}
			else
			{
				_curFace = _happyFace;
			}

			_curFace.SetActive(true);
		}
		#endregion Assessments

		public void AddTimedSE(TimedStatusEffect effect)
		{
			if (!StatusEffectsEnabled)
			{
				return;
			}

			StatusEffectUI ui = GetUi(effect.Type);
			if (ui.TimerActive)
			{
				ui.AddToTimer(effect);
				if (_timedCoyotes[ui] != null)
				{
					CoyoteTimeHandler.Instance.EndCoyoteTimePrematurely(_timedCoyotes[ui]);
					_timedCoyotes[ui] = null;
				}
			}
			else
			{
				ui.SetTimer(effect);
				switch (effect.Type)
				{
					case StatusEffectType.Heating:
						Temperature += 1;
						break;
					case StatusEffectType.Cooling:
						Temperature -= 1;
						break;
					default:
						break;
				}
			}
		}

		private void UpdateUIs(float deltaTime)
		{
			if (!StatusEffectsEnabled)
			{
				return;
			}

			if (Cold.TimerActive)
			{
				Cold.UpdateTime(deltaTime);
				if (!Cold.TimerActive)
				{
					_timedCoyotes[Cold] = CoyoteTimeHandler.Instance.InitiateCoyoteTime(UniversalCoyoteTime, () =>
					{
						Cold.EndTimer();
						Temperature += 1;
						_timedCoyotes[Cold] = null;
					});
				}
			}
			else
			{
				if (Temperature < 0 && !Cold.BubbleOut)
				{
					Cold.StartEffect();
				}
                else if(Temperature >= 0 && Cold.BubbleOut)
                {
					Cold.EndEffect();
                }
            }

			if (Heat.TimerActive)
			{
				Heat.UpdateTime(deltaTime);
				if (!Heat.TimerActive)
				{
					_timedCoyotes[Heat] = CoyoteTimeHandler.Instance.InitiateCoyoteTime(UniversalCoyoteTime, () =>
					{
						Heat.EndTimer();
						Temperature -= 1;
						_timedCoyotes[Heat] = null;
					});
				}
			}
			else
			{
				if (Temperature > 0 && !Heat.BubbleOut)
				{
					Heat.StartEffect();
				}
				else if (Temperature <= 0 && Heat.BubbleOut)
				{
					Heat.EndEffect();
				}
			}

			if (Lumin.TimerActive)
			{
				Lumin.UpdateTime(deltaTime);
				if (!Lumin.TimerActive)
				{
					_timedCoyotes[Lumin] = CoyoteTimeHandler.Instance.InitiateCoyoteTime(UniversalCoyoteTime, () =>
					{
						Lumin.EndTimer();
						_timedCoyotes[Lumin] = null;
					});
				}
			}
			else
			{
				if (IsIlluminated && !Lumin.BubbleOut)
				{
					Lumin.StartEffect();
				}
				else if (!IsIlluminated && Lumin.BubbleOut)
				{
					Lumin.EndEffect();
				}
			}

			if (Underwater.TimerActive)
			{
				Underwater.UpdateTime(deltaTime);
				if (!Underwater.TimerActive)
				{
					_timedCoyotes[Underwater] = CoyoteTimeHandler.Instance.InitiateCoyoteTime(UniversalCoyoteTime, () =>
					{
						Underwater.EndTimer();
						_timedCoyotes[Underwater] = null;
					});
				}
			}
			else
			{
				if (IsUnderWater && !Underwater.BubbleOut)
				{
					Underwater.StartEffect();
				}
				else if (!IsUnderWater && Underwater.BubbleOut)
				{
					Underwater.EndEffect();
				}
			}
		}

		public void ClearStatusEffects()
		{
			if (!StatusEffectsEnabled)
			{
				return;
			}

			Cold.EndTimer(() => { Temperature++; });

			Heat.EndTimer(() => { Temperature--; });

			_unbearables = new Dictionary<StatusEffectType, bool>()
			{
				{ StatusEffectType.Cooling, false },
				{ StatusEffectType.Heating, false },
				{ StatusEffectType.Illuminated, false },
				{ StatusEffectType.Underwater, false }
			};
			_unbearableTimes = new Dictionary<StatusEffectType, float>()
			{
				{ StatusEffectType.Cooling, 0 },
				{ StatusEffectType.Heating, 0 },
				{ StatusEffectType.Illuminated, 0 },
				{ StatusEffectType.Underwater, 0 }
			};

			_timedCoyotes = new Dictionary<StatusEffectUI, CoyoteTime>();
			_posCoyotes = new Dictionary<StatusEffectUI, CoyoteTime>();
			_ubCoyotes = new Dictionary<StatusEffectUI, CoyoteTime>();
		}

		private void InitiateRespawn()
		{
			if (!_respawning)
			{
				_respawning = true;
				PlayerStateMachine.Instance.UnbindControls();
				foreach (KeyValuePair<StatusEffectType, bool> unb in _unbearables)
				{
					if (unb.Value)
					{
						if (UNBEARABLE_ANIM_STRINGS.ContainsKey(unb.Key))
						{
							PlayerStateMachine.Instance.CharacterAnimator.SetBool(UNBEARABLE_ANIM_STRINGS[unb.Key], true);
							return;
						}

						break;
					}
				}

				Respawn();
			}			
		}

		public void Respawn()
		{
			PlayerManager.Instance.Respawn();
		}

		public void SetPrevTemp()
		{
			_prevEnvTemp = PlayerManager.Instance.GetTempLevel();
		}
	}
}