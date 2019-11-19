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
    public List<List<GameObject>> CellByType;
    public CellBehaviour[][] Cell;
    int CellArrayIndex = 0;

    // Hex Properties
    float HexScale = .57f;
    public int GridSize = 2;

    // Noise Properties
    public float[][] BiomeHumidity;

    // Terrain Noise
    float TerrainRandNum;
    float TerrainFrequancy = 0.03f;

    // Biome Noise
    float BiomeRandNum;
    float BiomeFrequancy = 0.15f;

    // Decoration Vaiables
    public List<GameObject> SnowTrees;
    public List<GameObject> Trees;

    // Temp Variables
    public float PowValue = 6f;
    public Button menuButton;
    public TextMeshProUGUI tileDisplay;
    int HexSize = 0;
    int iteration = 0;
    bool FirstOff = false;
    public void Awake()
    {
        SnowTrees = new List<GameObject>();
        Trees = new List<GameObject>();
        CellByType = new List<List<GameObject>>();
        HexSize = GridSize * 2 + 1;

        for (int i = 0; i < 16; i++)
        {
            CellByType.Add(new List<GameObject>());
        }
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
            }
        }

        GenerateTerrain();
    }

    private void Update()
    {
        if (Camera.main.transform.position.y < 18)
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
        else if(!FirstOff)
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
    }

    void GenerateMainTerrainPerlinNoise()
    {

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < goCell[q].Count; r++)
            {
                Vector2 pos = AxialFlatToWorld((int)Cell[q][r].TilePostition.x, (int)Cell[q][r].TilePostition.y);

                float Noise =  Mathf.PerlinNoise((pos.x * TerrainFrequancy + TerrainRandNum), pos.y * TerrainFrequancy + TerrainRandNum);
                Noise += 0.5f * Mathf.PerlinNoise((2 * pos.x * TerrainFrequancy + TerrainRandNum), 2 * pos.y * TerrainFrequancy + TerrainRandNum);
                Noise += 0.25f * Mathf.PerlinNoise((4 * pos.x * TerrainFrequancy + TerrainRandNum), 4 * pos.y * TerrainFrequancy + TerrainRandNum);

                Noise = Mathf.Pow(Noise, PowValue);

                Cell[q][r].SetTileProperties(Noise, BiomeHumidity[q][r]);
                CellByType[(int)Cell[q][r].TileBiome].Add(goCell[q][r]);
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
                BiomeHumidity[q][r] = Mathf.PerlinNoise((pos.x * BiomeFrequancy + (TerrainRandNum)) , pos.y * BiomeFrequancy + (TerrainRandNum));
            }
        }
    }

    void GenerateTerrain()
    {
        TerrainRandNum = UnityEngine.Random.Range(0, 99999);
        BiomeRandNum = UnityEngine.Random.Range(0, 99999);

        GenerateBiomePerlinNoise();
        GenerateMainTerrainPerlinNoise();


        for (int i = 0; i < CellByType.Count; i++)
        {
            if (CellByType[i].Count > 0)
            {
                // CellBehaviour TempCellBehaviour = new CellBehaviour();
                MeshFilter[] meshFilters = new MeshFilter[CellByType[i].Count];
                CombineInstance[] combine = new CombineInstance[meshFilters.Length];
                GameObject newChunk = new GameObject();
                for (int z = 0; z < CellByType[i].Count; z++)
                {
                    meshFilters[z] = CellByType[i][z].GetComponent<MeshFilter>();
                }


                int x = 0;
                while (x < meshFilters.Length)
                {
                    combine[x].mesh = meshFilters[x].sharedMesh;
                    combine[x].transform = meshFilters[x].transform.localToWorldMatrix;
                    CellByType[i][x].GetComponent<MeshRenderer>().enabled = false;
                    x++;
                }
                newChunk.AddComponent<MeshFilter>();
                newChunk.AddComponent<MeshRenderer>();
                newChunk.transform.GetComponent<MeshFilter>().mesh = new Mesh();
                newChunk.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
                newChunk.transform.name = System.Convert.ToString((CellBehaviour.BiomeType)i);
                newChunk.GetComponent<MeshRenderer>().material = CellByType[i][0].GetComponent<CellBehaviour>().CellMaterial[(int)CellByType[i][0].GetComponent<CellBehaviour>().TileBiome];
                newChunk.transform.gameObject.SetActive(true);
            }
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
        go.transform.parent = transform;
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

    CellBehaviour GetCellByPos(int x, int y)
    {
        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < goCell[q].Count; r++)
            {
                if (Cell[q][r].TilePostition.x == x && Cell[q][r].TilePostition.y == y)
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
