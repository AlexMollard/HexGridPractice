using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    public Material[] CellMaterial;
    public enum CellType { Water, Sand, Grass, Stone, Snow }
    public CellType TileType;
    public Vector2 TilePostition;
    public GameObject[] Neighbours;

    public void SetTileProperties()
    {

        if (gameObject.transform.localScale.y > 0.8f)
        {
            gameObject.GetComponent<MeshRenderer>().material = CellMaterial[0];
            transform.localScale = new Vector3(1, gameObject.transform.localScale.y * 7, 1);
            TileType = CellType.Snow;
        }
        else if (gameObject.transform.localScale.y > 0.6f)
        {
            gameObject.GetComponent<MeshRenderer>().material = CellMaterial[1];
            transform.localScale = new Vector3(1, gameObject.transform.localScale.y * 6, 1);
            TileType = CellType.Stone;

        }
        else if (gameObject.transform.localScale.y > 0.375f)
        {
            gameObject.GetComponent<MeshRenderer>().material = CellMaterial[2];
            transform.localScale = new Vector3(1, gameObject.transform.localScale.y * 4, 1);
            TileType = CellType.Grass;

        }
        else if (gameObject.transform.localScale.y > 0.3f)
        {
            gameObject.GetComponent<MeshRenderer>().material = CellMaterial[3];
            transform.localScale = new Vector3(1, gameObject.transform.localScale.y * 3, 1);
            TileType = CellType.Sand;

        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = CellMaterial[4];
            transform.localScale = new Vector3(1, 0.8f, 1);
            TileType = CellType.Water;

        }
    }
}
