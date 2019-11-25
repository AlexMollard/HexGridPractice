using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroller : MonoBehaviour
{
    public Vector2 CurrentOffset;
    public Vector2 ImageSize;
    public Vector2 TextureSize;
    public Vector2 UVPostition;


    public void SetTexture(float cellType, float Humidity, bool isWater, GameObject cell)
    {
        Mesh mesh = cell.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        List<Vector2> uvs = new List<Vector2>();

        mesh.GetUVs(0, uvs);

        ImageSize = new Vector2(cell.GetComponent<Renderer>().sharedMaterial.GetTexture("_MainTex").width, cell.GetComponent<Renderer>().sharedMaterial.GetTexture("_MainTex").height);


        //if (!isWater)
        //    Humidity = Mathf.Clamp(Humidity * 1.33f,0,1);

        //Humidity = ((Mathf.Round(((Humidity * (TextureSize.x - 1)) * PixelSize.x) / PixelSize.x) * PixelSize.x) - 1) * -1;
        //cellType = -1 - cellType;
        //cellType = Mathf.Round((cellType * PixelSize.y) / PixelSize.y) * PixelSize.y;

        //CurrentOffset = (UVPostition);
        //cell.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", CurrentOffset);


        TextureSize = new Vector2(1.0f / 32.0f, 1.0f / 32.0f);
        UVPostition.x = TextureSize.x * Humidity;
        UVPostition.y = TextureSize.y * cellType;

        for (var i = 0; i < uvs.Count; i++)
        {
            uvs[i] = UVPostition;
        }

        mesh.SetUVs(0, uvs);
    }
}
