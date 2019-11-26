﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoader : MonoBehaviour
{
    public GameObject Tile;
    TerrainBehavior CurrentTerrainBehavior;

    // Hex Properties
    int CellArrayIndex = 0;
    float HexScale = 1.14f;
    public int GridSize = 10;
    public List<List<GameObject>> CellObjects;
    public List<List<Cell>> Cells;
    GameObject SmallCellsParent;
    public List<Vector2> TilePostition;
    GameObject MeshParent;
    int tempint = 0;
    private void Start()
    {
        TilePostition = new List<Vector2>();
        SmallCellsParent = new GameObject();
        SmallCellsParent.name = "Inner Cells";
        SmallCellsParent.transform.parent = transform;
        CurrentTerrainBehavior = null;
        CellObjects = new List<List<GameObject>>();
        Cells = new List<List<Cell>>();
        CreateHexagon();

        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            Cells[q] = new List<Cell>();

            for (int r = 0; r < CellObjects[q].Count; r++)
            {
                Cells[q].Add(CellObjects[q][r].GetComponent<Cell>());
                Cells[q][r].TilePostition = TilePostition[tempint];
                tempint++;
            }
        }
    }

    public void LoadTerrain(TerrainBehavior terrain, float randNumber, GameObject TowersParent)
    {
        if (CurrentTerrainBehavior)
            Destroy(CurrentTerrainBehavior.TowerMeshParent);

        CurrentTerrainBehavior = terrain;
        //CurrentTerrainBehavior.Biome = GetComponent<CellBehaviour>().TileBiome;

        CurrentTerrainBehavior.TowerParent = TowersParent;
        CurrentTerrainBehavior.GenerateTerrain(randNumber, CellObjects, Cells, GridSize);

        if (MeshParent)
        {
            Destroy(MeshParent);
        }

            MeshParent = new GameObject();
            MeshParent.AddComponent<MeshRenderer>();
            MeshParent.AddComponent<MeshFilter>();
            MeshFilter[] meshFilters = SmallCellsParent.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.GetComponent<MeshRenderer>().enabled = false;

                i++;
            }
            MeshParent.transform.GetComponent<MeshFilter>().mesh = new Mesh();
            MeshParent.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            MeshParent.transform.GetComponent<Renderer>().material = meshFilters[0].gameObject.GetComponent<Renderer>().material;
            MeshParent.transform.gameObject.SetActive(true);



    }

    #region Hex Stuff
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
        go.transform.parent = SmallCellsParent.transform;
        go.transform.position = new Vector3(posQ + 400, 0, posR);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.transform.name = Q + ", " + R;
        TilePostition.Add(new Vector2(Q, R));

        go.AddComponent<MeshCollider>();
        CellObjects[CellArrayIndex].Add(go);
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

    public Cell GetCellByPos(int x, int y)
    {
        for (int q = 0; q < GridSize * 2 + 1; q++)
        {
            for (int r = 0; r < CellObjects[q].Count; r++)
            {
                if (Cells[q][r].TilePostition.x == x && Cells[q][r].TilePostition.y == y)
                {
                    return Cells[q][r];
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

    #endregion
}
