using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.Dependencies.Interfaces
{
	/// <summary>
	/// Interface for the ManagerBase class.
	/// </summary>
	public interface IManager
	{
		/// <summary>
		/// Sets all UI elements associated with this manager active based on the given boolean.
		/// </summary>
		/// <param name="active"></param>
		void SetUiActive(bool active, bool overrideExempt = false);
	}
}
