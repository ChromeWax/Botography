using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using Botography.Lab;

public class AlmanacUI : MenuUiCollectionBase
{
    [SerializeField] private GameObject _leftPlantTab;
    [SerializeField] private GameObject _rightPlantTab;
    [SerializeField] private GameObject _leftNoteTab;
    [SerializeField] private GameObject _rightNoteTab;
    [SerializeField] private GameObject _pageWithPlantInformation;
    [SerializeField] private GameObject _recipeOne;
    [SerializeField] private GameObject _recipeTwo;
    [SerializeField] private GameObject _createOne;
    [SerializeField] private GameObject _createTwo;
    [SerializeField] private GameObject _buttonToTurnLeftPage;
    [SerializeField] private GameObject _buttonToTurnRightPage;
    [SerializeField] private GameObject _firstNote;
    [SerializeField] private GameObject _secondNote;
    [SerializeField] private GameObject _plant;
    [SerializeField] private GameObject _note;
    [SerializeField] private GameObject _almanacBack;
    [SerializeField] private Button _helpBubble;
    [SerializeField] private GameObject _helpPopup;
    [SerializeField] private Button _leftCraft;
    [SerializeField] private Button _rightCraft;
    [SerializeField] private GameObject _lab;
    private bool _helpActive;

	private readonly Dictionary<string, AttributeValue> STRING_TO_ATTRIBUTE = new()
	{
		{ "loam", AttributeValue.Loam },
		{ "sand", AttributeValue.Sand },
		{ "silt", AttributeValue.Silt },
		{ "clay" , AttributeValue.Clay},
		{ "neutral", AttributeValue.Neutral },
		{ "basic", AttributeValue.Basic },
		{ "acidic", AttributeValue.Acidic }
	};

    public void SetPlantBlankPage(int index)
    {
        SwitchToPlantMode();
        SetRecipeOne("???", "???", "???");
        SetRecipeTwo("???", "???", "???");
        SetPlantName("???");
        SetScientificName("???");
        SetImage(null);
        SetPlantDescription("???");
        ToggleCraftButton(0, false);
        ToggleCraftButton(1, false);
        //_blankPage.SetActive(true);
        if (index > 10)
        {
            SetRecipeOneActive(false);
            SetRecipeTwoActive(false);
            TogglePanaceanDescription(true);
            _almanacBack.SetActive(true);
            SetPanaceanDescription("");
        }
        else
        {
            SetRecipeOneActive(true);
            SetRecipeTwoActive(true);
            TogglePanaceanDescription(false);
            _almanacBack.SetActive(false);
        }
    }

    public void SetNoteBlankPage(int index)
    {
        SwitchToNoteMode();
        if (index == 0)
        {
            SetNoteAuthor("", 0);
            SetNoteCoord("", 0);
            SetNoteDescription("", 0);
        }
        else
        {
            SetNoteAuthor("", 1);
            SetNoteCoord("", 1);
            SetNoteDescription("", 1);
        }
    }

    public void SetNotePage(string author, string coord, string description, int index)
    {
        SwitchToNoteMode();
        if (index == 0)
        {
            SetNoteAuthor(author, 0);
            SetNoteCoord(coord, 0);
            SetNoteDescription(description, 0);
        }
        else
        {
            SetNoteAuthor(author, 1);
            SetNoteCoord(coord, 1);
            SetNoteDescription(description, 1);
        }
    }

    public void SetPlantPage(string name, Sprite image, string scientificName, string description)
    {
        SwitchToPlantMode();
        SetRecipeOneActive(true);
        SetRecipeTwoActive(true);
        SetPlantName(name);
        SetScientificName(scientificName);
        SetImage(image);
        SetPlantDescription(description);
        TogglePanaceanDescription(false);
        _almanacBack.SetActive(false);
    }

    public void SetPanaceanPage(string name, Sprite image, string scientificName, string description)
    {
        SwitchToPlantMode();
        SetRecipeOneActive(false);
        SetRecipeTwoActive(false);
        SetPlantName(name);
        SetScientificName(scientificName);
        SetImage(image);
        SetPanaceanDescription(description);
        TogglePanaceanDescription(true);
        _almanacBack.SetActive(true);
    }

