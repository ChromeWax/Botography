using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Botography.Dependencies.Interfaces
{
	/// <summary>
	/// The interface for UI elements that need to be directly tracked by
	/// UI Collections and/or need to be able to implement pointer events.
	/// </summary>
	public interface IUiElement
	{
		/// <summary>
		/// The GameObject the element is attached to.
		/// </summary>
		GameObject GameObject { get; }

		/// <summary>
		/// If the element is exempt, that means it shouldn't necessarily
		/// be edited when iterating over many UI elements.
		/// </summary>
		bool Exempt { get; set; }
	}
}
