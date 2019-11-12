using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CellBehaviour : MonoBehaviour
{
    // Enums
    public enum CellType { Taiga, Savanna, Tundra, RainForest, Desert, Forest, Plains, Snow, Ocean, Beach, Bare, Scorched, HotRainForest, WetDesert, TropSeasonForest, DeepOcean }
    public enum BiomeType { Taiga, Savanna, Tundra, RainForest, Desert, Forest, Plains, Snow, Ocean, Beach, Bare, Scorched, HotRainForest, WetDesert, TropSeasonForest, DeepOcean }


    [Header("Tile Properties")]
    public Vector2 TilePostition;
    float altitude;
    float humidity;
    bool Selected;
    float[] TileHeight;
    CellType TileType;
    public BiomeType TileBiome = BiomeType.Plains;
    public Material[] CellMaterial;

    [Header("Tile Decorations")]
    public GameObject[] Trees;
    public GameObject[] Stones;
    public Material[] TreeMats;
    public Material[] StoneMats;
    GameObject TreeHolderObject;
    GameObject StoneHolderObject;
    List<GameObject> TempObjects;
    int StoneAmount;
    int TreeAmount;
    bool HasTrees = false;
    bool HasStones = false;
    bool HasSnow = false;

    public void Awake()
    {
        TempObjects = new List<GameObject>();
        TileHeight = new float[] { 3.75f, 3.0f, 4.25f, 1.75f, 3.0f, 2.75f, 2.5f, 4.45f, 1.0f, 1.5f, 4.0f, 3.85f, 3.0f, 2.0f, 2.0f, 1.0f };
    }

    public void SetTileProperties(float Altitude, float Humidity)
    {
        SetBiome(Altitude, Humidity);
        AssignType();
    }

    void SetBiome(float Altitude, float Humidity)
    {
        altitude = Altitude * 2;
        TileBiome = GetBiomeType(Altitude, Humidity);
    }

    BiomeType GetBiomeType(float Altitude, float Humidity)
    {
        if (Altitude < 0.025) return BiomeType.DeepOcean;
        if (Altitude < 0.1) return BiomeType.Ocean;
        if (Altitude < 0.15) return BiomeType.Beach;

        if (Altitude > 0.8)
        {
            if (Humidity < 0.1) return BiomeType.Scorched;
            if (Humidity < 0.2) return BiomeType.Bare;
            if (Humidity < 0.5) return BiomeType.Tundra;
            return BiomeType.Snow;
        }

        if (Altitude > 0.6)
        {
            if (Humidity < 0.33) return BiomeType.Desert;
            if (Humidity < 0.66) return BiomeType.Savanna;
            return BiomeType.Taiga;
        }

        if (Altitude > 0.3)
        {
            if (Humidity < 0.16) return BiomeType.Desert;
            if (Humidity < 0.50) return BiomeType.Plains;
            if (Humidity < 0.83) return BiomeType.Forest;
            return BiomeType.HotRainForest;
        }

        if (Humidity < 0.16) return BiomeType.WetDesert;
        if (Humidity < 0.33) return BiomeType.Plains;
        if (Humidity < 0.66) return BiomeType.TropSeasonForest;
        return BiomeType.RainForest;
    }


    void AssignType()
    {
        HasTrees = false;
        HasStones= false;
        HasSnow = false;

        if (TileBiome == BiomeType.Taiga)
        {
            TileType = CellType.Taiga;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Taiga];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasSnow = true;
            HasTrees = true;
            TreeAmount = Random.Range(5, 15);
        }

        else if (TileBiome == BiomeType.Savanna)
        {
            TileType = CellType.Savanna;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Savanna];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasStones = true;
            StoneAmount = Random.Range(3, 10);
        }

        else if (TileBiome == BiomeType.Tundra)
        {
            TileType = CellType.Tundra;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Tundra];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasSnow = true;
            HasTrees = true;
            TreeAmount = Random.Range(0, 5);
            HasStones = true;
            StoneAmount = Random.Range(0, 5);
        }

        else if(TileBiome == BiomeType.RainForest)
        {
            TileType = CellType.RainForest;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.RainForest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasTrees = true;
            TreeAmount = Random.Range(16, 30);
        }

        else if(TileBiome == BiomeType.Desert)
        {
            TileType = CellType.Desert;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Desert];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasStones = true;
            StoneAmount = Random.Range(0, 5);
        }

        else if(TileBiome == BiomeType.Forest)
        {
            TileType = CellType.Forest;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Forest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasTrees = true;
            TreeAmount = Random.Range(5, 15);
        }

        else if(TileBiome == BiomeType.Plains)
        {
            TileType = CellType.Plains;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Plains];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasTrees = true;
            TreeAmount = Random.Range(0, 4);
        }

        else if(TileBiome == BiomeType.Snow)
        {
            TileType = CellType.Snow;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Snow];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasSnow = true;
            HasTrees = true;
            TreeAmount = Random.Range(0, 2);

        }

        else if(TileBiome == BiomeType.Ocean)
        {
            TileType = CellType.Ocean;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Ocean];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + (altitude / 4.0f), 1f);

        }

        else if(TileBiome == BiomeType.Beach)
        {
            TileType = CellType.Beach;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Beach];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasStones = true;
            StoneAmount = Random.Range(0, 2);
        }

        else if(TileBiome == BiomeType.Bare)
        {
            TileType = CellType.Bare;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Bare];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.Scorched)
        {
            TileType = CellType.Scorched;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.Scorched];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasStones = true;
            StoneAmount = Random.Range(4, 10);
        }

        else if(TileBiome == BiomeType.HotRainForest)
        {
            TileType = CellType.HotRainForest;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.HotRainForest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasTrees = true;
            TreeAmount = Random.Range(6, 20);

        }

        else if(TileBiome == BiomeType.WetDesert)
        {
            TileType = CellType.WetDesert;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.WetDesert];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasStones = true;
            StoneAmount = Random.Range(0, 3);
        }

        else if (TileBiome == BiomeType.TropSeasonForest)
        {
            TileType = CellType.TropSeasonForest;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.TropSeasonForest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasTrees = true;
            TreeAmount = Random.Range(10, 30);
        }

        else if (TileBiome == BiomeType.DeepOcean)
        {
            TileType = CellType.DeepOcean;
            GetComponent<Renderer>().material = CellMaterial[(int)CellType.DeepOcean];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + (altitude / 4.0f), 1f);

        }

        transform.name = System.Convert.ToString(TileBiome);

        if (HasTrees && TreeAmount > 0)
        {
            if (TreeHolderObject)
            {
                Destroy(TreeHolderObject);
            }

            MeshFilter[] meshFilters = new MeshFilter[TreeAmount];
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            TreeHolderObject = new GameObject();
            this.gameObject.AddComponent<MeshCollider>();
            List<Vector3> TreeTempPoses = new List<Vector3>();

            for (int i = 0; i < TreeAmount; i++)
            {
                GameObject tree = Instantiate(Trees[0]);

                tree.AddComponent<MeshCollider>();
                RaycastHit hit;
                bool Placed = false;
                while (!Placed)
                {
                    float a = Random.value * 2 * Mathf.PI;
                    float r = Mathf.Sqrt(Random.value);

                    float x = r * Mathf.Cos(a);
                    float z = r * Mathf.Sin(a);

                    tree.transform.position = new Vector3(transform.position.x + x, transform.localScale.y / 4, transform.position.z + z);
                    if (Physics.Raycast(tree.transform.position, -tree.transform.up, out hit, 30f))
                    {
                        if (hit.transform.gameObject == gameObject)
                        {
                            if (TreeTempPoses.Count > 0)
                            {
                                bool isSafe = false;
                                for (int p = 0; p < TreeTempPoses.Count; p++)
                                {
                                    if (Vector3.Distance(TreeTempPoses[p], tree.transform.position) > 0.05f)
                                    {
                                        if (p == TreeTempPoses.Count - 1)
                                        {
                                            isSafe = true;
                                        }
                                        continue;
                                    }
                                    else
                                        break;
                                }
                                if (isSafe)
                                    Placed = true;
                            }
                            else
                                Placed = true;
                        }
                    }
                }
                TreeTempPoses.Add(tree.transform.position);
                tree.transform.position = new Vector3(tree.transform.position.x, transform.localScale.y / 5, tree.transform.position.z);

                meshFilters[i] = tree.GetComponentInChildren<MeshFilter>();
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].GetComponent<MeshRenderer>().enabled = false;

                tree.transform.localScale = new Vector3(1, 1, 1);
                TempObjects.Add(tree);
            }
            TreeHolderObject.AddComponent<MeshFilter>();
            TreeHolderObject.AddComponent<MeshRenderer>();
            TreeHolderObject.transform.GetComponent<MeshFilter>().mesh = new Mesh();
            TreeHolderObject.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

            if (HasSnow)
                TreeHolderObject.GetComponent<MeshRenderer>().material = TreeMats[1];
            else
                TreeHolderObject.GetComponent<MeshRenderer>().material = TreeMats[0];


            TreeHolderObject.transform.SetParent(transform);
            TreeHolderObject.transform.name = "Trees";
            TreeHolderObject.transform.gameObject.SetActive(true);
            for (int i = 0; i < TempObjects.Count; i++)
            {
                Destroy(TempObjects[i]);
            }
            TempObjects.Clear();
        }
        if (HasStones && StoneAmount > 0)
        {
            if (StoneHolderObject)
            {
                Destroy(StoneHolderObject);
            }
            MeshFilter[] meshFilters = new MeshFilter[StoneAmount];
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            StoneHolderObject = new GameObject();
            this.gameObject.AddComponent<MeshCollider>();
            List<Vector3> StoneTempPoses = new List<Vector3>();
            for (int i = 0; i < StoneAmount; i++)
            {
                GameObject stone = Instantiate(Stones[0], new Vector3(transform.position.x + (Random.Range(-1, 1) * Random.value) / 2.5f, transform.localScale.y / 5, transform.position.z + (Random.Range(-1, 1) * Random.value) / 2.5f), new Quaternion(0, Random.Range(0, 180), 0, 0));


                stone.AddComponent<MeshCollider>();
                RaycastHit hit;
                bool Placed = false;
                while (!Placed)
                {
                    float a = Random.value * 2 * Mathf.PI;
                    float r = Mathf.Sqrt(Random.value);

                    float x = r * Mathf.Cos(a);
                    float z = r * Mathf.Sin(a);

                    stone.transform.position = new Vector3(transform.position.x + x, transform.localScale.y / 4, transform.position.z + z);
                    if (Physics.Raycast(stone.transform.position, -stone.transform.up, out hit, 30f))
                    {
                        if (hit.transform.gameObject == gameObject)
                        {
                            if (StoneTempPoses.Count > 0)
                            {
                                bool isSafe = false;
                                for (int p = 0; p < StoneTempPoses.Count; p++)
                                {
                                    if (Vector3.Distance(StoneTempPoses[p], stone.transform.position) > 0.05f)
                                    {
                                        if (p == StoneTempPoses.Count - 1)
                                        {
                                            isSafe = true;
                                        }
                                        continue;
                                    }
                                    else
                                        break;
                                }
                                if (isSafe)
                                    Placed = true;
                            }
                            else
                                Placed = true;
                        }
                    }
                }
                StoneTempPoses.Add(stone.transform.position);
                stone.transform.position = new Vector3(stone.transform.position.x, transform.localScale.y / 5, stone.transform.position.z);





                meshFilters[i] = stone.GetComponentInChildren<MeshFilter>();
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].GetComponent<MeshRenderer>().enabled = false;

                stone.transform.localScale = new Vector3(1, 1, 1);
                TempObjects.Add(stone);
            }

            StoneHolderObject.AddComponent<MeshFilter>();
            StoneHolderObject.AddComponent<MeshRenderer>();
            StoneHolderObject.transform.GetComponent<MeshFilter>().mesh = new Mesh();
            StoneHolderObject.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            StoneHolderObject.GetComponent<MeshRenderer>().material = StoneMats[0];
            StoneHolderObject.transform.SetParent(transform);
            StoneHolderObject.transform.name = "Stones";
            StoneHolderObject.transform.gameObject.SetActive(true);


            for (int i = 0; i < TempObjects.Count; i++)
            {
                Destroy(TempObjects[i]);
            }
            TempObjects.Clear();
        }
        if (TreeHolderObject && !HasTrees)
        {
            Destroy(TreeHolderObject);
        }
        if (StoneHolderObject && !HasStones)
        {
            Destroy(StoneHolderObject);
        }
    }

    private void OnMouseOver()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase != TouchPhase.Moved)
        {
            if (!Selected)
            {
                transform.GetComponentInParent<SpawnGrid>().SetText("Biome: " + TileType);
                GetComponent<Renderer>().material.color -= new Color(0.5f * GetComponent<Renderer>().material.color.r, 0.5f * GetComponent<Renderer>().material.color.g, 0.5f * GetComponent<Renderer>().material.color.b); ;
            }
        Selected = true;
        }
    }

    private void OnMouseExit()
    {
        Selected = false;
    }

    
}