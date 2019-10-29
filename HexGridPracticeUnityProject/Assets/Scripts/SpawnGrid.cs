using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public int GridSize = 2;
    public int DectectionRadius = 10;

    // Noise Properties
    public float[][] BiomeHeat;
    public float[][] BiomeHumidity;

    // Terrain Noise
    public float TerrainRandNum;
    public float TerrainFrequancy = 0.01f;

    // Biome Noise
    public float BiomeRandNum;
    public float BiomeFrequancy = 0.01f;

    // Temp Variables
    float UpdateTimer = 0.0f;

    void Start()
    {
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

    void GenerateMainTerrainPerlinNoise()
    {

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < goCell[q].Count; r++)
            {
                    Vector2 pos = AxialFlatToWorld((int)Cell[q][r].TilePostition.x, (int)Cell[q][r].TilePostition.y);

                    float Noise = Mathf.PerlinNoise((pos.x * TerrainFrequancy + TerrainRandNum) / 2, pos.y * TerrainFrequancy + TerrainRandNum);

                    Cell[q][r].transform.localScale = new Vector3(1, Noise, 1);

                    Cell[q][r].SetTileProperties(BiomeHeat[q][r], BiomeHumidity[q][r]);
            }
        }
    }

    // Biome Generator
    void GenerateBiomePerlinNoise()
    {
        BiomeHeat = new float[GridSize * 2 + 1][];
        BiomeHumidity = new float[GridSize * 2 + 1][];
        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            BiomeHeat[q] = new float[goCell[q].Count];
            BiomeHumidity[q] = new float[goCell[q].Count];
            for (int r = 0; r < goCell[q].Count; r++)
            {
                Vector2 pos = AxialFlatToWorld((int)Cell[q][r].TilePostition.x, (int)Cell[q][r].TilePostition.y);
                BiomeHeat[q][r] = Mathf.PerlinNoise((pos.x * BiomeFrequancy + BiomeRandNum) / 2, pos.y * BiomeFrequancy + BiomeRandNum);
                BiomeHumidity[q][r] = Mathf.PerlinNoise((pos.x * BiomeFrequancy + (BiomeRandNum * 1.5f)) / 2, pos.y * BiomeFrequancy + (BiomeRandNum * 1.5f));
            }
        }
    }

    void GenerateTerrain()
    {
        TerrainRandNum = UnityEngine.Random.Range(0, 99999);
        BiomeRandNum = UnityEngine.Random.Range(0, 99999);

        GenerateBiomePerlinNoise();
        GenerateMainTerrainPerlinNoise();
    }


    private void Update()
    {
        UpdateTimer += Time.deltaTime * 10;
        if (Input.GetMouseButtonUp(0) || UpdateTimer > 1)
        {
            TerrainRandNum += 0.05f;
            BiomeRandNum -= 0.05f;
            GenerateBiomePerlinNoise();
            GenerateMainTerrainPerlinNoise();
            UpdateTimer = 0.0f;
        }    
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
        go.name = Q + ", " + R;

        go.transform.position = new Vector3(posQ, 0, posR);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.GetComponent<CellBehaviour>().TilePostition = new Vector2(Q, R);

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