    private void SetRecipeOneActive(bool active)
    {
        _recipeOne.SetActive(active);
        _createOne.SetActive(active);
    }

    private void SetRecipeTwoActive(bool active)
    {
        _recipeTwo.SetActive(active);
        _createTwo.SetActive(active);
    }

    public void SetRecipeOne(string sun, string water, string soil)
    { 
        _recipeOne.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sun;
        _recipeOne.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = soil;
        _recipeOne.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = water;
    }

    public void SetRecipeTwo(string sun, string water, string soil)
    {
        _recipeTwo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sun;
        _recipeTwo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = soil;
        _recipeTwo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = water;
    }

    private void SetPlantName(string name)
    {
        _pageWithPlantInformation.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
    }

    private void SetScientificName(string scientificName)
    {
        _pageWithPlantInformation.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = scientificName;
    }

    private void SetImage(Sprite image)
    {
        _pageWithPlantInformation.transform.GetChild(2).GetComponent<Image>().sprite = image;
        if (image != null)
            _pageWithPlantInformation.transform.GetChild(2).GetComponent<Image>().color = Color.white;
        else
            _pageWithPlantInformation.transform.GetChild(2).GetComponent<Image>().color = Color.clear;
    }

    private void SetPlantDescription(string description)
    {
        _pageWithPlantInformation.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = description;
    }

    private void SetPanaceanDescription(string description)
    {
        _pageWithPlantInformation.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = description;
    }

    private void SetNoteAuthor(string author, int index)
    {
        if (index == 0)
            _firstNote.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = author;
        else
            _secondNote.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = author;
    }

    private void SetNoteCoord(string coord, int index)
    {
        if (index == 0)
            _firstNote.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = coord;
        else
            _secondNote.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = coord;
    }

    private void SetNoteDescription(string description, int index)
    {
        if (index == 0)
            _firstNote.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = description;
        else
            _secondNote.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = description;
    }

    private void TogglePanaceanDescription(bool enable)
    {
        _pageWithPlantInformation.transform.GetChild(3).gameObject.SetActive(!enable);
        _pageWithPlantInformation.transform.GetChild(4).gameObject.SetActive(enable);
    }

