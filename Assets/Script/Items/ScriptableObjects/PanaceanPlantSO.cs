using UnityEngine;

[CreateAssetMenu(fileName = "PanaceanPlantSO", menuName = "PanaceanPlant")]

public class PanaceanPlantSO : ScriptableObject
{
    [SerializeField, Range(1, 15)] private int _pageNumber;
    [SerializeField] private string _plantName;
    [SerializeField] private string _plantScientificName;
    [SerializeField] private Sprite _artwork;
    [SerializeField] private Sprite _almanacArtwork;
    [SerializeField, TextArea(3, 10)] private string _description;
    [SerializeField] private GameObject _prefab;

    public int PageNumber => _pageNumber;
    public string PlantName => _plantName;
    public string PlantScientificName => _plantScientificName;
    public Sprite Artwork => _artwork;
    public Sprite AlmanacArtwork => _almanacArtwork;
    public string Description => _description;
    public GameObject Prefab => _prefab;
}
