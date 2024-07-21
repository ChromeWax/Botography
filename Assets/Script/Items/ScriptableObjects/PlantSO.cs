using UnityEngine;

[CreateAssetMenu(fileName = "PlantSO", menuName = "Plant")]
public class PlantSO : ScriptableObject
{
    [SerializeField, Range(1, 15)] private int _pageNumber;
    [SerializeField] private string _plantName;
    [SerializeField] private string _plantScientificName;
    [SerializeField] private Sprite _artwork;
    [SerializeField] private Sprite _almanacArtwork;
    [SerializeField] private SunlightLevel _plantSunlightLevel1;
    [SerializeField] private SoilType _plantSoilType1;
    [SerializeField] private WaterPh _plantWaterPh1;
    [SerializeField] private SunlightLevel _plantSunlightLevel2;
	[SerializeField] private SoilType _plantSoilType2;
	[SerializeField] private WaterPh _plantWaterPh2;
    [SerializeField, TextArea(3, 10)] private string _description;
    [SerializeField] private UsageType _plantUsageType;
    [SerializeField] private GameObject _prefab;

    public int PageNumber => _pageNumber;
    public string PlantName => _plantName;
    public string PlantScientificName => _plantScientificName;
    public Sprite Artwork => _artwork;
    public Sprite AlmanacArtwork => _almanacArtwork;
    public SunlightLevel PlantSunlightLevel1  => _plantSunlightLevel1;
    public SoilType PlantSoilType1 => _plantSoilType1;
    public WaterPh PlantWaterPh1 => _plantWaterPh1;
    public SunlightLevel PlantSunlightLevel2 => _plantSunlightLevel2;
    public SoilType PlantSoilType2 => _plantSoilType2;
    public WaterPh PlantWaterPh2 => _plantWaterPh2;
    public string Description => _description;
    public UsageType PlantUsageType => _plantUsageType;
    public GameObject Prefab => _prefab;

    public string getPlantName(){
        return _plantName;
    }
}
