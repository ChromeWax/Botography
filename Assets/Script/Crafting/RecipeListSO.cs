using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NoItem", menuName = "RecipeList")]
public class RecipeListSO : ScriptableObject
{
    [SerializeField] private List<PlantSO> _recipes = new List<PlantSO>();

    public List<PlantSO> Recipes => _recipes;

    public PlantSO GetRecipeOutput(SunlightLevel sunlightLevel, SoilType soilType, WaterPh waterPh)
    {
        foreach (PlantSO plant in _recipes)
        {
            SunlightLevel plantSunlightLevel1 = plant.PlantSunlightLevel1;
            SoilType plantSoilType1 = plant.PlantSoilType1;
            WaterPh plantWaterPh1 = plant.PlantWaterPh1;

			SunlightLevel plantSunlightLevel2 = plant.PlantSunlightLevel2;
			SoilType plantSoilType2 = plant.PlantSoilType2;
			WaterPh plantWaterPh2 = plant.PlantWaterPh2;

			if ((plantSunlightLevel1 == sunlightLevel && plantSoilType1 == soilType && plantWaterPh1 == waterPh)
                || (plantSunlightLevel2 == sunlightLevel && plantSoilType2 == soilType && plantWaterPh2 == waterPh))
                return plant;
        }

        return null;
    }
}
