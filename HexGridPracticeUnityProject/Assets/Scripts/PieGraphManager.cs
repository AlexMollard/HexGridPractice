using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PieGraphManager : MonoBehaviour
{
    public struct element
    {
        public float Value;
        public float Rotation;
        public float Fill;
        public GameObject Wedge;
        public GameObject Legend;
        public int Color;
    }
    public float Total;
    public CellBehaviour cell;
    public element[] Elements;
    public GameObject[] Wedges;
    public GameObject[] Legends;
    public GameObject Biome;
    public float timer = 0.0f;
    public element[] RefElements;
    public float currentRotation;
    public TextMeshProUGUI BiomeText;

    // Start is called before the first frame update
    void Start()
    {
        Elements = new element[Wedges.Length];
        RefElements = new element[Elements.Length];

        for (int i = 0; i < Elements.Length; i++)
        {
            Elements[i].Wedge = Wedges[i];
            Elements[i].Legend = Legends[i];
        }

    }

    public void UpdateChart()
    {
        Total = 0;
        Biome.GetComponent<Image>().color = cell.CellMaterial[(int)cell.TileBiome].color;
        BiomeText.text = System.Convert.ToString(cell.TileBiome);
        for (int i = 0; i < Elements.Length; i++)
        {
            Elements[i].Value = cell.TileProperties[i];
            Total += Elements[i].Value;
            Elements[i].Color = i;
            RefElements[i] = Elements[i];
        }

        element temp;

        // traverse 0 to array length 
        for (int i = 0; i < RefElements.Length - 1; i++)

            // traverse i+1 to array length 
            for (int j = i + 1; j < RefElements.Length; j++)

                // compare array element with  
                // all next element 
                if (RefElements[i].Value > RefElements[j].Value)
                {

                    temp = RefElements[i];
                    RefElements[i] = RefElements[j];
                    RefElements[j] = temp;
                }

        currentRotation = 0f;
        for (int i = 0; i < Elements.Length; i++)
        {
            RefElements[i].Wedge.GetComponent<Image>().fillAmount = RefElements[i].Value / Total;
            Transform WedgeTransform = RefElements[i].Wedge.transform;
            currentRotation += RefElements[i].Wedge.GetComponent<Image>().fillAmount * 360;
            WedgeTransform.rotation = Quaternion.Euler(new Vector3(WedgeTransform.rotation.x, WedgeTransform.rotation.y, currentRotation));

        }
    }

}

