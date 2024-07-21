using Botography.UIGeneric;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ColorConstants;

namespace Botography.Notifications
{
	/// <summary>
	/// Controls the appearance and disappearance of text in the notification window.
	/// Used by the Notifyer.
	/// </summary>
	public class Notification : MonoBehaviour
	{
		public static Notification notificationOnScreen;
		private bool _displaying = false;
		private float _timeSinceDisplayed = 0;
		private float _timeToDisplay;

		public TextElement TextElement;

		private void Start()
		{
			if (!_displaying)
			{
				gameObject.SetActive(false);
			}
		}

		private void Update()
		{
			if (_displaying)
			{
				if (_timeSinceDisplayed >= _timeToDisplay)
				{
					Destroy(gameObject);
					notificationOnScreen = null;
				}
				_timeSinceDisplayed += Time.deltaTime;
			}
		}

		public void SetMessage(string message)
		{
			TextElement.Text.text = message;
		}

		public void SetMessageColor(Color color)
		{
			TextElement.Text.color = color;
		}

		public void Display(float timeToDisplay = 3)
		{
			notificationOnScreen = this;
			_timeToDisplay = timeToDisplay;
			_displaying = true;
			gameObject.SetActive(true);
		}
	}
}
