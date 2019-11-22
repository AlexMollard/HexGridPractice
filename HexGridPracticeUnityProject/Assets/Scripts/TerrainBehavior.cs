using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBehavior : MonoBehaviour
{
    public CellBehaviour.BiomeType Biome;
    BiomeManager biomeManager;

    public List<List<Cell>> Cells;

    public Material[] TileMaterial;
    public int[] TileTypeCount;
    public GameObject[] OreTowers;
    public GameObject[] TreeTowers;
    public GameObject[] ShrubTowers;
    public GameObject[] AnimalTowers;
    public bool[] HasTreeType;
    public bool[] HasOreType;
    public bool[] HasShrubType;
    public bool[] HasAnimalType;
    float timer = 0.0f;

    public GameObject TowerParent;

    public float perlinFrequancy = 2.0f;

    public List<List<GameObject>> CellObjects;
    TileProperties tileProperties;
    // Hex Properties
    int CellArrayIndex = 0;
    float HexScale = .57f;
    public int GridSize = 2;

    void Start()
    {
        #region Constructing Variables
        TileTypeCount = new int[Enum.GetNames(typeof(BiomeManager.CellType)).Length + Enum.GetNames(typeof(BiomeManager.TowerType)).Length];
        tileProperties = new TileProperties();
        TowerParent = new GameObject();
        TowerParent.name = "Towers";
        //CellObjects = new List<List<GameObject>>();
        //Cells = new List<List<Cell>>();

        HasTreeType = new bool[Enum.GetNames(typeof(BiomeManager.TreeType)).Length];
        HasOreType = new bool[Enum.GetNames(typeof(BiomeManager.OreType)).Length];
        HasShrubType = new bool[Enum.GetNames(typeof(BiomeManager.ShrubType)).Length];
        HasAnimalType = new bool[Enum.GetNames(typeof(BiomeManager.AnimalType)).Length];

        for (int i = 0; i < Enum.GetNames(typeof(BiomeManager.TreeType)).Length; i++)
        {
            HasTreeType[i] = false;
        }
        for (int i = 0; i < Enum.GetNames(typeof(BiomeManager.OreType)).Length; i++)
        {
            HasOreType[i] = false;
        }
        for (int i = 0; i < Enum.GetNames(typeof(BiomeManager.ShrubType)).Length; i++)
        {
            HasShrubType[i] = false;
        }
        for (int i = 0; i < Enum.GetNames(typeof(BiomeManager.AnimalType)).Length; i++)
        {
            HasAnimalType[i] = false;
        }
        #endregion



        biomeManager = GetComponent<BiomeManager>();
    }

    public void GenerateTerrain(float randomNumber, List<List<GameObject>> gameObjects, List<List<Cell>> cells)
    {
        CellObjects = gameObjects;
        Cells = cells;

        for (int i = 0; i < TileTypeCount.Length; i++)
        {
            TileTypeCount[i] = 0;
        }


        float humidity = 0.0f;
        float altitude = 0.0f;
        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < CellObjects[q].Count; r++)
            {
                if (Cells[q][r].TowerObject)
                {
                    Destroy(Cells[q][r].TowerObject);
                }

                altitude = Mathf.Clamp(Mathf.PerlinNoise(q * perlinFrequancy + randomNumber, r * perlinFrequancy + randomNumber), 0.01f, 1);
                humidity = Mathf.Clamp(Mathf.PerlinNoise(q * (perlinFrequancy / 2) + randomNumber, r * (perlinFrequancy / 2) + randomNumber), 0.01f, 1);

                CellObjects[q][r].transform.localScale = new Vector3(1, altitude * 4, 1);
                CellObjects[q][r].transform.position = new Vector3(CellObjects[q][r].transform.position.x, 0, CellObjects[q][r].transform.position.z);

                if (Biome == CellBehaviour.BiomeType.Taiga)
                    tileProperties = biomeManager.Taiga(altitude, humidity);

                CellObjects[q][r].GetComponent<Renderer>().material = TileMaterial[tileProperties.CellType];
                TileTypeCount[tileProperties.CellType] += 1;


                if (tileProperties.CellType == 0)
                {
                    CellObjects[q][r].transform.localScale = new Vector3(1, 1f, 1);
                    CellObjects[q][r].transform.position = new Vector3(CellObjects[q][r].transform.position.x,0, CellObjects[q][r].transform.position.z);
                }

                if (tileProperties.TowerIndex != 404)
                {
                    // Ore
                    if (tileProperties.Towertype == 0)
                    {
                        Cells[q][r].Tower = System.Convert.ToString((BiomeManager.OreType)tileProperties.TowerIndex);
                        Cells[q][r].TowerObject = Instantiate(OreTowers[(int)tileProperties.TowerIndex]);
                        Cells[q][r].TowerObject.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                        HasOreType[(int)tileProperties.TowerIndex] = true;
                        TileTypeCount[Enum.GetNames(typeof(BiomeManager.CellType)).Length + tileProperties.Towertype + 1] += 1;
                    }
                    // Tree
                    else if (tileProperties.Towertype == 1)
                    {
                        Cells[q][r].Tower = System.Convert.ToString((BiomeManager.TreeType)tileProperties.TowerIndex);
                        Cells[q][r].TowerObject = Instantiate(TreeTowers[(int)tileProperties.TowerIndex]);
                        Cells[q][r].TowerObject.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                        HasTreeType[(int)tileProperties.TowerIndex] = true;
                        TileTypeCount[Enum.GetNames(typeof(BiomeManager.CellType)).Length + tileProperties.Towertype + 1] += 1;
                    }
                    // Shrub
                    else if (tileProperties.Towertype == 2)
                    {
                        Cells[q][r].Tower = System.Convert.ToString((BiomeManager.ShrubType)tileProperties.TowerIndex);
                        Cells[q][r].TowerObject = Instantiate(ShrubTowers[(int)tileProperties.TowerIndex]);
                        Cells[q][r].TowerObject.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                        HasShrubType[(int)tileProperties.TowerIndex] = true;
                        TileTypeCount[Enum.GetNames(typeof(BiomeManager.CellType)).Length + tileProperties.Towertype + 1] += 1;
                    }
                    // Animals
                    else if (tileProperties.Towertype == 3)
                    {
                        Cells[q][r].Tower = System.Convert.ToString((BiomeManager.AnimalType)tileProperties.TowerIndex);
                        Cells[q][r].TowerObject = Instantiate(AnimalTowers[(int)tileProperties.TowerIndex]);
                        Cells[q][r].TowerObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        HasAnimalType[(int)tileProperties.TowerIndex] = true;
                        TileTypeCount[Enum.GetNames(typeof(BiomeManager.CellType)).Length + tileProperties.Towertype + 1] += 1;
                    }
                    else
                    {
                        Debug.Log("Unknown Tower Type");
                    }

                    if (TowerParent)
                    {
                        Debug.Log("Tower");
                    }

                    Cells[q][r].hasTower = true;
                    Cells[q][r].TowerObject.transform.position = new Vector3(CellObjects[q][r].transform.position.x, (CellObjects[q][r].transform.localScale.y / 5), CellObjects[q][r].transform.position.z);
                    Cells[q][r].TowerObject.transform.parent = TowerParent.transform;
                }
            }
        }

    }


   
}
