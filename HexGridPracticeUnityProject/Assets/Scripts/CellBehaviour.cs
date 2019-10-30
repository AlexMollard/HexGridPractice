using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    public Material[] CellMaterial;
    public enum CellType { Taiga, Savanna, Tundra, RainForest, Desert, Forest, Plains, Snow, Ocean, Beach, Bare, Scorched, HotRainForest, WetDesert, TropSeasonForest }
    public enum BiomeType { Taiga, Savanna, Tundra, RainForest, Desert, Forest, Plains, Snow, Ocean, Beach, Bare, Scorched, HotRainForest, WetDesert, TropSeasonForest }
    public float[] TileHeight;
    public CellType TileType;
    public BiomeType TileBiome = BiomeType.Plains;
    public Vector2 TilePostition;
    public GameObject[] Neighbours;
    public float altitude;
    public float humidity;
    public bool Selected;
    public Color defaultColor;

    public void Awake()
    {
        TileHeight = new float[] { 3.75f, 3.0f, 4.25f, 1.75f, 3.0f, 2.75f, 2.5f, 4.45f, 1.0f, 1.5f, 4.0f, 3.85f, 3.0f, 2.0f, 2.0f };
    }

    public void SetTileProperties(float Altitude, float Humidity)
    {
        SetBiome(Altitude, Humidity);
        AssignType();
    }

    void SetBiome(float Altitude, float Humidity)
    {
        altitude = Altitude * 2;
        TileBiome = GetBiomeType(Altitude, Humidity);
    }

    BiomeType GetBiomeType(float Altitude, float Humidity)
    {
        if (Altitude < 0.1) return BiomeType.Ocean;
        if (Altitude < 0.15) return BiomeType.Beach;

        if (Altitude > 0.8)
        {
            if (Humidity < 0.1) return BiomeType.Scorched;
            if (Humidity < 0.2) return BiomeType.Bare;
            if (Humidity < 0.5) return BiomeType.Tundra;
            return BiomeType.Snow;
        }

        if (Altitude > 0.6)
        {
            if (Humidity < 0.33) return BiomeType.Desert;
            if (Humidity < 0.66) return BiomeType.Savanna;
            return BiomeType.Taiga;
        }

        if (Altitude > 0.3)
        {
            if (Humidity < 0.16) return BiomeType.Desert;
            if (Humidity < 0.50) return BiomeType.Plains;
            if (Humidity < 0.83) return BiomeType.Forest;
            return BiomeType.HotRainForest;
        }

        if (Humidity < 0.16) return BiomeType.WetDesert;
        if (Humidity < 0.33) return BiomeType.Plains;
        if (Humidity < 0.66) return BiomeType.TropSeasonForest;
        return BiomeType.RainForest;
    }


    void AssignType()
    {
        if (TileBiome == BiomeType.Taiga)
        {
            TileType = CellType.Taiga;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Taiga];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
        }

        else if (TileBiome == BiomeType.Savanna)
        {
            TileType = CellType.Savanna;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Savanna];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if (TileBiome == BiomeType.Tundra)
        {
            TileType = CellType.Tundra;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Tundra];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.RainForest)
        {
            TileType = CellType.RainForest;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.RainForest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.Desert)
        {
            TileType = CellType.Desert;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Desert];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.Forest)
        {
            TileType = CellType.Forest;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Forest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.Plains)
        {
            TileType = CellType.Plains;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Plains];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.Snow)
        {
            TileType = CellType.Snow;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Snow];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.Ocean)
        {
            TileType = CellType.Ocean;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Ocean];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + (altitude / 4.0f), 1f);

        }

        else if(TileBiome == BiomeType.Beach)
        {
            TileType = CellType.Beach;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Beach];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.Bare)
        {
            TileType = CellType.Bare;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Bare];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.Scorched)
        {
            TileType = CellType.Scorched;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Scorched];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.HotRainForest)
        {
            TileType = CellType.HotRainForest;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.HotRainForest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.WetDesert)
        {
            TileType = CellType.WetDesert;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.WetDesert];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if (TileBiome == BiomeType.TropSeasonForest)
        {
            TileType = CellType.TropSeasonForest;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.TropSeasonForest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

    }

    private void OnMouseOver()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase != TouchPhase.Moved)
        {
            if (!Selected)
            {
                transform.GetComponentInParent<SpawnGrid>().SetText("Biome: " + TileType);
                transform.localScale = new Vector3(1f, transform.localScale.y * 1.25f, 1f);
                defaultColor = GetComponent<Renderer>().material.color;
                GetComponent<Renderer>().material.color -= new Color(0.5f * GetComponent<Renderer>().material.color.r, 0.5f * GetComponent<Renderer>().material.color.g, 0.5f * GetComponent<Renderer>().material.color.b); ;
            }
        Selected = true;
        }
    }

    private void OnMouseExit()
    {
        if (Selected)
        {
            GetComponent<Renderer>().material.color = defaultColor;
        }
        Selected = false;
        AssignType();
    }
}
