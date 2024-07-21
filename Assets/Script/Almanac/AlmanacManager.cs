using Botography.Dependencies;
using Botography.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using static AlmanacConstants;

/// <summary>
/// This outlines the functionality of the almanac, including moving between pages and which plant pages appear.
/// </summary>

public class AlmanacManager : ManagerBase<AlmanacSystem, AlmanacUI>
{
    public static AlmanacManager Instance { get; private set; }
    private bool _ctrlBound = false;

    private readonly Dictionary<SunlightLevel, string> LIGHT_TO_STRING = new()
	{
		{ SunlightLevel.fullSun, "full sun" },
		{ SunlightLevel.partialSun, "partial sun" },
		{ SunlightLevel.halfAndHalf, "half and half" },
		{ SunlightLevel.partialShade, "partial shade" },
		{ SunlightLevel.fullShade , "full shade" }
	};

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        int currentPageNumber = system.GetCurrentPageNumber();
        GoToPageNumber(currentPageNumber);
    }

	public void GoToPageNumber(int newPageNumber)
    {
        Debug.Log(newPageNumber);
        system.SetCurrentPageNumber(newPageNumber);
        if (newPageNumber < 11)
        {
            AlmanacPlantDiscovered plant = system.GetPlantInfo(newPageNumber);
            if (plant.discovered)
            {
                PlantSO plantSO = plant.plantSO;
                uiCollection.SetPlantPage(plantSO.PlantName, plantSO.AlmanacArtwork, plantSO.PlantScientificName, plantSO.Description);

                if (plant.recipeOneDiscovered)
                {
                    uiCollection.SetRecipeOne(LIGHT_TO_STRING[plantSO.PlantSunlightLevel1], plantSO.PlantWaterPh1.ToString(), plantSO.PlantSoilType1.ToString());

                    if (InventoryManager.Instance.DoesSampleExist(InventoryConstants.WATERPH_DICTIONARY[plantSO.PlantWaterPh1]) && 
                        InventoryManager.Instance.DoesSampleExist(InventoryConstants.SOILTYPE_DICTIONARY[plantSO.PlantSoilType1]) &&
                        plantSO.PlantSunlightLevel1 == PlayerManager.Instance.GetLightLevel())
                    {
                        uiCollection.ToggleCraftButton(0, true);
                    }
                    else
                        uiCollection.ToggleCraftButton(0, false);
                }
                else
                {
                    uiCollection.SetRecipeOne("???", "???", "???");
                    uiCollection.ToggleCraftButton(0, false);
                }

                if (plant.recipeTwoDiscovered)
                {
                    uiCollection.SetRecipeTwo(LIGHT_TO_STRING[plantSO.PlantSunlightLevel2], plantSO.PlantWaterPh2.ToString(), plantSO.PlantSoilType2.ToString());

                    if (InventoryManager.Instance.DoesSampleExist(InventoryConstants.WATERPH_DICTIONARY[plantSO.PlantWaterPh2]) && 
                        InventoryManager.Instance.DoesSampleExist(InventoryConstants.SOILTYPE_DICTIONARY[plantSO.PlantSoilType2]) &&
                        plantSO.PlantSunlightLevel2 == PlayerManager.Instance.GetLightLevel())
                    {
                        uiCollection.ToggleCraftButton(1, true);
                    }
                    else
                        uiCollection.ToggleCraftButton(1, false);
                }
                else
                {
                    uiCollection.SetRecipeTwo("???", "???", "???");
                    uiCollection.ToggleCraftButton(1, false);
                }
            }
            else
            {
                uiCollection.SetPlantBlankPage(newPageNumber);
            }
            uiCollection.UpdatePlantTabSides(newPageNumber, system.GetCurrentNoteCount());
            uiCollection.UpdateLeftRightButton(newPageNumber, system.GetCurrentNoteCount());
        }
        else if (newPageNumber < 15)
        {
            AlmanacPanaceanPlantDiscovered plant = system.GetPanaceanPlantInfo(newPageNumber - 11);
            if (plant.discovered)
            {
                PanaceanPlantSO plantSO = plant.panceanPlantSO;
                uiCollection.SetPanaceanPage(plantSO.PlantName, plantSO.AlmanacArtwork, plantSO.PlantScientificName, plantSO.Description);
            }
            else
            {
                uiCollection.SetPlantBlankPage(newPageNumber);
            }
            uiCollection.UpdatePlantTabSides(newPageNumber, system.GetCurrentNoteCount());
            uiCollection.UpdateLeftRightButton(newPageNumber, system.GetCurrentNoteCount());
        }
        else 
        {
            NoteSO note1 = system.GetNoteInfo((newPageNumber - 15) * 2);
            NoteSO note2 = system.GetNoteInfo((newPageNumber - 15) * 2 + 1);
            if (note1 == null)
			{
                uiCollection.SetNoteBlankPage(0);
			}
            else
			{
				
                uiCollection.SetNotePage(note1.Author, GetCoord(), note1.Description, 0);
			}

            if (note2 == null)
			{
                uiCollection.SetNoteBlankPage(1);
			}
            else
			{
                uiCollection.SetNotePage(note2.Author, GetCoord(), note2.Description, 1);
			}
        
            uiCollection.UpdatePlantTabSides(newPageNumber, system.GetCurrentNoteCount());
            uiCollection.UpdateLeftRightButton(newPageNumber, system.GetCurrentNoteCount());
        }
    }
	
	private string GetCoord()
	{
		(int x, int y) noteCoord;
		string noteCoordStr;
		
		noteCoord = PlayerManager.Instance.GetCurrentSector();
		noteCoordStr = "x: " + noteCoord.Item1.ToString() + ", " + "y: " + noteCoord.Item2.ToString();
		return noteCoordStr;
	}

    public void TurnLeftPage()
    {
        int newPage = system.GetCurrentPageNumber() - 1;
        if (newPage < 0)
            newPage = 0;
        SoundManager.Instance.PlaySFX("page turn");
        GoToPageNumber(newPage);    
    }

    public void TurnRightPage()
    {
        int newPage = system.GetCurrentPageNumber() + 1;
        int maxCount = 15 + Mathf.RoundToInt(system.GetCurrentNoteCount() / 2);
        if (newPage > maxCount)
            newPage = maxCount;
        SoundManager.Instance.PlaySFX("page turn");
        GoToPageNumber(newPage);    
    }

    public void SetPlantDiscoveryStatus(int index, bool discovered, bool recipeOneDiscovered, bool recipeTwoDiscovered)
    {
        system.SetPlantDiscoveryStatus(index, discovered, recipeOneDiscovered, recipeTwoDiscovered);
    }

    public void SetPanaceanPlantDiscoveryStatus(int index, bool discovered)
    {
        system.SetPanaceanPlantDiscoveryStatus(index, discovered);
    }

    public void AddNote(NoteSO note)
    {
        system.AddNoteSO(note);
    }

    public AlmanacPlantDiscovered GetPlantInfo(int plantIndex)
    {
        return system.GetPlantInfo(plantIndex);
    }

    public AlmanacPanaceanPlantDiscovered GetPanaceanPlantInfo(int plantIndex)
    {
        return system.GetPanaceanPlantInfo(plantIndex);
    }

    public void GetCurrentPageNumber()
    {
        system.GetCurrentPageNumber();
    }

    public void SetCurrentPageNumber(int pageIndex)
    {
        system.SetCurrentPageNumber(pageIndex);
    }

    public int GetCurrentNoteCount()
    {
        return system.GetCurrentNoteCount();
    }

    public void SetPageToCurrentPageCount()
    {
        GoToPageNumber(system.GetCurrentPageNumber());
    }

	public void ToggleHelpPopup()
	{
        uiCollection.ToggleHelpPopup();
	}

	public void ToggleHelpPopup(bool active)
	{
		uiCollection.ToggleHelpPopup(active);
	}

	private void OnDisable()
	{
        ToggleHelpPopup(false);
	}
	
	public List<AlmanacPlantDiscovered> getPlantsDiscovered(){
		return system.getPlantsDiscovered();
	}
	
	public List<AlmanacPanaceanPlantDiscovered> getPanaceanDiscovered(){
		return system.getPanaceanDiscovered();
	}
	
	public List<NoteSO> getNotes(){
		return system.getNotes();
	}

    private void AlmanacInput(Vector2 input)
    {
        if (input.x != 0)
        {
            if (input.x < 0)
			{
				TurnLeftPage();
			}
			else if (input.x > 0)
			{
				TurnRightPage();
			}
		}
    }

    public void UnbindAlmanacControls()
    {
		if (_ctrlBound)
		{
			InputHandler.Instance.OnMovementPressed -= AlmanacInput;
            _ctrlBound = false;
		}
	}

	public void BindAlmanacControls()
	{
        if (!_ctrlBound)
        {
            InputHandler.Instance.OnMovementPressed += AlmanacInput;
            _ctrlBound = true;
        }
	}

    public bool GetCtrlBound()
    {
        return _ctrlBound;
    }

    public void UpdatePage()
    {
        GoToPageNumber(system.GetCurrentPageNumber());
    }
}
