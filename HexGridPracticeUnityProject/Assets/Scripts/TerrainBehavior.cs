using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBehavior : MonoBehaviour
{
    public CellBehaviour.BiomeType Biome;
    BiomeManager biomeManager;
    public List<Cell> StreamCells;
    public List<List<Cell>> Cells;
    int RiverCount = 0;

    public int[] TileTypeCount;
    public GameObject[] OreTowers;
    public GameObject[] TreeTowers;
    public GameObject[] ShrubTowers;
    public GameObject[] AnimalTowers;
    public bool[] HasTreeType;
    public bool[] HasOreType;
    public bool[] HasShrubType;
    public bool[] HasAnimalType;
    public Color[] CellColors;
    float timer = 0.0f;
    public GameObject TowerMeshParent;

    public GameObject TowerParent;

    public float perlinFrequancy = 1.0f;
    public List<List<GameObject>> CellObjects;
    int GridSize;

    void Start()
    {
        #region Constructing Variables
        StreamCells = new List<Cell>();
        CellColors = new Color[Enum.GetNames(typeof(BiomeManager.CellType)).Length];
        TileTypeCount = new int[Enum.GetNames(typeof(BiomeManager.CellType)).Length + Enum.GetNames(typeof(BiomeManager.TowerType)).Length];

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

        // Setting colors
        CellColors[(int)BiomeManager.CellType.Water] = new Color(0.3f, 0.3f, 0.7f);
        CellColors[(int)BiomeManager.CellType.Sand] = new Color(0.4f, 0.4f, 0);
        CellColors[(int)BiomeManager.CellType.Dirt] = new Color(0.4f, 0.2f, 0);
        CellColors[(int)BiomeManager.CellType.Grass] = new Color(0, 0.4f, 0);
        CellColors[(int)BiomeManager.CellType.Stone] = new Color(0.15f, 0.15f, 0.15f);
        CellColors[(int)BiomeManager.CellType.Snow] = new Color(0.4f, 0.4f, 0.4f);
        CellColors[(int)BiomeManager.CellType.Ice] = new Color(0.15f, 0.15f, 0.35f);
        CellColors[(int)BiomeManager.CellType.Lava] = new Color(0.35f, 0, 0);



        #endregion

        biomeManager = GetComponent<BiomeManager>();
    }

    public void GenerateTerrain(float randomNumber, List<List<GameObject>> gameObjects, List<List<Cell>> cells, int gridSize)
    {
        List<GameObject> Towers = new List<GameObject>();
        CellObjects = gameObjects;
        Cells = cells;
        GridSize = gridSize;
        UnityEngine.Random.InitState((int)randomNumber);

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < CellObjects[q].Count; r++)
            {
                cells[q][r].CellType = BiomeManager.CellType.Grass;
                cells[q][r].Humidity = 0;
                cells[q][r].Altitude = Mathf.Clamp(Mathf.PerlinNoise(q * perlinFrequancy + randomNumber, r * perlinFrequancy + randomNumber), 0.01f, 1);
            }
        }

        GenerateRivers(biomeManager.BiomeSeaLevel[(int)Biome]);

        for (int i = 0; i < TileTypeCount.Length; i++)
        {
            TileTypeCount[i] = 0;
        }

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < CellObjects[q].Count; r++)
            {
                if (Cells[q][r].TowerObject)
                {
                    Destroy(Cells[q][r].TowerObject);
                }


                if (Cells[q][r].CellType != BiomeManager.CellType.Water)
                {
                    Cell ClosestStream = StreamCells[0];

                    for (int i = 0; i < StreamCells.Count; i++)
                    {
                        if (HexDistance(Cells[q][r].TilePostition, StreamCells[i].TilePostition) < HexDistance(Cells[q][r].TilePostition, ClosestStream.TilePostition))
                        {
                            ClosestStream = StreamCells[i];
                        }
                    }
                    float DistanceShade = Mathf.Clamp(HexDistance(Cells[q][r].TilePostition, ClosestStream.TilePostition) / 10, 0, 0.9f);
                    Cells[q][r].Humidity = DistanceShade;
                }


                if (Biome == CellBehaviour.BiomeType.Taiga)
                    cells[q][r] = biomeManager.Taiga(cells[q][r], (int)randomNumber);
                else if (Biome == CellBehaviour.BiomeType.DeepOcean)
                    cells[q][r] = biomeManager.DeepOcean(cells[q][r], (int)randomNumber);
                else if (Biome == CellBehaviour.BiomeType.Ocean)
                    cells[q][r] = biomeManager.Ocean(cells[q][r], (int)randomNumber);
                else if (Biome == CellBehaviour.BiomeType.Beach)
                    cells[q][r] = biomeManager.Beach(cells[q][r], (int)randomNumber);

                //Mathf.Lerp(1f, 75f, altitude / 7.5f)
                if (cells[q][r].CellType != BiomeManager.CellType.Water)
                {
                    CellObjects[q][r].transform.localScale = new Vector3(2f, Mathf.Lerp(0.1f, 10.0f, Cells[q][r].Altitude), 2f);
                }
                CellObjects[q][r].transform.position = new Vector3(CellObjects[q][r].transform.position.x, 0, CellObjects[q][r].transform.position.z);

                if (Cells[q][r].TowerIndex != 404)
                {
                    // Ore
                    if (Cells[q][r].TowerType == BiomeManager.TowerType.Ore)
                    {
                        Cells[q][r].Tower = System.Convert.ToString((BiomeManager.OreType)cells[q][r].TowerIndex);
                        Cells[q][r].TowerObject = Instantiate(OreTowers[Cells[q][r].TowerIndex]);
                        Cells[q][r].TowerObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                        Cells[q][r].TowerObject.transform.name = System.Convert.ToString((BiomeManager.OreType)cells[q][r].TowerIndex);
                        HasOreType[Cells[q][r].TowerIndex] = true;
                        TileTypeCount[Enum.GetNames(typeof(BiomeManager.CellType)).Length + (int)cells[q][r].TowerType] += 1;
                    }
                    // Tree
                    else if (Cells[q][r].TowerType == BiomeManager.TowerType.Tree)
                    {
                        Cells[q][r].Tower = System.Convert.ToString((BiomeManager.TreeType)cells[q][r].TowerIndex);
                        Cells[q][r].TowerObject = Instantiate(TreeTowers[Cells[q][r].TowerIndex]);
                        Cells[q][r].TowerObject.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                        Cells[q][r].TowerObject.transform.name = System.Convert.ToString((BiomeManager.TreeType)cells[q][r].TowerIndex);
                        HasTreeType[Cells[q][r].TowerIndex] = true;
                        TileTypeCount[Enum.GetNames(typeof(BiomeManager.CellType)).Length + (int)cells[q][r].TowerType] += 1;
                    }
                    // Shrub
                    else if (Cells[q][r].TowerType == BiomeManager.TowerType.Shrub)
                    {
                        Cells[q][r].Tower = System.Convert.ToString((BiomeManager.ShrubType)cells[q][r].TowerIndex);
                        Cells[q][r].TowerObject = Instantiate(ShrubTowers[Cells[q][r].TowerIndex]);
                        Cells[q][r].TowerObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                        Cells[q][r].TowerObject.transform.name = System.Convert.ToString((BiomeManager.ShrubType)cells[q][r].TowerIndex);
                        HasShrubType[Cells[q][r].TowerIndex] = true;
                        TileTypeCount[Enum.GetNames(typeof(BiomeManager.CellType)).Length + (int)cells[q][r].TowerType] += 1;
                    }
                    // Animals
                    else if (Cells[q][r].TowerType == BiomeManager.TowerType.Animal)
                    {
                        Cells[q][r].Tower = System.Convert.ToString((BiomeManager.AnimalType)cells[q][r].TowerIndex);
                        Cells[q][r].TowerObject = Instantiate(AnimalTowers[Cells[q][r].TowerIndex]);
                        Cells[q][r].TowerObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                        Cells[q][r].TowerObject.transform.name = System.Convert.ToString((BiomeManager.AnimalType)cells[q][r].TowerIndex);
                        HasAnimalType[Cells[q][r].TowerIndex] = true;
                        TileTypeCount[Enum.GetNames(typeof(BiomeManager.CellType)).Length + (int)cells[q][r].TowerType] += 1;
                    }
                    else
                    {
                        Debug.Log("Unknown Tower Type");
                    }

                    Cells[q][r].hasTower = true;
                    Cells[q][r].TowerObject.transform.position = new Vector3(CellObjects[q][r].transform.position.x, (CellObjects[q][r].transform.localScale.y / 5), CellObjects[q][r].transform.position.z);
                    Cells[q][r].TowerObject.transform.parent = TowerParent.transform;
                    Towers.Add(Cells[q][r].TowerObject);
                }
                else
                {
                    TileTypeCount[(int)cells[q][r].CellType] += 1;
                }

            }
        }

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < CellObjects[q].Count; r++)
            {
                if (Cells[q][r].CellType != BiomeManager.CellType.Water)
                {
                    GetComponent<UVScroller>().SetTexture((float)Cells[q][r].CellType, (Cells[q][r].Humidity - 1) * -1, false, CellObjects[q][r]);
                }
            }
        }
        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < CellObjects[q].Count; r++)
            {

                if (Cells[q][r].CellType == BiomeManager.CellType.Water)
                {
                    #region Getting Neighbours
                    List<float> WaterLevels = new List<float>();
                    float waterNeighbours = 0;
                    float normalWaterLevel = 0;
                    List<Cell> neighbour = new List<Cell>();
                    List<Cell> NotWaterNeighbour = new List<Cell>();
                    bool FoundNonWater = false;


                    if (GetCellByPos(new Vector2(Cells[q][r].TilePostition.x + 1, Cells[q][r].TilePostition.y)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cells[q][r].TilePostition.x + 1, Cells[q][r].TilePostition.y)));
                        if (neighbour[neighbour.Count - 1].CellType == BiomeManager.CellType.Water)
                        {
                            waterNeighbours++;
                        }
                        else
                        {
                            FoundNonWater = true;
                        }
                    }

                    if (GetCellByPos(new Vector2(Cells[q][r].TilePostition.x, Cells[q][r].TilePostition.y + 1)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cells[q][r].TilePostition.x, Cells[q][r].TilePostition.y + 1)));
                        if (neighbour[neighbour.Count - 1].CellType == BiomeManager.CellType.Water)
                        {
                            waterNeighbours++;
                        }
                        else
                        {
                            FoundNonWater = true;
                        }
                    }

                    if (GetCellByPos(new Vector2(Cells[q][r].TilePostition.x - 1, Cells[q][r].TilePostition.y + 1)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cells[q][r].TilePostition.x - 1, Cells[q][r].TilePostition.y + 1)));
                        if (neighbour[neighbour.Count - 1].CellType == BiomeManager.CellType.Water)
                        {
                            waterNeighbours++;
                        }
                        else
                        {
                            FoundNonWater = true;
                        }
                    }

                    if (GetCellByPos(new Vector2(Cells[q][r].TilePostition.x - 1, Cells[q][r].TilePostition.y)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cells[q][r].TilePostition.x - 1, Cells[q][r].TilePostition.y)));
                        if (neighbour[neighbour.Count - 1].CellType == BiomeManager.CellType.Water)
                        {
                            waterNeighbours++;
                        }
                        else
                        {
                            FoundNonWater = true;
                        }
                    }

                    if (GetCellByPos(new Vector2(Cells[q][r].TilePostition.x, Cells[q][r].TilePostition.y - 1)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cells[q][r].TilePostition.x, Cells[q][r].TilePostition.y - 1)));
                        if (neighbour[neighbour.Count - 1].CellType == BiomeManager.CellType.Water)
                        {
                            waterNeighbours++;
                        }
                        else
                        {
                            FoundNonWater = true;
                        }
                    }

                    if (GetCellByPos(new Vector2(Cells[q][r].TilePostition.x + 1, Cells[q][r].TilePostition.y - 1)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cells[q][r].TilePostition.x + 1, Cells[q][r].TilePostition.y - 1)));
                        if (neighbour[neighbour.Count - 1].CellType == BiomeManager.CellType.Water)
                        {
                            waterNeighbours++;
                        }
                        else
                        {
                            FoundNonWater = true;
                        }
                    }

                    #endregion
                    if (FoundNonWater)
                    {
                        Cell SmallestNonWaterTile = neighbour[0];

                        for (int i = 0; i < neighbour.Count; i++)
                            if (neighbour[i].CellType != BiomeManager.CellType.Water)
                            {
                                SmallestNonWaterTile = neighbour[i];
                                FoundNonWater = true;
                                break;
                            }

                        for (int i = 0; i < neighbour.Count; i++)
                        {
                            if (neighbour[i])
                                if (neighbour[i].CellType != BiomeManager.CellType.Water)
                                {
                                    if (neighbour[i].Altitude < SmallestNonWaterTile.Altitude)
                                        SmallestNonWaterTile = neighbour[i];
                                }
                        }

                        Cells[q][r].transform.localScale = new Vector3(2, Mathf.Clamp(SmallestNonWaterTile.Altitude * 0.1f, 0.1f, 10f), 2);
                    }


                    for (int i = 0; i < neighbour.Count; i++)
                    {
                        if (neighbour[i])
                            WaterLevels.Add(neighbour[i].transform.localScale.y);
                    }

                    for (int v = 0; v < WaterLevels.Count; v++)
                        normalWaterLevel += WaterLevels[v];

                    normalWaterLevel /= WaterLevels.Count;

                    Cells[q][r].transform.localScale = new Vector3(2, Mathf.Clamp(normalWaterLevel * 0.95f, 0.1f, 10f), 2);

                    float waterDepth = Mathf.Lerp(0, 1, waterNeighbours / 6f) / 4;

                    GetComponent<UVScroller>().SetTexture((float)Cells[q][r].CellType, waterDepth, true, CellObjects[q][r]);


                }
            }
        }

        if (TowerMeshParent)
        {
            Destroy(TowerMeshParent);
        }

        TowerMeshParent = new GameObject();
        TowerMeshParent.AddComponent<MeshRenderer>();
        TowerMeshParent.AddComponent<MeshFilter>();
        MeshFilter[] meshFilters = new MeshFilter[Towers.Count];
        for (int i = 0; i < Towers.Count; i++)
        {
            meshFilters[i] = Towers[i].GetComponent<MeshFilter>();
        }

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int m = 0;
        while (m < meshFilters.Length)
        {
            combine[m].mesh = meshFilters[m].sharedMesh;
            combine[m].transform = meshFilters[m].transform.localToWorldMatrix;
            meshFilters[m].gameObject.GetComponent<MeshRenderer>().enabled = false;

            m++;
        }
        TowerMeshParent.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        TowerMeshParent.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        TowerMeshParent.transform.GetComponent<Renderer>().material = meshFilters[0].gameObject.GetComponent<Renderer>().material;
        TowerMeshParent.transform.gameObject.SetActive(true);
    }

    public void GenerateRivers(float SeaLevel)
    {
        if (Biome == CellBehaviour.BiomeType.Taiga)
            RiverCount = 1;
        else
            RiverCount = 1;

        Vector3 SeaLevelScale = new Vector3(2, Mathf.Lerp(0.1f, 10.0f, SeaLevel), 2);

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < CellObjects[q].Count; r++)
            {
                if (Cells[q][r].Altitude < SeaLevel)
                {
                    Cells[q][r].CellType = BiomeManager.CellType.Water;
                    StreamCells.Add(Cells[q][r]);
                    CellObjects[q][r].transform.localScale = SeaLevelScale;
                }
            }
        }

        GameObject[] HighestTile = new GameObject[RiverCount];
        Vector2 HigestTilePos = new Vector2(0, 0);
        int RiverLength = 300;

        for (int i = 0; i < HighestTile.Length; i++)
        {
            int LoopDestroyer = 0;
            int x = 0;
            int z = 0;
            do {
                if (LoopDestroyer > GridSize)
                    break;
               x = UnityEngine.Random.Range(0, GridSize * 2 + 1);
               z = UnityEngine.Random.Range(0, CellObjects[x].Count);
                LoopDestroyer++;
            } while (Cells[x][z].CellType != BiomeManager.CellType.Water);

            HighestTile[i] = CellObjects[x][z];
        }

        Cell[] neighbour = new Cell[6];

        for (int q = 0; q < HighestTile.Length; q++)
        {
            Cell NextHighestTile;
            HighestTile[q].GetComponent<Cell>().CellType = BiomeManager.CellType.Water;
            NextHighestTile = HighestTile[q].GetComponent<Cell>();
            HigestTilePos = HighestTile[q].GetComponent<Cell>().TilePostition;

            for (int i = 0; i < RiverLength; i++)
            {
                #region Getting Neighbours
                if (GetCellByPos(new Vector2(HigestTilePos.x + 1, HigestTilePos.y)) != null)
                {
                    neighbour[0] = GetCellByPos(new Vector2(HigestTilePos.x + 1, HigestTilePos.y));
                    NextHighestTile = neighbour[0];
                }

                if (GetCellByPos(new Vector2(HigestTilePos.x, HigestTilePos.y + 1)) != null)
                {
                    neighbour[1] = GetCellByPos(new Vector2(HigestTilePos.x, HigestTilePos.y + 1));
                    if (NextHighestTile.Altitude > neighbour[1].Altitude)
                    {
                        NextHighestTile = neighbour[1];
                    }
                }

                if (GetCellByPos(new Vector2(HigestTilePos.x - 1, HigestTilePos.y + 1)) != null)
                {
                    neighbour[2] = GetCellByPos(new Vector2(HigestTilePos.x - 1, HigestTilePos.y + 1));
                    if (NextHighestTile.Altitude > neighbour[2].Altitude)
                    {
                        NextHighestTile = neighbour[2];
                    }
                }

                if (GetCellByPos(new Vector2(HigestTilePos.x - 1, HigestTilePos.y)) != null)
                {
                    neighbour[3] = GetCellByPos(new Vector2(HigestTilePos.x - 1, HigestTilePos.y));
                    if (NextHighestTile.Altitude > neighbour[3].Altitude)
                    {
                        NextHighestTile = neighbour[3];
                    }
                }

                if (GetCellByPos(new Vector2(HigestTilePos.x, HigestTilePos.y - 1)) != null)
                {
                    neighbour[4] = GetCellByPos(new Vector2(HigestTilePos.x, HigestTilePos.y - 1));
                    if (NextHighestTile.Altitude > neighbour[4].Altitude)
                    {
                        NextHighestTile = neighbour[4];
                    }
                }

                if (GetCellByPos(new Vector2(HigestTilePos.x + 1, HigestTilePos.y - 1)) != null)
                {
                    neighbour[5] = GetCellByPos(new Vector2(HigestTilePos.x + 1, HigestTilePos.y - 1));
                    if (NextHighestTile.Altitude > neighbour[5].Altitude)
                    {
                        NextHighestTile = neighbour[5];
                    }
                }
                #endregion

                if (i == 0)
                {
                    if (NextHighestTile.gameObject == HighestTile[q])
                    {
                        break;
                    }

                    bool[] IsGonnaBeWater = new bool[6];
                    for (int v = 0; v < 6; v++)
                    {
                        IsGonnaBeWater[v] = (UnityEngine.Random.value < 0.5f ? true : false);

                        if (neighbour[v] && IsGonnaBeWater[v])
                        {
                            neighbour[v].CellType = BiomeManager.CellType.Water;
                        }
                    }
                }
                else if (NextHighestTile.GetComponent<Cell>().CellType == BiomeManager.CellType.Water)
                {
                    break;
                }
                else if (NextHighestTile.gameObject == HighestTile[q])
                {
                    for (int v = 0; v < 6; v++)
                    {
                        if (neighbour[v])
                        {
                            neighbour[v].CellType = BiomeManager.CellType.Water;
                        }
                    }

                    break;
                }

                NextHighestTile.GetComponent<Cell>().CellType = BiomeManager.CellType.Water;
                StreamCells.Add(NextHighestTile);
                HighestTile[q] = NextHighestTile.gameObject;
                HigestTilePos = HighestTile[q].GetComponent<Cell>().TilePostition;
            }
        }
    }

    #region HexStuff
    public Cell GetCellByPos(Vector2 pos)
    {
        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < CellObjects[q].Count; r++)
            {
                if (Cells[q][r].TilePostition.x == pos.x && Cells[q][r].TilePostition.y == pos.y)
                {
                    if (Cells[q][r])
                    {
                        return Cells[q][r];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        return null;
    }

    Vector3 axial_to_cube(Vector2 Hex)
    {
        var x = Hex.x;
        var z = Hex.y;
        var y = -x - z;
        return new Vector3(x, y, z);
    }

    float cube_distance(Vector3 HexOne, Vector3 HexTwo)
    {
        return Mathf.Max(Mathf.Abs(HexOne.x - HexTwo.x), Mathf.Abs(HexOne.y - HexTwo.y), Mathf.Abs(HexOne.z - HexTwo.z));
    }

    float HexDistance(Vector2 HexOne, Vector2 HexTwo)
    {
        Vector3 ac = axial_to_cube(HexOne);
        Vector3 bc = axial_to_cube(HexTwo);
        return cube_distance(ac, bc);
    }
    #endregion
}
