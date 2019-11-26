using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHexMesh : MonoBehaviour
{
    UVScroller scroller;

    [Header("CellType")]
    public float CellType = 0.0f;

    [Header("Humidity")]
    public float Humidity = 0.0f;

    private void Start()
    {
        scroller = GetComponent<UVScroller>();
    }
    private void Update()
    {
        
        scroller.SetTexture(CellType, Humidity, false, gameObject);

        //Mesh mesh = GetComponent<MeshFilter>().mesh;
        //Vector3[] vertices = mesh.vertices;
        //Vector3[] normals = mesh.normals;
        //
        //List<Vector2> uvs = new List<Vector2>();
        //
        //mesh.GetUVs(0, uvs);
        //
        //for (var i = 0; i < uvs.Count; i++)
        //{
        //    uvs[i] += new Vector2(0, Time.deltaTime * 1f);
        //}
        //
        //mesh.SetUVs(0, uvs);
        //mesh.vertices = vertices;
    }
}
