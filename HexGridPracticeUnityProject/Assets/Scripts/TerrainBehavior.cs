using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBehavior : MonoBehaviour
{
    public GameObject Tile;
    public CellBehaviour.BiomeType Biome;
    BiomeManager biomeManager;

    public List<List<Cell>> Cells;

    public Material[] TileMaterial;
    public GameObject[] OreTowers;
    public GameObject[] TreeTowers;
    public GameObject[] ShrubTowers;
    public GameObject[] AnimalTowers;
    public bool[] HasTreeType;
    public bool[] HasOreType;
    public bool[] HasShrubType;
    public bool[] HasAnimalType;

    public GameObject TowerParent;

    public int TerrainSize = 10;
    public float perlinFrequancy = 2.0f;

    public List<List<GameObject>> CellObjects;

    // Hex Properties
    int CellArrayIndex = 0;
    float HexScale = .57f;
    public int GridSize = 2;

    void Start()
    {
        TowerParent = new GameObject();
        TowerParent.name = "Towers";
        CellObjects = new List<List<GameObject>>();
        Cells = new List<List<Cell>>();
        CreateHexagon();
        #region TestingTiles I have hardcoded values that should be changed in future builds

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
        HasTreeType[1] = true;
        HasTreeType[5] = true;
        HasTreeType[3] = true;
        HasTreeType[6] = true;
        HasTreeType[7] = true;
        HasTreeType[8] = true;
        HasOreType[0] = true;
        HasOreType[3] = true;
        HasOreType[4] = true;
        HasShrubType[0] = true;
        HasShrubType[1] = true;
        HasShrubType[2] = true;
        HasShrubType[3] = true;
        HasShrubType[4] = true;
        HasAnimalType[0] = true;
        HasAnimalType[1] = true;
        HasAnimalType[2] = true;
        HasAnimalType[3] = true;
        #endregion

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            Cells[q] = new List<Cell>();

            for (int r = 0; r < CellObjects[q].Count; r++)
            {
                Cells[q].Add(CellObjects[q][r].GetComponent<Cell>());
            }
        }

        biomeManager = GetComponent<BiomeManager>();

        GenerateTerrain();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GenerateTerrain();
        }
    }
    public void GenerateTerrain()
    {
        float randomNumber = UnityEngine.Random.Range(0, 100000);
        float humidity = 0.0f;
        float altitude = 0.0f;
        TileProperties tileProperties = new TileProperties();
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
                    }
                    // Tree
                    else if (tileProperties.Towertype == 1)
                    {
                        Cells[q][r].Tower = System.Convert.ToString((BiomeManager.TreeType)tileProperties.TowerIndex);
                        Cells[q][r].TowerObject = Instantiate(TreeTowers[(int)tileProperties.TowerIndex]);
                        Cells[q][r].TowerObject.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

                    }
                    // Shrub
                    else if (tileProperties.Towertype == 2)
                    {
                        Cells[q][r].Tower = System.Convert.ToString((BiomeManager.ShrubType)tileProperties.TowerIndex);
                        Cells[q][r].TowerObject = Instantiate(ShrubTowers[(int)tileProperties.TowerIndex]);
                        Cells[q][r].TowerObject.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                    }
                    // Animals
                    else if (tileProperties.Towertype == 3)
                    {
                        Cells[q][r].Tower = System.Convert.ToString((BiomeManager.AnimalType)tileProperties.TowerIndex);
                        Cells[q][r].TowerObject = Instantiate(AnimalTowers[(int)tileProperties.TowerIndex]);
                        Cells[q][r].TowerObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    }
                    else
                    {
                        Debug.Log("Unknown Tower Type");
                    }

                    Cells[q][r].hasTower = true;
                    Cells[q][r].TowerObject.transform.position = new Vector3(CellObjects[q][r].transform.position.x, (CellObjects[q][r].transform.localScale.y / 5), CellObjects[q][r].transform.position.z);
                    Cells[q][r].TowerObject.transform.parent = TowerParent.transform;
                }
            }
        }





    }
    void CreateHexagon()
    {
        for (int q = -GridSize; q <= GridSize; q++)
        {
            CellObjects.Add(new List<GameObject>());
            Cells.Add(new List<Cell>());

            int r1 = Mathf.Max(-GridSize, -q - GridSize);
            int r2 = Mathf.Min(GridSize, -q + GridSize);

            for (int r = r1; r <= r2; r++)
            {
                CreateCell(q, r);
            }
            CellArrayIndex++;
        }
    }

    private void CreateCell(int Q, int R)
    {
        float posQ = AxialFlatToWorld(Q, R).y;
        float posR = AxialFlatToWorld(Q, R).x;

        GameObject go = Instantiate(Tile);
        go.transform.parent = transform;
        go.transform.position = new Vector3(posQ, 0, posR);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.AddComponent<MeshCollider>();
        CellObjects[CellArrayIndex].Add(go);
    }

    Vector2 AxialFlatToWorld(int q, int r)
    {
        var x = HexScale * (3.0f / 2f * q);
        var y = HexScale * (Mathf.Sqrt(3f) / 2f * q + (Mathf.Sqrt(3f) * r));

        return new Vector2(x, y);
    }
}
