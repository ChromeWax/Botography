using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.Dependencies
{
	using Interfaces;

	/// <summary>
	/// Systems control and cache the values of particular computational significance
	/// in order for aspects of the game to function properly.
	/// </summary>
	public class SystemBase : MonoBehaviour, ISystem
	{
		#region Properties
		public SystemBase Base
		{
			get
			{
				return this;
			}
		}
		#endregion Properties
	}
}
