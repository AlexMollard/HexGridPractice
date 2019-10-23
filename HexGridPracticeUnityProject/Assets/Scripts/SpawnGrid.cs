using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
    int[,] axial_directions = new int[6, 2] {
        {+1, 0}, {+1, -1}, {0, -1},
        {-1, 0}, {-1, +1}, {0, +1}
    };

    public GameObject TilePrefab;
    public List<List<GameObject>> Cell;
    float timer = 0.0f;
    float[,] Noise;
    float scale = .57f;
    public int RepeatAmount = 2;
    public float RandomNumber;
    public int ChunkSize = 2;
    public float Freqancy = 0.01f;
    GameObject goDaddy;
    List <Vector2> tempPos;
    void Start()
    {
        RandomNumber = UnityEngine.Random.value;

        Cell = new List<List<GameObject>>();
        Cell.Add(new List<GameObject>());

        float SecondChunkCenter = ChunkSize * 2f;

        for(int i = 0; i < 6; ++i)
        {
            var offsetx = axial_directions[i, 0];
            var offsety = axial_directions[i, 1];

            tempPos = new List<Vector2>();

            tempPos.Add(FlatToWorld((int)SecondChunkCenter * offsetx, (int)SecondChunkCenter * offsety));

            goDaddy = Instantiate(TilePrefab, new Vector3(tempPos[0].x, 1.0f, tempPos[0].y), new Quaternion(), this.transform);

            goDaddy.GetComponent<MeshRenderer>().enabled = false;
            //goDaddy.transform.Rotate(new Vector3(0, 0, 0));

            Createegon(i, tempPos[0]);
        }

        


        //Cell.Add(new List<GameObject>());
        //tempPos.Add(FlatToWorld((int)SecondChunkCenter, 0));
        //goDaddy = Instantiate(TilePrefab, new Vector3(tempPos[1].x, 1.0f, tempPos[1].y), new Quaternion(), this.transform);
        //goDaddy.transform.Rotate(new Vector3(0, 3, 0));
        
        //Createegon(1,tempPos[1]);
        
        //Cell.Add(new List<GameObject>());
        //tempPos.Add(FlatToWorld((int)SecondChunkCenter, (int)-SecondChunkCenter));
        //goDaddy = Instantiate(TilePrefab, new Vector3(tempPos[2].x, 1.0f, tempPos[2].y), new Quaternion(), this.transform);
        //goDaddy.transform.Rotate(new Vector3(0, 0, 0));
        
        //Createegon(2,tempPos[2]);
        
        //Cell.Add(new List<GameObject>());
        //tempPos.Add(FlatToWorld(0, (int)SecondChunkCenter));
        //goDaddy = Instantiate(TilePrefab, new Vector3(tempPos[3].x, 1.0f, tempPos[3].y), new Quaternion(), this.transform);
        //goDaddy.transform.Rotate(new Vector3(0, 3, 0));
        
        //Createegon(3,tempPos[3]);
        
        //Cell.Add(new List<GameObject>());
        //tempPos.Add(FlatToWorld((int)-SecondChunkCenter, (int)SecondChunkCenter));
        //goDaddy = Instantiate(TilePrefab, new Vector3(tempPos[4].x, 1.0f, tempPos[4].y), new Quaternion(), this.transform);
        //goDaddy.transform.Rotate(new Vector3(0, 0, 0));
        
        //Createegon(4,tempPos[4]);
        
        //Cell.Add(new List<GameObject>());
        //tempPos.Add(FlatToWorld((int)-SecondChunkCenter, 0));
        //goDaddy = Instantiate(TilePrefab, new Vector3(tempPos[5].x, 1.0f, tempPos[5].y), new Quaternion(), this.transform);
        //goDaddy.transform.Rotate(new Vector3(0, 3, 0));
        
        
        //Createegon(5,tempPos[5]);
        
        //Cell.Add(new List<GameObject>());
        //tempPos.Add(FlatToWorld(0, (int)-SecondChunkCenter));
        //goDaddy = Instantiate(TilePrefab, new Vector3(tempPos[6].x, 1.0f, tempPos[6].y), new Quaternion(), this.transform);
        //goDaddy.transform.Rotate(new Vector3(0, 0, 0));
        
        //Createegon(6,tempPos[6]);
    }

    void Createegon(int index, Vector2 OffSet)
    {
        for (int q = -ChunkSize; q <= ChunkSize; q++)
        {
            int r1 = Mathf.Max(-ChunkSize, -q - ChunkSize);
            int r2 = Mathf.Min(ChunkSize, -q + ChunkSize);

            for (int r = r1; r <= r2; r++)
            {
                CreateCell(index,q, r, OffSet);
            }
        }
    }

    private void CreateCell(int index, int R, int Q,Vector2 offset)
    {
        int posR = R;
        int posQ = Q;

        GameObject go = Instantiate(TilePrefab);
        go.transform.parent = goDaddy.transform;
        Cell[index].Add(go);

        Vector2 tempPos = AxialToWorld(Q, R, offset);

        go.transform.position = new Vector3(tempPos.x, 0, tempPos.y);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.GetComponent<CellBehaviour>().TilePostition = new Vector2(posR, posQ);
    }

    Vector2 AxialToWorld(int q, int r, Vector2 offset)
    {
        var x = scale * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r) + offset.x;
        var y = scale * (3.0f / 2f * r) + offset.y;
        return new Vector2(x, y);
    }

    Vector2 FlatToWorld(int q, int r)
    {
        var x = scale * (3.0f/ 2f * q);
        var y = scale * (Mathf.Sqrt(3f) / 2f * q + Mathf.Sqrt(3f) * r);
        return new Vector2(x, y);
    }
    private void Update()
    {
        timer += Time.deltaTime * 5.0f;


        if (Input.GetMouseButton(0) && timer > 0.1f)
        {
            RandomNumber += 0.05f;
            for (int i = 0; i < Cell.Count; i++)
            {

                foreach (GameObject g in Cell[i])
                {
                    CellBehaviour behaviour = g.GetComponent<CellBehaviour>();
                    Vector2 pos = AxialToWorld((int)behaviour.TilePostition.x, (int)behaviour.TilePostition.y, tempPos[i]);

                    float Noise = Mathf.PerlinNoise((pos.x * Freqancy + RandomNumber) / 2, pos.y * Freqancy + RandomNumber);

                    g.transform.localScale = new Vector3(1, Noise, 1);

                    behaviour.SetTileProperties();
                }
            }

            timer = 0.0f;
        }
    }
}
