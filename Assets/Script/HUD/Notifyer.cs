using Botography.UIGeneric;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ColorConstants;

namespace Botography.Notifications
{
	/// <summary>
	/// The Notifyer has its own notification window that it posts notifications to.
	/// It has all of the necessary functions to post notifications to that window
	/// externally.
	/// </summary>
	public class Notifyer : MonoBehaviour
	{
		public static Notifyer Instance { get; private set; }

		public enum LogType
		{
			Log,
			Warning,
			Error
		}

		private Color _defaultNotificationColor = GetRegionUiColorDictionary()[UiColors.White];

		[SerializeField]
		private GameObject _notificationPrefab;
		[SerializeField]
		private GameObject _notificationWindow;
		[SerializeField]
		public bool stackNotification = false;

		/*private void Start()
		{
			Notify("Hello", UiColors.Blue);
		}*/

		private void Awake() 
		{
			if (Instance != null && Instance != this)
				Destroy(this);
			else
				Instance = this;	
		}

		public bool Notify(string message, UiColors color = UiColors.None, bool logNotification = false, LogType logType = LogType.Log)
		{
			try
			{
				Color newColor = _defaultNotificationColor;
				if (color != UiColors.None)
				{
					newColor = GetRegionUiColorDictionary()[color];
				}

				if (!stackNotification && Notification.notificationOnScreen != null)
				{
					Destroy(Notification.notificationOnScreen.gameObject);
					Notification.notificationOnScreen = null;
				}

				Notification newNotification = Instantiate(_notificationPrefab, _notificationWindow.transform).GetComponent<Notification>();
				newNotification.SetMessageColor(newColor);
				newNotification.SetMessage(message);
				newNotification.Display();

				if (logNotification)
				{
					switch(logType)
					{
						case LogType.Log:
							Debug.Log(message);
							break;
						case LogType.Warning:
							Debug.LogWarning(message);
							break;
						case LogType.Error:
							Debug.LogError(message);
							break;
					}
				}

				return true;
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
				return false;
			}
		}
	}

}