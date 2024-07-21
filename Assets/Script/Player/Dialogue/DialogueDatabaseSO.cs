using System.Collections.Generic;
using UnityEngine;

namespace Botography.Player.Dialogue
{
	[CreateAssetMenu(menuName = "Dialogue/Dialogue Database")]
	/// <summary>
	/// Carries a collection of ConvoSO objects to reference and return information about.
	/// </summary>
	public class DialogueDatabaseSO : ScriptableObject
	{
		[SerializeField] private List<ConvoSO> convos;

		public ConvoSO GetConvoByName(string name)
		{
			foreach (ConvoSO c in convos)
			{
				if (c.Name.Equals(name))
				{
					return c;
				}
			}

			return null;
		}

		public List<string> GetDupConvos()
		{
			List<string> dupC = new();
			List<string> uniqueC = new();

			foreach (ConvoSO c in convos)
			{
				if (uniqueC.Contains(c.Name) && !dupC.Contains(c.Name))
				{
					dupC.Add(c.Name);
				}
				else
				{
					uniqueC.Add(c.Name);
				}
			}

			return dupC;
		}
	}
}

