using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Dependencies;

public class AlmanacSystem : MenuSystemBase
{
    [SerializeField] private List<AlmanacPlantDiscovered> _recipePlants;
    [SerializeField] private List<AlmanacPanaceanPlantDiscovered> _panaceanPlants;
    [SerializeField] private List<NoteSO> _notes;

    private int _pageCurrentlyOpen = 0;

    public void SetPlantDiscoveryStatus(int index, bool discovered, bool recipeOneDiscovered, bool recipeTwoDiscovered)
    {
        _recipePlants[index].discovered = discovered;
        _recipePlants[index].recipeOneDiscovered = recipeOneDiscovered;
        _recipePlants[index].recipeTwoDiscovered = recipeTwoDiscovered;
    }

    public void SetPanaceanPlantDiscoveryStatus(int index, bool discovered)
    {
        _panaceanPlants[index].discovered = discovered;
    }

    public void AddNoteSO(NoteSO note)
    {
        _notes.Add(note);
    }

    public AlmanacPlantDiscovered GetPlantInfo(int plantIndex)
    {
        return _recipePlants[plantIndex];
    }

    public AlmanacPanaceanPlantDiscovered GetPanaceanPlantInfo(int plantIndex)
    {
        return _panaceanPlants[plantIndex];
    }

    public NoteSO GetNoteInfo(int noteIndex)
    {
        if (noteIndex < _notes.Count)
            return _notes[noteIndex];
        else
            return null;
    }

    public int GetCurrentPageNumber()
    {
        return _pageCurrentlyOpen;
    }

    public void SetCurrentPageNumber(int pageIndex)
    {
        _pageCurrentlyOpen = pageIndex;
    }

    public int GetCurrentNoteCount()
    {
        return _notes.Count;
    }

    protected override void ToggleMenuOn()
    {
        throw new System.NotImplementedException();
    }

    protected override void ToggleMenuOff()
    {
        throw new System.NotImplementedException();
    }
	
	public List<AlmanacPlantDiscovered> getPlantsDiscovered(){
		return _recipePlants;
	}
	
	public List<AlmanacPanaceanPlantDiscovered> getPanaceanDiscovered(){
		return _panaceanPlants;
	}
	
	public List<NoteSO> getNotes(){
		return _notes;
	}
}
