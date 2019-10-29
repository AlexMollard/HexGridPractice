using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{

    //BiomeType[,] BiomeTable = new BiomeType[7, 7] {   
    ////COLDEST           //COLDER           //COLD             //HOT                  //HOTTER               //HOTTEST              //HOTTEST
    //{ BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Plains,  BiomeType.Desert,      BiomeType.Desert,      BiomeType.Desert    ,  BiomeType.Desert     },     //DRYEST
    //{ BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Plains,  BiomeType.Desert,      BiomeType.Desert,      BiomeType.Desert    ,  BiomeType.Desert     },     //DRYER
    //{ BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Forest,  BiomeType.Forest,      BiomeType.Savanna,     BiomeType.Savanna   ,  BiomeType.Savanna    },     //DRY
    //{ BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Forest,  BiomeType.Forest,      BiomeType.Savanna,     BiomeType.Savanna   ,  BiomeType.Savanna    },     //WET
    //{ BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Taiga,   BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest },     //WETTER
    //{ BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Taiga,   BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest },      //WETTEST
    //{ BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Taiga,   BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest }      //WETTEST
    //};

    public Material[] CellMaterial;
    public enum CellType  { Taiga, Savanna, Tundra, RainForest, Desert, Forest, Plains, Snow, Ocean, Beach, Bare, Scorched, HotRainForest, WetDesert }
    public enum BiomeType { Taiga, Savanna, Tundra, RainForest, Desert, Forest, Plains, Snow, Ocean, Beach, Bare, Scorched, HotRainForest, WetDesert }
    public CellType TileType;
    public BiomeType TileBiome = BiomeType.Plains;
    public Vector2 TilePostition;
    public GameObject[] Neighbours;
    public float Altitude;
    public float humidity;
    public void SetTileProperties(float Altitude, float Humidity)
    {
        SetBiome(Altitude, Humidity);
        AssignType();

    }

    void SetBiome(float Altitude, float Humidity)
    {
        TileBiome = GetBiomeType(Altitude, Humidity);
    }

    BiomeType GetBiomeType(float Altitude, float Humidity)
    {
        //Heat += transform.localScale.y / 10;
        //Heat /= 2;
        //Humidity /= 2;
        //Heat *= 10;
        //Humidity *= 10;
        //
        // heat = Heat;
        // humidity = Humidity;
        // Heat = Mathf.Clamp(Heat, 0, 6);
        // Humidity = Mathf.Clamp(Humidity, 0, 6);
        // return BiomeTable[(int)Humidity, (int)Heat];

        if (Altitude < 0.1) return BiomeType.Ocean;
        if (Altitude < 0.12) return BiomeType.Beach;

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
        if (Humidity < 0.66) return BiomeType.Forest;
        return BiomeType.RainForest;
    }


    void AssignType()
    {
        if (TileBiome == BiomeType.Taiga)
        {
            TileType = CellType.Taiga;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Taiga];
        }

        else if (TileBiome == BiomeType.Savanna)
        {
            TileType = CellType.Savanna;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Savanna];
        }

        else if (TileBiome == BiomeType.Tundra)
        {
            TileType = CellType.Tundra;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Tundra];
        }

        else if(TileBiome == BiomeType.RainForest)
        {
            TileType = CellType.RainForest;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.RainForest];
        }

        else if(TileBiome == BiomeType.Desert)
        {
            TileType = CellType.Desert;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Desert];
        }

        else if(TileBiome == BiomeType.Forest)
        {
            TileType = CellType.Forest;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Forest];
        }

        else if(TileBiome == BiomeType.Plains)
        {
            TileType = CellType.Plains;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Plains];
        }

        else if(TileBiome == BiomeType.Snow)
        {
            TileType = CellType.Snow;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Snow];
        }

        else if(TileBiome == BiomeType.Ocean)
        {
            TileType = CellType.Ocean;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Ocean];
        }

        else if(TileBiome == BiomeType.Beach)
        {
            TileType = CellType.Beach;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Beach];
        }

        else if(TileBiome == BiomeType.Bare)
        {
            TileType = CellType.Bare;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Bare];
        }

        else if(TileBiome == BiomeType.Scorched)
        {
            TileType = CellType.Scorched;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Scorched];
        }

        else if(TileBiome == BiomeType.HotRainForest)
        {
            TileType = CellType.HotRainForest;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.HotRainForest];
        }

        else if(TileBiome == BiomeType.WetDesert)
        {
            TileType = CellType.WetDesert;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.WetDesert];
        }

    }
}