    public void UpdatePlantTabSides(int pageIndex, int noteCount)
    {
        if (pageIndex < 15)
        {
            for (int i = 0; i < 15; i++)
            {
                _leftNoteTab.SetActive(false);
                _rightNoteTab.SetActive(false);
                _leftPlantTab.SetActive(true);
                _rightPlantTab.SetActive(true);
                
                if (i < pageIndex)
                {
                    _leftPlantTab.transform.GetChild(i).gameObject.SetActive(true);
                    _rightPlantTab.transform.GetChild(i).gameObject.SetActive(false);
                    _leftPlantTab.transform.GetChild(i).GetComponent<Button>().interactable = true;
                }
                else if (i == pageIndex)
                {
                    _leftPlantTab.transform.GetChild(i).gameObject.SetActive(false);
                    _rightPlantTab.transform.GetChild(i).gameObject.SetActive(true);
                    _rightPlantTab.transform.GetChild(i).GetComponent<Button>().interactable = false;
                }
                else
                {
                    _leftPlantTab.transform.GetChild(i).gameObject.SetActive(false);
                    _rightPlantTab.transform.GetChild(i).gameObject.SetActive(true);
                    _rightPlantTab.transform.GetChild(i).GetComponent<Button>().interactable = true;
                }
            }
        }
        else
        {
            _leftNoteTab.SetActive(true);
            _rightNoteTab.SetActive(true);
            _leftPlantTab.SetActive(false);
            _rightPlantTab.SetActive(false);

            int plantTabCount = pageIndex - 15;
            for (int i = 0; i < Mathf.CeilToInt((float)noteCount / 2); i++)
            {
                if (i < plantTabCount)
                {
                    _leftNoteTab.transform.GetChild(i).gameObject.SetActive(true);
                    _rightNoteTab.transform.GetChild(i).gameObject.SetActive(false);
                    _rightNoteTab.transform.GetChild(i).GetComponent<Button>().interactable = true;
                }
                else if (i == plantTabCount)
                {
                    _leftNoteTab.transform.GetChild(i).gameObject.SetActive(false);
                    _rightNoteTab.transform.GetChild(i).gameObject.SetActive(true);
                    _rightNoteTab.transform.GetChild(i).GetComponent<Button>().interactable = false;
                }
                else
                {
                    _leftNoteTab.transform.GetChild(i).gameObject.SetActive(false);
                    _rightNoteTab.transform.GetChild(i).gameObject.SetActive(true);
                    _rightNoteTab.transform.GetChild(i).GetComponent<Button>().interactable = true;
                }
            }
            for (int i = Mathf.CeilToInt((float)noteCount / 2); i < 15; i++)
            {
                _leftNoteTab.transform.GetChild(i).gameObject.SetActive(false);
                _rightNoteTab.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void UpdateLeftRightButton(int index, int noteCount)
    {
        if (index == 0)
        {
            _buttonToTurnLeftPage.gameObject.GetComponent<Button>().interactable = false;
            _buttonToTurnRightPage.gameObject.GetComponent<Button>().interactable = true;
        }
        else if (Mathf.Ceil((float)noteCount / 2) == index - 14)
        {
            _buttonToTurnLeftPage.gameObject.GetComponent<Button>().interactable = true;
            _buttonToTurnRightPage.gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            _buttonToTurnLeftPage.gameObject.GetComponent<Button>().interactable = true;
            _buttonToTurnRightPage.gameObject.GetComponent<Button>().interactable = true;
        }

        if (index < 15)
            SwitchToPlantMode();
        else
            SwitchToNoteMode();
    }

    private void SwitchToNoteMode()
    {
        _plant.SetActive(false);
        _note.SetActive(true);
    }

    private void SwitchToPlantMode()
    {
        _plant.SetActive(true);
        _note.SetActive(false);
    }

    public void ToggleHelpPopup()
    {
        _helpActive = !_helpActive;
        _helpPopup.SetActive(_helpActive);
    }

    public void ToggleHelpPopup(bool active)
    {
        _helpActive = active;
        _helpPopup.SetActive(active);
    }

	private void OnEnable()
	{
        AlmanacManager.Instance.BindAlmanacControls();
        AlmanacManager.Instance.UpdatePage();
	}

	private void OnDisable()
	{
		ToggleHelpPopup(false);
        AlmanacManager.Instance.UnbindAlmanacControls();
	}

    public void ToggleCraftButton(int index, bool enable)
    {
        if (index == 0)
        {
            _leftCraft.interactable = enable;
            if (enable)
                _leftCraft.GetComponent<Image>().color = Color.white;
            else
                _leftCraft.GetComponent<Image>().color = Color.gray;
        }    
        else
        {
            _rightCraft.interactable = enable;
            if (enable)
                _rightCraft.GetComponent<Image>().color = Color.white;
            else
                _rightCraft.GetComponent<Image>().color = Color.gray;
        }
    }
    
    public void CreateButton(int index)
    {
        string soil;
        string water;
        AttributeValue soilAtt;
        AttributeValue waterAtt;

        if (index == 0)
        {
            soil = _recipeOne.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
            water = _recipeOne.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
        }
        else
        {
            soil = _recipeTwo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
            water = _recipeTwo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
        }

        soilAtt = STRING_TO_ATTRIBUTE[soil];
        waterAtt = STRING_TO_ATTRIBUTE[water];

        UIManager.Instance.ToggleUI(_lab);
        LabManager.Instance.PopulateSlots(InventoryConstants.WATER_ATTRIBUTE_DICTIONARY[waterAtt], InventoryConstants.SOIL_ATTRIBUTE_DICTIONARY[soilAtt]);
    }

}
