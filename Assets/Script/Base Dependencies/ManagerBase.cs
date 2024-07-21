using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.Dependencies
{
	using Interfaces;
	using Unity.VisualScripting;

	/// <summary>
	/// Managers manage the relationship between UI collections and Systems. Systems should almost
	/// never communicate directly with UI elements or UI collections.This reduces unnecessary coupling.
	/// Managers also are able to communicate with one another so that systems can affect other systems as needed.
	/// </summary>
	/// <typeparam name="S">The system concrete class.</typeparam>
	/// <typeparam name="U">The UI collection concrete class.</typeparam>
	abstract public class ManagerBase<S, U> : MonoBehaviour, IManager where S : ISystem where U : IUiCollection
	{
		[SerializeField] protected S system;
		[SerializeField] protected U uiCollection;

		public void SetUiActive(bool active, bool overrideExempt = false)
		{
			uiCollection.SetAllElementsActive(active, overrideExempt);
		}
	}
}
