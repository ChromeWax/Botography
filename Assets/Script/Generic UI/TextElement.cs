using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Dependencies.Interfaces;
using TMPro;

namespace Botography.UIGeneric
{
	/// <summary>
	/// A class for standardizing the text elements in the game.
	/// </summary>
	public class TextElement : MonoBehaviour, IUiElement
	{
		protected bool exempt;
		[SerializeField]
		protected TextMeshProUGUI text;

		#region Properties
		public GameObject GameObject
		{
			get { return gameObject; }
		}

		public bool Exempt
		{
			get { return exempt; }
			set { exempt = value; }
		}

		public TextMeshProUGUI Text
		{
			get { return text; }
		}
		#endregion Properties

		
	}
}
