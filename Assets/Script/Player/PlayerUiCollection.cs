using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Dependencies.Interfaces;
using Botography.Dependencies;

namespace Botography.Player
{
    public class PlayerUiCollection : UiCollectionBase
    {
        [SerializeField] private GameObject _intrctCanvas;
		[SerializeField] private GameObject _intrctIcon;
        [SerializeField] private TMPro.TextMeshProUGUI _intrctActText;

		// Start is called before the first frame update
		void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ActivateInteractIcon(string action)
        {
            _intrctActText.text = action;
            _intrctIcon.SetActive(true);
        }

		public void DeactivateInteractIcon()
		{
			_intrctActText.text = "";
			_intrctIcon.SetActive(false);
		}

        public void ShowInteractionIcon(bool active)
        {
            _intrctCanvas.SetActive(active);
        }
	}
}
