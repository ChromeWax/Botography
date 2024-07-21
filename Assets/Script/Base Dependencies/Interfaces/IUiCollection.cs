using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.Dependencies.Interfaces
{
	/// <summary>
	/// Interface for the UiCollectionBase class.
	/// </summary>
	public interface IUiCollection
	{
		bool IsActive { get; }

		UiCollectionBase Base { get; }

		/// <summary>
		/// Sets all UI elements active based on the given boolean.
		/// </summary>
		/// <param name="active"></param>
		/// <param name="overrideExempt">When false, UI elements that are exempt are skipped.</param>
		void SetAllElementsActive(bool active, bool overrideExempt = false);
	}
}
