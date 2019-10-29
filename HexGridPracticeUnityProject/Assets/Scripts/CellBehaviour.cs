using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{

    BiomeType[,] BiomeTable = new BiomeType[7, 7] {   
    //COLDEST           //COLDER           //COLD             //HOT                  //HOTTER               //HOTTEST              //HOTTEST
    { BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Plains,  BiomeType.Desert,      BiomeType.Desert,      BiomeType.Desert    ,  BiomeType.Desert     },     //DRYEST
    { BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Plains,  BiomeType.Desert,      BiomeType.Desert,      BiomeType.Desert    ,  BiomeType.Desert     },     //DRYER
    { BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Forest,  BiomeType.Forest,      BiomeType.Savanna,     BiomeType.Savanna   ,  BiomeType.Savanna    },     //DRY
    { BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Forest,  BiomeType.Forest,      BiomeType.Savanna,     BiomeType.Savanna   ,  BiomeType.Savanna    },     //WET
    { BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Taiga,   BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest },     //WETTER
    { BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Taiga,   BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest },      //WETTEST
    { BiomeType.Ice,  BiomeType.Tundra,  BiomeType.Taiga,   BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest,  BiomeType.RainForest }      //WETTEST
    };

    public Material[] CellMaterial;
    public enum CellType { Water, Sand, RedSand, Dirt, Grass, Stone, Snow, Ice, DarkGrass, LightStone, DarkSand}
    public enum BiomeType {Taiga, Savanna, Tundra, RainForest, Desert, Forest, Plains, Ice}
    public CellType TileType;
    public BiomeType TileBiome = BiomeType.Plains;
    public Vector2 TilePostition;
    public GameObject[] Neighbours;
    public float heat;
    public float humidity;
    public void SetTileProperties(float Heat, float Humidity)
    {
        SetBiome(Heat,Humidity);
        AssignType();

    }

    void SetBiome(float Heat, float Humidity)
    {
        TileBiome = GetBiomeType(Heat, Humidity);
    }

    BiomeType GetBiomeType(float Heat, float Humidity)
    {
       Heat += transform.localScale.y / 10;
       //Heat /= 2;
       //Humidity /= 2;
       Heat *= 10;
       Humidity *= 10;

        heat = Heat;
        humidity = Humidity;
        Heat = Mathf.Clamp(Heat, 0, 6);
        Humidity = Mathf.Clamp(Humidity, 0, 6);
        return BiomeTable[(int)Humidity, (int)Heat];
    }


    void AssignType()
    {
        if (TileBiome == BiomeType.Taiga)
        {
            if (transform.localScale.y > 0.65f)
            {
                TileType = CellType.Snow;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Snow];
            }
            else if (transform.localScale.y > 0.5f)
            {
                TileType = CellType.Stone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Stone];
            }
            else if (transform.localScale.y > 0.45f)
            {
                TileType = CellType.LightStone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.LightStone];
            }
            else if (transform.localScale.y > 0.3f)
            {
                TileType = CellType.DarkGrass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.DarkGrass];
            }
            else if (transform.localScale.y > 0.2f)
            {
                TileType = CellType.Grass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Grass];
            }
            else
            {
                TileType = CellType.Water;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Water];
            }
            return;
        }

        if (TileBiome == BiomeType.Savanna)
        {
            if (transform.localScale.y > 0.65f)
            {
                TileType = CellType.LightStone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.LightStone];
            }
            else if (transform.localScale.y > 0.5f)
            {
                TileType = CellType.Dirt;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Dirt];
            }
            else if (transform.localScale.y > 0.45f)
            {
                TileType = CellType.Sand;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Sand];
            }
            else if (transform.localScale.y > 0.3f)
            {
                TileType = CellType.RedSand;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.RedSand];
            }
            else if (transform.localScale.y > 0.2f)
            {
                TileType = CellType.DarkGrass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.DarkGrass];
            }
            else
            {
                TileType = CellType.Stone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Stone];
            }
            return;
        }

        if (TileBiome == BiomeType.Tundra)
        {
            if (transform.localScale.y > 0.65f)
            {
                TileType = CellType.Snow;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Snow];
            }
            else if (transform.localScale.y > 0.5f)
            {
                TileType = CellType.Stone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Stone];
            }
            else if (transform.localScale.y > 0.45f)
            {
                TileType = CellType.LightStone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.LightStone];
            }
            else if (transform.localScale.y > 0.3f)
            {
                TileType = CellType.Grass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Grass];
            }
            else if (transform.localScale.y > 0.2f)
            {
                TileType = CellType.Ice;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Ice];
            }
            else
            {
                TileType = CellType.Water;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Water];
            }
            return;
        }

        if (TileBiome == BiomeType.RainForest)
        {
            if (transform.localScale.y > 0.65f)
            {
                TileType = CellType.Stone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Stone];
            }
            else if (transform.localScale.y > 0.5f)
            {
                TileType = CellType.LightStone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.LightStone];
            }
            else if (transform.localScale.y > 0.45f)
            {
                TileType = CellType.DarkGrass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.DarkGrass];
            }
            else if (transform.localScale.y > 0.3f)
            {
                TileType = CellType.Grass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Grass];
            }
            else if (transform.localScale.y > 0.2f)
            {
                TileType = CellType.DarkGrass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.DarkGrass];
            }
            else
            {
                TileType = CellType.Water;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Water];
            }
            return;
        }

        if (TileBiome == BiomeType.Desert)
        {
            if (transform.localScale.y > 0.65f)
            {
                TileType = CellType.LightStone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.LightStone];
            }
            else if (transform.localScale.y > 0.5f)
            {
                TileType = CellType.Sand;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Sand];
            }
            else if (transform.localScale.y > 0.45f)
            {
                TileType = CellType.DarkSand;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.DarkSand];                          
            }   
            else if (transform.localScale.y > 0.3f)
            {
                TileType = CellType.Sand;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Sand];
            }
            else if (transform.localScale.y > 0.2f)
            {
                TileType = CellType.DarkSand;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.DarkSand];
            }
            else
            {
                TileType = CellType.Dirt;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Dirt];
            }
            return;
        }

        if (TileBiome == BiomeType.Forest)
        {
            if (transform.localScale.y > 0.65f)
            {
                TileType = CellType.Stone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Stone];
            }
            else if (transform.localScale.y > 0.5f)
            {
                TileType = CellType.LightStone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.LightStone];
            }
            else if (transform.localScale.y > 0.45f)
            {
                TileType = CellType.DarkGrass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.DarkGrass];
            }
            else if (transform.localScale.y > 0.3f)
            {
                TileType = CellType.Grass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Grass];
            }
            else if (transform.localScale.y > 0.2f)
            {
                TileType = CellType.Stone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Stone];
            }
            else
            {
                TileType = CellType.Water;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Water];
            }
            return;
        }

        if (TileBiome == BiomeType.Plains)
        {
            if (transform.localScale.y > 0.65f)
            {
                TileType = CellType.LightStone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.LightStone];
            }
            else if (transform.localScale.y > 0.5f)
            {
                TileType = CellType.DarkGrass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.DarkGrass];
            }
            else if (transform.localScale.y > 0.45f)
            {
                TileType = CellType.Grass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Grass];
            }
            else if (transform.localScale.y > 0.3f)
            {
                TileType = CellType.Grass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Grass];
            }
            else if (transform.localScale.y > 0.2f)
            {
                TileType = CellType.DarkGrass;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.DarkGrass];
            }
            else
            {
                TileType = CellType.Water;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Water];
            }
            return;
        }

        if (TileBiome == BiomeType.Ice)
        {
            if (transform.localScale.y > 0.65f)
            {
                TileType = CellType.Snow;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Snow];
            }
            else if (transform.localScale.y > 0.5f)
            {
                TileType = CellType.LightStone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.LightStone];
            }
            else if (transform.localScale.y > 0.45f)
            {
                TileType = CellType.Snow;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Snow];
            }
            else if (transform.localScale.y > 0.3f)
            {
                TileType = CellType.Snow;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Snow];
            }
            else if (transform.localScale.y > 0.2f)
            {
                TileType = CellType.LightStone;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.LightStone];
            }
            else
            {
                TileType = CellType.Ice;
                GetComponent<Renderer>().material = CellMaterial[(int)CellType.Ice];
            }
            return;
        }

    }
}
