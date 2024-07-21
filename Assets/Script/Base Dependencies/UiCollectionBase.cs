using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.Dependencies
{
	using Interfaces;

	/// <summary>
	/// The UI Collections are meant to be classes related to certain systems
	/// that track and edit any UI elements related to the corresponding system.
	/// </summary>
	abstract public class UiCollectionBase : MonoBehaviour, IUiCollection
	{
		protected bool isActive;
		protected List<IUiElement> uiElements;

		#region Properties
		public bool IsActive {
			get { return isActive; }
		}

		public UiCollectionBase Base { get { return this; } }
		#endregion Properties

		public UiCollectionBase()
		{
			isActive = false;
		}

		public void SetAllElementsActive(bool active, bool overrideExempt = false)
		{
			foreach (IUiElement element in uiElements)
			{
				if (!overrideExempt && element.Exempt)
				{
					continue;
				}

				element.GameObject.SetActive(active);
			}
			isActive = active;
		}
		
		public void CloseMenu()
		{
			PlayerStateMachine.Instance.BindControls();
			gameObject.SetActive(false);
		}
	}
}
