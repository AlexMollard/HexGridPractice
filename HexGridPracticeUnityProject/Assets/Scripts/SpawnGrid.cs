using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
    public Mesh HexMesh;
    public Material[] CellMaterial;
    public int GridSize = 2;
    public List<List<GameObject>> Cell;
    bool SecondRow = false;
    // Start is called before the first frame update
    void Start()
    {
        Cell = new List<List<GameObject>>();

        for (int x = 0; x < GridSize * GridSize; x++)
        {
            Cell.Add(new List<GameObject>());



            for (int z = 0; z < GridSize; z++)
            {
                Cell[x].Add(new GameObject("Cell: " + x + ", " + z));

                if (SecondRow)
                    Cell[x][z].transform.position = new Vector3(((x) - (GridSize * GridSize / 2)) * 0.5f, 0, ((z + 0.5f) - (GridSize / 2)) * 1.75f);
                else
                    Cell[x][z].transform.position = new Vector3(((x) - (GridSize * GridSize / 2)) * 0.5f , 0, (z - (GridSize / 2)) * 1.75f);

                Cell[x][z].transform.localScale = new Vector3(1, Random.value * 2, 1);
                Cell[x][z].AddComponent<MeshRenderer>();
                Cell[x][z].AddComponent<MeshFilter>();
                Cell[x][z].GetComponent<MeshFilter>().mesh = HexMesh;

                if (Cell[x][z].transform.localScale.y > 1)
                    Cell[x][z].GetComponent<MeshRenderer>().material = CellMaterial[0];
                else if (Cell[x][z].transform.localScale.y > 0.5)
                    Cell[x][z].GetComponent<MeshRenderer>().material = CellMaterial[1];
                else
                    Cell[x][z].GetComponent<MeshRenderer>().material = CellMaterial[2];
            }
            if (!SecondRow)
                SecondRow = true;
            else
                SecondRow = false;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
