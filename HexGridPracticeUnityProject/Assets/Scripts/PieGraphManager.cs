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
    public TextMeshProUGUI[] Trees;
    public TextMeshProUGUI[] Ores;
    public TextMeshProUGUI[] Shrubs;
    public TextMeshProUGUI[] Animals;
    
    public TextMeshProUGUI Alt;
    public TextMeshProUGUI Hum;


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

        // Contains
        List<string> tempList = new List<string>();

        for (int i = 0; i < Trees.Length; i++)
        {
            Trees[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Ores.Length; i++)
        {
            Ores[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Shrubs.Length; i++)
        {
            Shrubs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Animals.Length; i++)
        {
            Animals[i].gameObject.SetActive(false);
        }




        //Trees
        for (int i = 0; i < Enum.GetNames(typeof(Cell.TreeType)).Length; i++)
        {
            if (cell.terrain.HasTreeType[i])
            {
                tempList.Add(System.Convert.ToString((Cell.TreeType)i));
            }
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            Trees[i].gameObject.SetActive(true);
            Trees[i].text = "- " + tempList[i];
        }
        tempList.Clear();



        //Ores
        for (int i = 0; i < Enum.GetNames(typeof(Cell.OreType)).Length; i++)
        {
            if (cell.terrain.HasOreType[i])
            {
                tempList.Add(System.Convert.ToString((Cell.OreType)i));
            }
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            Ores[i].gameObject.SetActive(true);
            Ores[i].text = "- " + tempList[i];
        }
        tempList.Clear();

        //Shrubs
        for (int i = 0; i < Enum.GetNames(typeof(Cell.ShrubType)).Length; i++)
        {
            if (cell.terrain.HasShrubType[i])
            {
                tempList.Add(System.Convert.ToString((Cell.ShrubType)i));
            }
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            Shrubs[i].gameObject.SetActive(true);
            Shrubs[i].text = "- " + tempList[i];
        }
        tempList.Clear();

        //Animals
        for (int i = 0; i < Enum.GetNames(typeof(Cell.AnimalType)).Length; i++)
        {
            if (cell.terrain.HasAnimalType[i])
            {
                tempList.Add(System.Convert.ToString((Cell.AnimalType)i));
            }
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            Animals[i].gameObject.SetActive(true);
            Animals[i].text = "- " + tempList[i];
        }
        tempList.Clear();


        Alt.text = System.Convert.ToString(cell.altitude);
        Hum.text = System.Convert.ToString(cell.humidity);

    }

}

