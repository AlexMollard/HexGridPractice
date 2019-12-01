using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SpawnGrid : MonoBehaviour
{
    // Tile Mesh
    public GameObject TilePrefab;

    // Hex Directions
    int[,] axial_directions = new int[6, 2] {
        {2, -1}, {1, -2}, {-1, -1},
        {-2, 1}, {-1, 2}, {1, 1}
    };

    // Hex Storage
    public List<List<GameObject>> goCell;
    public CellBehaviour[][] Cell;
    int CellArrayIndex = 0;

    // Hex Properties
    float HexScale = .57f;
    public int GridSize = 10;

    // Noise Properties
    public float[][] BiomeHumidity;
    public float[][] Noise;
    public float[][] Bounds;

    // Terrain Noise
    float TerrainRandNum;
    public float TerrainFrequancy = 0.03f;

    // Biome Noise
    float BiomeRandNum;
    public float BiomeFrequancy = 0.15f;

    // Decoration Vaiables
    public List<GameObject> SnowTrees;
    public List<GameObject> Trees;

    // Temp Variables
    [Range(0, 10)]
    public float PowValue = 6f;
    [Range(0, 10)]
    public float BiomePowValue = 6f;
    public Button menuButton;
    public TextMeshProUGUI tileDisplay;
    int HexSize = 0;
    int iteration = 0;
    bool FirstOff = false;
    public bool isReady = false;
    List<GameObject> Chunks;
    float timer = 0.0f;
    Vector3 startupScale;
    public GameObject Towers;
    public GameObject ChunkParent;
    GameObject BigCellParent;
    public bool IsOnMap = true;
    public Material UVMap;
    public List<CellBehaviour> StreamCells;
    //TEXTURE SETTINGS
    int texWidth = 0;
    int texHeight = 0;
 
     //MASK SETTINGS
    float maskThreshold = 2f;

    //REFERENCES
    Texture2D mask;
    public void Awake()
    {
        BigCellParent = new GameObject();
        BigCellParent.name = "Map Cells";
        BigCellParent.transform.parent = transform;
        Towers = new GameObject();
        Towers.name = "Towers";
        Chunks = new List<GameObject>();
        SnowTrees = new List<GameObject>();
        Trees = new List<GameObject>();
        HexSize = GridSize * 2 + 1;
    }

    void Start()
    {
        FirstOff = false;
        menuButton.onClick.AddListener(MainMenu);
        goCell = new List<List<GameObject>>();
        CreateHexagon();

        Cell = new CellBehaviour[GridSize * 2 + 1][];

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            Cell[q] = new CellBehaviour[goCell[q].Count];

            for (int r = 0; r < goCell[q].Count; r++)
            {
                Cell[q][r] = goCell[q][r].GetComponent<CellBehaviour>();
                Cell[q][r].terrainLoader = GetComponent<TerrainLoader>();
            }
        }

        GenerateTerrain();
    }

    private void Update()
    {
        if (IsOnMap)
        {
            if (Camera.main.transform.position.y < 20 && isReady)
            {
                FirstOff = false;
                iteration++;
                if (iteration == 1)
                {
                    for (int q = 0; q < HexSize / 6; q++)
                    {
                        for (int r = 0; r < goCell[q].Count; r++)
                        {
                            Cell[q][r].CheckTreeVisable(true);
                        }
                    }
                }
                else if (iteration == 2)
                {
                    for (int q = (HexSize / 6) * 1; q < (HexSize / 6) * 2; q++)
                    {
                        for (int r = 0; r < goCell[q].Count; r++)
                        {
                            Cell[q][r].CheckTreeVisable(true);
                        }
                    }
                }
                else if (iteration == 3)
                {
                    for (int q = (HexSize / 6) * 2; q < (HexSize / 6) * 3; q++)
                    {
                        for (int r = 0; r < goCell[q].Count; r++)
                        {
                            Cell[q][r].CheckTreeVisable(true);
                        }
                    }
                }
                else if (iteration == 4)
                {
                    for (int q = (HexSize / 6) * 3; q < (HexSize / 6) * 4; q++)
                    {
                        for (int r = 0; r < goCell[q].Count; r++)
                        {
                            Cell[q][r].CheckTreeVisable(true);
                        }
                    }
                }
                else if (iteration == 5)
                {
                    for (int q = (HexSize / 6) * 4; q < (HexSize / 6) * 5; q++)
                    {
                        for (int r = 0; r < goCell[q].Count; r++)
                        {
                            Cell[q][r].CheckTreeVisable(true);
                        }
                    }
                }
                else if (iteration == 6)
                {
                    for (int q = (HexSize / 6) * 5; q < HexSize; q++)
                    {
                        for (int r = 0; r < goCell[q].Count; r++)
                        {
                            Cell[q][r].CheckTreeVisable(true);
                        }
                    }
                    iteration = 0;
                }

            }
            else if (!FirstOff)
            {
                for (int q = 0; q < HexSize; q++)
                {
                    for (int r = 0; r < goCell[q].Count; r++)
                    {
                        Cell[q][r].CheckTreeVisable(false);
                    }
                }
                FirstOff = true;
            }
            else
            {
                timer += Time.deltaTime * 0.5f;
                startupScale = new Vector3(1, Mathf.Clamp(Mathf.Lerp(0, 1, timer), 0, 1), 1);
                for (int i = 0; i < Chunks.Count; i++)
                {
                    Chunks[i].transform.localScale = startupScale;
                }

                if (Chunks[0].transform.localScale.y >= 1)
                {
                    for (int i = 0; i < Chunks.Count; i++)
                    {
                        Chunks[i].transform.localScale = new Vector3(1, 1, 1); ;
                    }
                    isReady = true;
                }
            }
        }


    }

    void GenerateMainTerrainPerlinNoise()
    {
        Noise = new float[GridSize * 2 + 1][];
        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            Noise[q] = new float[goCell[q].Count];
            for (int r = 0; r < goCell[q].Count; r++)
            {
                Vector2 pos = AxialFlatToWorld((int)Cell[q][r].TilePostition.x, (int)Cell[q][r].TilePostition.y);

                Noise[q][r] = Mathf.PerlinNoise((pos.x * TerrainFrequancy + TerrainRandNum), pos.y * TerrainFrequancy + TerrainRandNum);
                Noise[q][r] += 0.5f * Mathf.PerlinNoise((2 * pos.x * TerrainFrequancy + TerrainRandNum), 2 * pos.y * TerrainFrequancy + TerrainRandNum);
                Noise[q][r] += 0.25f * Mathf.PerlinNoise((4 * pos.x * TerrainFrequancy + TerrainRandNum), 4 * pos.y * TerrainFrequancy + TerrainRandNum);

                Noise[q][r] = Mathf.Pow(Noise[q][r], PowValue);
            }
        }
    }


    void GenerateTexture()
    {
        Vector2 maskCenter = new Vector3(0, 0);

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < goCell[q].Count; r++)
            { 
                float distFromCenter = Vector2.Distance(maskCenter, AxialFlatToWorld((int)Cell[q][r].TilePostition.x, (int)Cell[q][r].TilePostition.y));
                float maskPixel = (0.5f - (distFromCenter / goCell[q].Count) );
               // Noise[q][r] += maskPixel;
            }
        }

    }
    // Biome Generator
    void GenerateBiomePerlinNoise()
    {
        BiomeHumidity = new float[GridSize * 2 + 1][];
        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            BiomeHumidity[q] = new float[goCell[q].Count];
            for (int r = 0; r < goCell[q].Count; r++)
            {
                Vector2 pos = AxialFlatToWorld((int)Cell[q][r].TilePostition.x, (int)Cell[q][r].TilePostition.y);
                BiomeHumidity[q][r] = Mathf.Pow(Mathf.PerlinNoise((pos.x * BiomeFrequancy + (TerrainRandNum)), pos.y * BiomeFrequancy + (TerrainRandNum)), BiomePowValue);
            }
        }
    }

    public void SetTerrain()
    {
        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < goCell[q].Count; r++)
            {
                Cell[q][r].SetTileProperties(Noise[q][r], BiomeHumidity[q][r]);
            }
        }
        
        GenerateRivers();

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < goCell[q].Count; r++)
            {
                if (Cell[q][r].TileBiome != CellBehaviour.BiomeType.Ocean)
                {
                    CellBehaviour ClosestStream = StreamCells[0];

                    for (int i = 0; i < StreamCells.Count; i++)
                    {
                        if (HexDistance(Cell[q][r].TilePostition, StreamCells[i].TilePostition) < HexDistance(Cell[q][r].TilePostition, ClosestStream.TilePostition))
                        {
                            ClosestStream = StreamCells[i];
                        }
                    }
                    float DistanceShade = Mathf.Clamp(HexDistance(Cell[q][r].TilePostition, ClosestStream.TilePostition) / 10, 0, 0.9f);
                    Cell[q][r].humidity = Mathf.Clamp(Cell[q][r].humidity + DistanceShade / 2,0,1);
                    Cell[q][r].GetComponent<UVScroller>().SetTexture(ObjectType.Biome, Cell[q][r].gameObject, (int)Cell[q][r].TileType, (Cell[q][r].humidity - 1) * -1);
                }

                if (Cell[q][r].TileBiome == CellBehaviour.BiomeType.Ocean)
                {
                    #region Getting Neighbours
                    List<float> WaterLevels = new List<float>();
                    float waterNeighbours = 0;
                    float normalWaterLevel = 0;
                    List<CellBehaviour> neighbour = new List<CellBehaviour>();
                    List<CellBehaviour> NotWaterNeighbour = new List<CellBehaviour>();
                    bool FoundNonWater = false;


                    if (GetCellByPos(new Vector2(Cell[q][r].TilePostition.x + 1, Cell[q][r].TilePostition.y)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cell[q][r].TilePostition.x + 1, Cell[q][r].TilePostition.y)));
                        if (neighbour[neighbour.Count - 1].TileBiome == CellBehaviour.BiomeType.Ocean)
                        {
                            waterNeighbours++;
                        }
                        else
                        {
                            FoundNonWater = true;
                        }
                    }

                    if (GetCellByPos(new Vector2(Cell[q][r].TilePostition.x, Cell[q][r].TilePostition.y + 1)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cell[q][r].TilePostition.x, Cell[q][r].TilePostition.y + 1)));
                        if (neighbour[neighbour.Count - 1].TileBiome == CellBehaviour.BiomeType.Ocean)
                        {
                            waterNeighbours++;
                        }
                        else
                        {
                            FoundNonWater = true;
                        }
                    }

                    if (GetCellByPos(new Vector2(Cell[q][r].TilePostition.x - 1, Cell[q][r].TilePostition.y + 1)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cell[q][r].TilePostition.x - 1, Cell[q][r].TilePostition.y + 1)));
                        if (neighbour[neighbour.Count - 1].TileBiome == CellBehaviour.BiomeType.Ocean)
                        {
                            waterNeighbours++;
                        }
                        else
                        {
                            FoundNonWater = true;
                        }
                    }

                    if (GetCellByPos(new Vector2(Cell[q][r].TilePostition.x - 1, Cell[q][r].TilePostition.y)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cell[q][r].TilePostition.x - 1, Cell[q][r].TilePostition.y)));
                        if (neighbour[neighbour.Count - 1].TileBiome == CellBehaviour.BiomeType.Ocean)
                        {
                            waterNeighbours++;
                        }
                        else
                        {
                            FoundNonWater = true;
                        }
                    }

                    if (GetCellByPos(new Vector2(Cell[q][r].TilePostition.x, Cell[q][r].TilePostition.y - 1)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cell[q][r].TilePostition.x, Cell[q][r].TilePostition.y - 1)));
                        if (neighbour[neighbour.Count - 1].TileBiome == CellBehaviour.BiomeType.Ocean)
                        {
                            waterNeighbours++;
                        }
                        else
                        {
                            FoundNonWater = true;
                        }
                    }

                    if (GetCellByPos(new Vector2(Cell[q][r].TilePostition.x + 1, Cell[q][r].TilePostition.y - 1)) != null)
                    {
                        neighbour.Add(GetCellByPos(new Vector2(Cell[q][r].TilePostition.x + 1, Cell[q][r].TilePostition.y - 1)));
                        if (neighbour[neighbour.Count - 1].TileBiome == CellBehaviour.BiomeType.Ocean)
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
                        CellBehaviour SmallestNonWaterTile = neighbour[0];

                        for (int i = 0; i < neighbour.Count; i++)
                            if (neighbour[i].TileBiome != CellBehaviour.BiomeType.Ocean)
                            {
                                SmallestNonWaterTile = neighbour[i];
                                FoundNonWater = true;
                                break;
                            }

                        for (int i = 0; i < neighbour.Count; i++)
                        {
                            if (neighbour[i])
                                if (neighbour[i].TileBiome != CellBehaviour.BiomeType.Ocean)
                                {
                                    if (neighbour[i].altitude < SmallestNonWaterTile.altitude)
                                        SmallestNonWaterTile = neighbour[i];
                                }
                        }

                        Cell[q][r].transform.localScale = new Vector3(1, Mathf.Clamp(SmallestNonWaterTile.altitude * 0.1f, 0.1f, 1f), 1);
                    }


                    for (int i = 0; i < neighbour.Count; i++)
                    {
                        if (neighbour[i])
                            WaterLevels.Add(neighbour[i].transform.localScale.y);
                    }

                    for (int v = 0; v < WaterLevels.Count; v++)
                        normalWaterLevel += WaterLevels[v];

                    normalWaterLevel /= WaterLevels.Count;

                    Cell[q][r].transform.localScale = new Vector3(1, normalWaterLevel, 1);

                    float waterDepth = Mathf.Lerp(0, 1, waterNeighbours / 6f) * 0.75f;

                    Cell[q][r].GetComponent<UVScroller>().SetTexture(ObjectType.Biome, Cell[q][r].gameObject, (float)Cell[q][r].TileBiome, Mathf.Clamp(waterDepth + (Cell[q][r].humidity * 0.25f), 0.1f, 1f));
                }

            }
        }
    }
    public void GenerateRivers()
    {
        float SeaLevel = 0.1f;
        int RiverCount = 0;
        CellBehaviour HighestTileOnMap = Cell[0][0];
        List<GameObject> HighestTile = new List<GameObject>();
        HighestTile.Add(goCell[0][0]);
        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < goCell[q].Count; r++)
            {
                if (Cell[q][r].altitude < SeaLevel)
                {
                    Cell[q][r].TileBiome = CellBehaviour.BiomeType.Ocean;
                    StreamCells.Add(Cell[q][r]);
                    Cell[q][r].transform.localScale = new Vector3(1, SeaLevel, 1);
                }

                if (HighestTileOnMap.altitude < Cell[q][r].altitude)
                    HighestTileOnMap = Cell[q][r];

                //if (HexDistance(HighestTile[0].GetComponent<CellBehaviour>().TilePostition, Cell[q][r].TilePostition) > )
                //{
                //
                //}
            }
        }
        HighestTile[0] = HighestTileOnMap.gameObject;

        int RiverLength = 200;
        CellBehaviour[] neighbours = null;
        for (int i = 0; i < 0; i++)
        {
            neighbours = GetNeighbours(HighestTile[i].GetComponent<CellBehaviour>());
            for (int z = 0; z < neighbours.Length; z++)
            {
                neighbours[z].GetComponent<CellBehaviour>().TileBiome = CellBehaviour.BiomeType.Ocean;
                StreamCells.Add(neighbours[z]);
                HighestTile.Add(neighbours[z].gameObject);
                CellBehaviour[] Innerneighbours = null;
                if (neighbours[z] != null)
                {
                    Innerneighbours = GetNeighbours(neighbours[z].GetComponent<CellBehaviour>());
                    for (int p = 0; p < Innerneighbours.Length; p++)
                    {
                        Innerneighbours[p].GetComponent<CellBehaviour>().TileBiome = CellBehaviour.BiomeType.Ocean;
                        StreamCells.Add(Innerneighbours[p]);
                        HighestTile.Add(Innerneighbours[p].gameObject);
                        Innerneighbours[p].altitude *= 0.9f;
                    }
                }
            }
        }

        CellBehaviour[] neighbour = new CellBehaviour[6];

        for (int q = 0; q < HighestTile.Count; q++)
        {
            CellBehaviour NextHighestTile;
            CellBehaviour SecondHighestTile = null;
            HighestTile[q].GetComponent<CellBehaviour>().TileBiome = CellBehaviour.BiomeType.Ocean;
            NextHighestTile = HighestTile[q].GetComponent<CellBehaviour>();

            for (int i = 0; i < RiverLength; i++)
            {
                #region Getting Neighbours

                neighbour = GetNeighbours(HighestTile[q].GetComponent<CellBehaviour>());

                NextHighestTile = HighestTileOnMap;

                for (int l = 0; l < 6; l++)
                {
                    if (neighbour[l] != null)
                    {
                        if (NextHighestTile.altitude > neighbour[l].altitude)
                        {
                            SecondHighestTile = NextHighestTile;
                            NextHighestTile = neighbour[l];
                        }
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
                            neighbour[v].TileBiome = CellBehaviour.BiomeType.Ocean;
                        }
                    }
                }
                else if (NextHighestTile.GetComponent<CellBehaviour>().TileBiome == CellBehaviour.BiomeType.Ocean)
                {
                    break;
                }
                else if (NextHighestTile.gameObject == HighestTile[q])
                {
                    for (int v = 0; v < 6; v++)
                    {
                        if (neighbour[v])
                        {
                            neighbour[v].TileBiome = CellBehaviour.BiomeType.Ocean;
                        }
                    }
                    break;
                }

                if (SecondHighestTile)
                {
                    StreamCells.Add(SecondHighestTile);
                    SecondHighestTile.GetComponent<CellBehaviour>().TileBiome = CellBehaviour.BiomeType.Ocean;
                }

                NextHighestTile.GetComponent<CellBehaviour>().TileBiome = CellBehaviour.BiomeType.Ocean;
                StreamCells.Add(NextHighestTile);
                HighestTile[q] = NextHighestTile.gameObject;
            }
        }
    }

    CellBehaviour[] GetNeighbours(CellBehaviour cell)
    {
        CellBehaviour[] neighbour = new CellBehaviour[6];
        Vector2 TilePos = cell.TilePostition;

        if (GetCellByPos(new Vector2(TilePos.x + 1, TilePos.y)) != null)
            neighbour[0] = GetCellByPos(new Vector2(TilePos.x + 1, TilePos.y));
        else
            neighbour[0] = null;

        if (GetCellByPos(new Vector2(TilePos.x, TilePos.y + 1)) != null)
            neighbour[1] = GetCellByPos(new Vector2(TilePos.x, TilePos.y + 1));
        else
            neighbour[1] = null;

        if (GetCellByPos(new Vector2(TilePos.x - 1, TilePos.y + 1)) != null)
            neighbour[2] = GetCellByPos(new Vector2(TilePos.x - 1, TilePos.y + 1));
        else
            neighbour[2] = null;

        if (GetCellByPos(new Vector2(TilePos.x - 1, TilePos.y)) != null)
            neighbour[3] = GetCellByPos(new Vector2(TilePos.x - 1, TilePos.y));
        else
            neighbour[3] = null;

        if (GetCellByPos(new Vector2(TilePos.x, TilePos.y - 1)) != null)
            neighbour[4] = GetCellByPos(new Vector2(TilePos.x, TilePos.y - 1));
        else
            neighbour[4] = null;

        if (GetCellByPos(new Vector2(TilePos.x + 1, TilePos.y - 1)) != null)
            neighbour[5] = GetCellByPos(new Vector2(TilePos.x + 1, TilePos.y - 1));
        else
            neighbour[5] = null;

        return neighbour;
    }

    void GenerateTerrain()
    {
        TerrainRandNum = UnityEngine.Random.Range(0, 99999);
        BiomeRandNum = UnityEngine.Random.Range(0, 99999);

        GenerateMainTerrainPerlinNoise();
        GenerateTexture();
        GenerateBiomePerlinNoise();
        SetTerrain();

        List<List<GameObject>> CellsToCombine = new List<List<GameObject>>();
        MeshFilter[] meshFilters = null;
        CombineInstance[] combine = null;
        GameObject newChunk = null;

        ChunkParent = new GameObject();
        ChunkParent.name = "Chunk Meshes";
        int celltocombineindex = 0;
        CellsToCombine.Add(new List<GameObject>());

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < goCell[q].Count; r++)
            {
                CellsToCombine[celltocombineindex].Add(goCell[q][r]);

                if (CellsToCombine[celltocombineindex].Count > 1500)
                {
                    CellsToCombine.Add(new List<GameObject>());
                    celltocombineindex++;
                }
            }
        }

        for (int i = 0; i < CellsToCombine.Count; i++)
        {
            meshFilters = new MeshFilter[CellsToCombine[i].Count];

            for (int z = 0; z < CellsToCombine[i].Count; z++)
            {
                meshFilters[z] = CellsToCombine[i][z].GetComponent<MeshFilter>();
            }

            combine = new CombineInstance[meshFilters.Length];

            int x = 0;
            while (x < meshFilters.Length)
            {
                combine[x].mesh = meshFilters[x].sharedMesh;
                combine[x].transform = meshFilters[x].transform.localToWorldMatrix;
                CellsToCombine[i][x].GetComponent<MeshRenderer>().enabled = false;
                x++;
            }

            newChunk = new GameObject();
            newChunk.AddComponent<MeshFilter>();
            newChunk.AddComponent<MeshRenderer>();
            newChunk.transform.GetComponent<MeshFilter>().mesh = new Mesh();
            newChunk.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            newChunk.transform.name = "Chunk" + i;
            newChunk.GetComponent<MeshRenderer>().sharedMaterial = UVMap;
            newChunk.transform.gameObject.SetActive(true);
            newChunk.transform.localScale = new Vector3(1, 0.0f, 1);
            newChunk.transform.SetParent(ChunkParent.transform);
            Chunks.Add(newChunk);

        }
    } 

    public void SetText(string inputText)
    {
        tileDisplay.text = inputText;
    }
    void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    //----------------------
    //  HEX FUNCTIONS
    //----------------------
    void CreateHexagon()
    {
        for (int q = -GridSize; q <= GridSize; q++)
        {
            goCell.Add(new List<GameObject>());

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

        GameObject go = Instantiate(TilePrefab);
        go.transform.parent = BigCellParent.transform;
        go.transform.position = new Vector3(posQ, 0, posR);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.GetComponent<CellBehaviour>().TilePostition = new Vector2(Q, R);
        go.AddComponent<MeshCollider>();
        goCell[CellArrayIndex].Add(go);
    }

    Vector2 AxialFlatToWorld(int q, int r)
    {
        var x = HexScale * (3.0f / 2f * q);
        var y = HexScale * (Mathf.Sqrt(3f) / 2f * q + (Mathf.Sqrt(3f) * r));

        return new Vector2(x, y);
    }

    Vector2 AxialPointToWorld(int q, int r)
    {
        var x = HexScale * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
        var y = HexScale * (3.0f / 2f * r);

        return new Vector2(x, y);
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

    CellBehaviour GetCellByPos(Vector2 pos)
    {
        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < goCell[q].Count; r++)
            {
                if (Cell[q][r].TilePostition.x == pos.x && Cell[q][r].TilePostition.y == pos.y)
                {
                    return Cell[q][r];
                }
            }
        }
        return null;
    }

    Vector2 CubeToAxiel(Vector3 cube)
    {
        var q = cube.x;
        var r = cube.z;
        return new Vector2(q, r);
    }

    Vector2 RotateCellRight(Vector2 cellPos, Vector3 C, int i)
    {
        Vector3 P = axial_to_cube(cellPos);
        Vector3 PFromC = P - C;
        Vector3 RFromC = new Vector3();

        float x = PFromC.x * -1;
        float y = PFromC.y * -1;
        float z = PFromC.z * -1;

        RFromC = new Vector3(z, x, y);

        Vector3 R = RFromC + C;
        return CubeToAxiel(R);
    }

}
