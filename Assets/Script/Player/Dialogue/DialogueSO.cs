using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Botography.Player.Dialogue
{
    /// <summary>
    /// Abstract class that holds the integral information for a block of dialogue.
    /// </summary>
    public abstract class DialogueSO : ScriptableObject
    {
        [HideInInspector] public DialogueSO Prev;
        [HideInInspector] public DialogueSO Next;
        public string SpeakerName = PlayerConstants.DEFAULT_DIALOGUE_NAME;
        [TextArea(3, 10)] public string Text;
        [HideInInspector] public string UnrichText;

        public abstract bool IsAsync();

        public void GenUnrichText()
        {
            UnrichText = "";

            string[] textSplit = Regex.Split(Text, @"(?<!\\)<.*?(?<!\\)>");

            for (int i = 0; i < textSplit.Length; i++)
            {
                UnrichText = $"{UnrichText}{textSplit[i]}";
            }

            UnrichText = UnrichText.Replace("\\<", "<");
			UnrichText = UnrichText.Replace("\\>", ">");
		}
    }
}