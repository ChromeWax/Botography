using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Dependencies.Interfaces;
using Botography.Dependencies;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Botography.Player
{
    /// <summary>
    /// The system that handles player data.
    /// </summary>
    public class PlayerSystem : SystemBase
    {
		public Vector2 Position
		{
			get { return transform.position; }
		}

        public bool InDarkness;


		// Start is called before the first frame update
		void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
