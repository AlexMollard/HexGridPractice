using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
    public GameObject TilePrefab;
    public List<List<GameObject>> Cell;
    bool SecondRow = false;
    float timer = 0.0f;
    float[,] Noise;
    public int RepeatAmount = 2;
    public float RandomNumber;
    public Vector2 GridSize;
    bool Pause = false;
    public float Freqancy = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        RandomNumber = Random.value;

        Cell = new List<List<GameObject>>();

        for (int x = 0; x < (int)GridSize.x * 2; x++)
        {
            Cell.Add(new List<GameObject>());

            //if (!SecondRow)
            //    SecondRow = true;
            //else
            //    SecondRow = false;

            /*
                var x = size * 3f/2f * hex.col
                var y = size * Mathf.Sqrt(3f) * (hex.row + 0.5f * (hex.col & 1)) 
             
             */

            for (int z = 0; z < (int)GridSize.y; z++)
            {
                Cell[x].Add(Instantiate(TilePrefab));

                //if (SecondRow)
                //    Cell[x][z].transform.position = new Vector3(((x) - ((int)GridSize.x)) * 0.5f, 0, ((z + 0.5f) - ((int)GridSize.y / 2)) * 1.75f);
                //else
                //    Cell[x][z].transform.position = new Vector3(((x) - ((int)GridSize.x)) * 0.5f , 0, (z - ((int)GridSize.y / 2)) * 1.75f);

                float scale = .57f;

                var lx = scale * Mathf.Sqrt(3f) * (z + 0.5f * (x & 1));
                var lz = scale * 3f / 2f * x;


                Cell[x][z].transform.position = new Vector3(lx, 0, lz);


                Cell[x][z].transform.localScale = new Vector3(1, 1, 1);

                Cell[x][z].GetComponent<CellBehaviour>().SetTileProperties(x, z);

            }
        }

        Noise = new float[Cell.Count, Cell[0].Count];

        GenerateNoise();
    }

    void GenerateNoise()
    {
            RandomNumber += 0.05f;
        for (int i = 0; i < RepeatAmount; i++)
        {
            for (int x = 0; x < Cell.Count; x++)
            {
                for (int z = 0; z < Cell[x].Count; z++)
                {
                    if (i == 1)
                        Noise[x, z] = Mathf.PerlinNoise(((float)x * Freqancy + RandomNumber) / 2, (float)z * Freqancy + RandomNumber);
                    else
                        Noise[x, z] += Mathf.PingPong( Mathf.PerlinNoise(((float)x * Freqancy + (RandomNumber + (i * 2))) / 2, (float)z * Freqancy + (RandomNumber + (i * 2))),1);
                }
            }
        }
    }


    private void Update()
    {
        timer += Time.deltaTime * 5.0f;


        if (Input.GetMouseButton(0) && timer > 0.2f)
        {
            GenerateNoise();
            for (int x = 0; x < Cell.Count; x++)
            {
                for (int z = 0; z < Cell[x].Count; z++)
                {
                    Cell[x][z].transform.localScale = new Vector3(1, Noise[x, z], 1);

                    Cell[x][z].GetComponent<CellBehaviour>().SetTileProperties(x, z);
                }
            }

            timer = 0.0f;
        }
    }
}
