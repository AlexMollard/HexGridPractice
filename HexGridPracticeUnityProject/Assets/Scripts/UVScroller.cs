using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType { Biome, Cell, Ore, Tree, Shrub, Animal }


public class UVScroller : MonoBehaviour
{
    public int NumberOfSubTexturesAcross = 32;
    public Vector2 ImageSize;
    public Vector2 TextureSize;
    public List<Vector2> OriginalUVS = new List<Vector2>();
    public Mesh Hexagon;
    public Sprite UvAtlas;
    Mesh mesh = null;
    float horizontalOffset;
    float verticalOffset;
    List<Vector2> uvs = new List<Vector2>();
    Vector3[] vertices;
    Vector3[] normals;

    private void Start()
    {
        OriginalUVS = new List<Vector2>();
        ImageSize = new Vector2(UvAtlas.texture.width, UvAtlas.texture.height);
    }

    public void SetTexture(ObjectType type, GameObject cell = null, float cellType = 0.0f, float Humidity = 0.0f)
    {
        Hexagon.GetUVs(0, OriginalUVS); 
        if (type == ObjectType.Cell)
        {
            mesh = cell.GetComponent<MeshFilter>().mesh;
            horizontalOffset = Mathf.Round( Humidity * 16);
            verticalOffset = cellType - System.Enum.GetNames(typeof(BiomeManager.CellType)).Length + NumberOfSubTexturesAcross;
        }
        else if (type == ObjectType.Biome)
        {
            mesh = cell.GetComponent<MeshFilter>().mesh;
            horizontalOffset = Mathf.Round(Humidity * 16) + 16;
            verticalOffset = cellType - System.Enum.GetNames(typeof(CellBehaviour.BiomeType)).Length + NumberOfSubTexturesAcross;
        }

        vertices = mesh.vertices;
        normals = mesh.normals;

        mesh.GetUVs(0, uvs);

        for (var i = 0; i < uvs.Count; i++)
        {
            Vector2 NewUVPos = OriginalUVS[i];

            NewUVPos /= NumberOfSubTexturesAcross;
            NewUVPos += new Vector2(horizontalOffset / NumberOfSubTexturesAcross, verticalOffset / NumberOfSubTexturesAcross);

            uvs[i] = NewUVPos;
        }

        mesh.SetUVs(0, uvs);
    }
}
