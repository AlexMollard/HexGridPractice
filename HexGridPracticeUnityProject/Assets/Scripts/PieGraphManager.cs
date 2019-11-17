using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieGraphManager : MonoBehaviour
{
    public struct element
    {
        public string Name;
        public int Value;
        public float Rotation;
        public float Fill;
        public GameObject Wedge;
        public GameObject Legend;
        public Color Color;
    }

    public int Total;
    public element[] Elements;
    public GameObject[] Wedges;
    public GameObject[] Legends;

    // Start is called before the first frame update
    void Start()
    {
        Elements = new element[Wedges.Length];
    }

    public void UpdateChart(CellBehaviour cell)
    {
        for (int i = 0; i < Wedges.Length; i++)
        {

        }
    }
}

