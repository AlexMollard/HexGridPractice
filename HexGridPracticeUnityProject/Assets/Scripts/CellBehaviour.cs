using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CellBehaviour : MonoBehaviour
{
    public struct SmallTile
    {
       public ResourceType tileType;
    }

    // Enums
    public enum BiomeType { Taiga, Savanna, Tundra, RainForest, Desert, Forest, Plains, Snow, Ocean, Beach, Bare, Scorched, HotRainForest, WetDesert, TropSeasonForest, DeepOcean }

    public enum ResourceType {Grass,Stone,Sand,Water,Trees,Ores,Animals, Other};

    Transform ChildTransform;
    Renderer TreeRend;
    Renderer StoneRend;
    Vector3 ChildOffset;

    [Header("Tile Properties")]
    public Vector2 TilePostition;
    float altitude;
    float humidity;
    bool Selected;
    float[] TileHeight;
    BiomeType TileType;
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
    public float[] TileProperties;
    
    // Terrain
    public float[,] InnerTileNoise;
    float TerrainRandNum;
    float TerrainFrequancy = 0.03f;
    float PowValue = 2.03f;
    SmallTile[,] InnerTile;
    TerrainBehavior terrain;
    public void Awake()
    {
        ChildOffset = new Vector3();
        TerrainRandNum = Random.Range(1, 1000);
        ChildTransform = GetComponent<Transform>();
        TempObjects = new List<GameObject>();
        TileHeight = new float[] { 3.75f, 3.0f, 4.25f, 1.75f, 3.0f, 2.75f, 2.5f, 4.45f, 1.0f, 1.5f, 4.0f, 3.85f, 3.0f, 2.0f, 2.0f, 1.0f };
        TileProperties = new float[8];
        PowValue = 2.0f;
        for (int i = 0; i < TileProperties.Length; i++)
        {
            TileProperties[i] = 0;
        }
        InnerTileNoise = new float[10,10];
        InnerTile = new SmallTile[10,10];
    }

    public void SetTileProperties(float Altitude, float Humidity)
    {
        SetBiome(Altitude, Humidity);
        AssignType();
        GenerateTerrain();

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
            TileType = BiomeType.Taiga;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.Taiga];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasSnow = true;
            HasTrees = true;
            TreeAmount = Random.Range(3, 10);
        }

        else if (TileBiome == BiomeType.Savanna)
        {
            TileType = BiomeType.Savanna;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.Savanna];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasStones = true;
            StoneAmount = Random.Range(3, 10);
        }

        else if (TileBiome == BiomeType.Tundra)
        {
            TileType = BiomeType.Tundra;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.Tundra];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasSnow = true;
            HasTrees = true;
            TreeAmount = Random.Range(0, 3);
            HasStones = true;
            StoneAmount = Random.Range(0, 5);
        }

        else if(TileBiome == BiomeType.RainForest)
        {
            TileType = BiomeType.RainForest;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.RainForest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasTrees = true;
            TreeAmount = Random.Range(5, 10);
        }

        else if(TileBiome == BiomeType.Desert)
        {
            TileType = BiomeType.Desert;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.Desert];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasStones = true;
            StoneAmount = Random.Range(0, 5);
        }

        else if(TileBiome == BiomeType.Forest)
        {
            TileType = BiomeType.Forest;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.Forest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasTrees = true;
            TreeAmount = Random.Range(3, 7);
        }

        else if(TileBiome == BiomeType.Plains)
        {
            TileType = BiomeType.Plains;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.Plains];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasTrees = true;
            TreeAmount = Random.Range(0, 4);
        }

        else if(TileBiome == BiomeType.Snow)
        {
            TileType = BiomeType.Snow;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.Snow];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasSnow = true;
            HasTrees = true;
            TreeAmount = Random.Range(0, 2);

        }

        else if(TileBiome == BiomeType.Ocean)
        {
            TileType = BiomeType.Ocean;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.Ocean];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + (altitude / 4.0f), 1f);

        }

        else if(TileBiome == BiomeType.Beach)
        {
            TileType = BiomeType.Beach;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.Beach];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasStones = true;
            StoneAmount = Random.Range(0, 2);
        }

        else if(TileBiome == BiomeType.Bare)
        {
            TileType = BiomeType.Bare;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.Bare];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);

        }

        else if(TileBiome == BiomeType.Scorched)
        {
            TileType = BiomeType.Scorched;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.Scorched];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasStones = true;
            StoneAmount = Random.Range(4, 10);
        }

        else if(TileBiome == BiomeType.HotRainForest)
        {
            TileType = BiomeType.HotRainForest;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.HotRainForest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasTrees = true;
            TreeAmount = Random.Range(3, 8);

        }

        else if(TileBiome == BiomeType.WetDesert)
        {
            TileType = BiomeType.WetDesert;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.WetDesert];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasStones = true;
            StoneAmount = Random.Range(0, 3);
        }

        else if (TileBiome == BiomeType.TropSeasonForest)
        {
            TileType = BiomeType.TropSeasonForest;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.TropSeasonForest];
            transform.localScale = new Vector3(1f, TileHeight[(int)TileType] + altitude, 1f);
            HasTrees = true;
            TreeAmount = Random.Range(5, 10);
        }

        else if (TileBiome == BiomeType.DeepOcean)
        {
            TileType = BiomeType.DeepOcean;
            GetComponent<Renderer>().material = CellMaterial[(int)BiomeType.DeepOcean];
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
                                    if (Vector3.Distance(TreeTempPoses[p], tree.transform.position) > 0.2f)
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
            TreeHolderObject.layer = 2;
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
            StoneHolderObject.layer = 2;
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

    public void CheckTreeVisable(bool enable)
    {
        if (ChildTransform.childCount > 0)
        {
            TreeRend = ChildTransform.GetChild(0).GetComponent<Renderer>();
            if (ChildTransform.childCount > 1)
                StoneRend = ChildTransform.GetChild(1).GetComponent<Renderer>();

        }

        if (!enable && ChildTransform.childCount > 0)
        {
            TreeRend.enabled = false;
            if (ChildTransform.childCount > 1)
                StoneRend.enabled = false;
        }
        else if (ChildTransform.childCount > 0)
        {
            float offset = ChildTransform.localScale.y / 5;

            Vector3 cameraview = Camera.main.WorldToViewportPoint(ChildTransform.position + new Vector3(0, offset, 0));

            if (cameraview.x > -0.3f && cameraview.y > -0.3f && cameraview.z > 0 && cameraview.x < 1.3f && cameraview.y < 1.3f)
            {

                TreeRend.enabled = true;
                if (ChildTransform.childCount > 1)
                    StoneRend.enabled = true;
            }
            else
            {
                TreeRend.enabled = false;
                if (ChildTransform.childCount > 1)
                    StoneRend.enabled = false;
            }
            
        }

    }

    public void GenerateTerrain()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                float Noise = Mathf.PerlinNoise((x * TerrainFrequancy + TerrainRandNum), y * TerrainFrequancy + TerrainRandNum);
                Noise += 0.5f * Mathf.PerlinNoise((2 * x * TerrainFrequancy + TerrainRandNum), 2 * y * TerrainFrequancy + TerrainRandNum);
                Noise += 0.25f * Mathf.PerlinNoise((4 * x * TerrainFrequancy + TerrainRandNum), 4 * y * TerrainFrequancy + TerrainRandNum);

                Noise = Mathf.Pow(Noise, PowValue);

                InnerTileNoise[x, y] = Noise;
                if (Noise < 0.1f)
                {
                    InnerTile[x, y].tileType = ResourceType.Water;
                }
                else if (Noise < 0.18f)
                {
                    InnerTile[x, y].tileType = ResourceType.Sand;
                }
                else if (Noise < 0.8f)
                {
                    if (Random.value > 0.2f)
                    {
                        InnerTile[x, y].tileType = ResourceType.Grass;
                    }
                    else if (Random.value > 0.4f)
                    {
                        InnerTile[x, y].tileType = ResourceType.Trees;
                    }
                    else
                    {
                        InnerTile[x, y].tileType = ResourceType.Animals;
                    }
                }
                else
                {
                    if (Random.value > 0.1f)
                    {
                        InnerTile[x, y].tileType = ResourceType.Stone;
                    }
                    else
                    {
                        InnerTile[x, y].tileType = ResourceType.Ores;
                    }
                }
                TileProperties[(int)InnerTile[x, y].tileType] += 1;
            }
        }

    }


}