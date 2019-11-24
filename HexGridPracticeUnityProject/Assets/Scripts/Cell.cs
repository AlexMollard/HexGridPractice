using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2 TilePostition;
    public BiomeManager.CellType CellType;
    public BiomeManager.TowerType TowerType;
    public int TowerIndex;
    public bool hasTower = false;
    public string Tower = "";
    public GameObject TowerObject;
    public float Humidity;
    public float Altitude;
    // Start is called before the first frame update
    void Start()
    {
    }
}
