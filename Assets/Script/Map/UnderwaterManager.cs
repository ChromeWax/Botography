using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Botography.Player;
using Botography.Player.StatusEffects;
using Botography.OverworldInteraction;

public class UnderwaterManager : MonoBehaviour
{
	public static UnderwaterManager Instance;

    [SerializeField] private GameObject waterGameObject;
    [SerializeField] Tilemap waterTilemap;
    [SerializeField] Tilemap borderTilemap;
    private int prevLayer;
    private string prevSortingLayer;
    SpriteRenderer[] spriteRenderers;
    Tilemap[] tilemapRenderers;
    List<Color> colors = new List<Color>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        spriteRenderers = waterGameObject.GetComponentsInChildren<SpriteRenderer>();
        tilemapRenderers = waterGameObject.GetComponentsInChildren<Tilemap>();
    }

    private void Start()
    {
		waterGameObject.SetActive(true);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = Color.clear;
        }
        for (int i = 0; i < tilemapRenderers.Length; i++)
        {
            colors.Add(tilemapRenderers[i].color);
            tilemapRenderers[i].color = Color.clear;
        }
    }

    public void ToggleUnderwater(bool enable)
    {
        if (enable)
        {
		    PlayerStateMachine.Instance.GoUnderwater();
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i] == null)
                    continue;

                if (spriteRenderers[i].gameObject.name == "GroundShadow")
                    spriteRenderers[i].color = new Color32(0, 0, 0, 150);
                else if (spriteRenderers[i].gameObject.name == "Water")
                    spriteRenderers[i].color = new Color32(255, 255, 255, 115);
                else
                    spriteRenderers[i].color = Color.white;
            }
            for (int i = 0; i < tilemapRenderers.Length; i++)
                tilemapRenderers[i].color = colors[i];
        }
        else
        {
			PlayerStateMachine.Instance.GoAbovewater();
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i] == null)
                    continue;

                spriteRenderers[i].color = Color.clear;
            }
            for (int i = 0; i < tilemapRenderers.Length; i++)
                tilemapRenderers[i].color = Color.clear;
        }

        PostProcessManager.Instance.SetUnderwater(enable);
		StatusEffectsHandler.Instance.IsUnderWater = enable;
	}

    public void SetPreviousLayers(int layer, string sortingLayer)
    {
        prevLayer = layer;
        prevSortingLayer = sortingLayer;
    }

    public int GetPrevLayer()
    {
        return prevLayer;
    }

    public string GetPrevSortingLayer()
    {
        return prevSortingLayer;
    }

    /*
    private void Update()
    {
		Vector3Int gridPositionInt = OWIManager.getTile(PlayerManager.Instance.GetCurrentPlayerPosition()) + new Vector3Int(-1, 0, 0);
		TileBase waterTile = waterTilemap.GetTile(gridPositionInt);
		TileBase borderTile = borderTilemap.GetTile(gridPositionInt);

        if (waterTile == null && borderTile == null)
            ToggleUnderwater(false);
    }
    */
}
