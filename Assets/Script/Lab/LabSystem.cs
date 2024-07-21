using Botography.Dependencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.Lab
{
    /// <summary>
    /// The system that handles the algorithms and data sets for the lab.
    /// </summary>
    public class LabSystem : MenuSystemBase
    {
        private List<GameObject> _inventoryItems;
        [SerializeField] private RecipeListSO _recipeList;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

		protected override void ToggleMenuOn()
		{
            
		}

        protected override void ToggleMenuOff()
        {
            
        }

        public void LoadInventory(List<GameObject> items)
        {
            _inventoryItems = items;
        }

        public PlantSO TryCombination(SunlightLevel sun, SoilType soil, WaterPh water)
        {
            return _recipeList.GetRecipeOutput(sun, soil, water);
        }
	}
}
