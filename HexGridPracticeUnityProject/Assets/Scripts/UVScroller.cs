using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroller : MonoBehaviour
{
    public Vector2 ImageSize;
    public Vector2 TextureSize;
    public List<Vector2> OriginalUVS = new List<Vector2>();
    public Mesh Hexagon;

    private void Start()
    {
        OriginalUVS = new List<Vector2>();
        Hexagon.GetUVs(0, OriginalUVS);
    }

    public void SetTexture(float cellType, float Humidity, bool isWater, GameObject cell)
    {
        Mesh mesh = cell.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        List<Vector2> uvs = new List<Vector2>();

        mesh.GetUVs(0, uvs);

        if(!isWater)
            Humidity /= 2;

        ImageSize = new Vector2(cell.GetComponent<Renderer>().sharedMaterial.GetTexture("_MainTex").width, cell.GetComponent<Renderer>().sharedMaterial.GetTexture("_MainTex").height);

        int NumberOfSubTexturesAcross = 20;

        float horizontalOffset = Humidity * 32;
        float verticalOffset = cellType - System.Enum.GetNames(typeof(BiomeManager.CellType)).Length + NumberOfSubTexturesAcross;

        for (var i = 0; i < uvs.Count; i++)
        {
            Vector2 start = OriginalUVS[i];

            start /= NumberOfSubTexturesAcross;
            start += new Vector2(horizontalOffset / NumberOfSubTexturesAcross, verticalOffset / NumberOfSubTexturesAcross);

            uvs[i] = start;
        }

        mesh.SetUVs(0, uvs);
    }
}
