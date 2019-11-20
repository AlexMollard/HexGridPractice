using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBehavior : MonoBehaviour
{
    CellBehaviour.BiomeType Biome;
    Cell[,] cells;
    public bool[] HasTreeType;
    public bool[] HasOreType;
    public bool[] HasShrubType;
    public bool[] HasAnimalType;

    // Start is called before the first frame update
    void Start()
    {
        HasTreeType = new bool[Enum.GetNames(typeof(Cell.TreeType)).Length];
        HasOreType = new bool[Enum.GetNames(typeof(Cell.OreType)).Length];
        HasShrubType = new bool[Enum.GetNames(typeof(Cell.ShrubType)).Length];
        HasAnimalType = new bool[Enum.GetNames(typeof(Cell.AnimalType)).Length];

        for (int i = 0; i < Enum.GetNames(typeof(Cell.TreeType)).Length; i++)
        {
            HasTreeType[i] = false;
        }
        for (int i = 0; i < Enum.GetNames(typeof(Cell.OreType)).Length; i++)
        {
            HasOreType[i] = false;
        }
        for (int i = 0; i < Enum.GetNames(typeof(Cell.ShrubType)).Length; i++)
        {
            HasShrubType[i] = false;
        }
        for (int i = 0; i < Enum.GetNames(typeof(Cell.AnimalType)).Length; i++)
        {
            HasAnimalType[i] = false;
        }
        HasTreeType[1] = true;
        HasTreeType[5] = true;
        HasTreeType[3] = true;
        HasTreeType[6] = true;
        HasTreeType[7] = true;
        HasTreeType[8] = true;
        HasOreType[0] = true;
        HasOreType[3] = true;
        HasOreType[4] = true;
        HasShrubType[0] = true;
        HasShrubType[1] = true;
        HasShrubType[2] = true;
        HasShrubType[3] = true;
        HasShrubType[4] = true;
        HasAnimalType[0] = true;
        HasAnimalType[1] = true;
        HasAnimalType[2] = true;
        HasAnimalType[3] = true;
    }
}
