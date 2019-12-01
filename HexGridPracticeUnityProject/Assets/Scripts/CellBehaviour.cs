using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CellBehaviour : MonoBehaviour
{
    // Enums
    public enum BiomeType {Taiga, Tundra, RainForest, Desert, Forest, Plains, Moutains, Ocean, Beach, TropSeasonForest}

    Transform ChildTransform;
    Renderer TreeRend;
    Renderer StoneRend;
    public TerrainLoader terrainLoader;
    //Vector3 ChildOffset;

    [Header("Tile Properties")]
    public Vector2 TilePostition;
    public float altitude;
    public float humidity;
    bool Selected;
    float[] TileHeight;
    public BiomeType TileType;
    public BiomeType TileBiome = BiomeType.Plains;

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
    float PowValue = 2.03f;
    public TerrainBehavior terrain;
    GameObject TowersParent;
    float TerrainRandomNumber = 0.0f;
    public void Awake()
    {
        TerrainRandomNumber = UnityEngine.Random.Range(0, 100000);

        TerrainRandNum = Random.Range(1, 1000);
        ChildTransform = GetComponent<Transform>();
        TempObjects = new List<GameObject>();
        TileHeight = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };
        PowValue = 2.0f;

        TileProperties = new float[System.Enum.GetNames(typeof(BiomeManager.CellType)).Length + System.Enum.GetNames(typeof(BiomeManager.TowerType)).Length];   
        for (int i = 0; i < TileProperties.Length; i++)
        {
            TileProperties[i] = 0;
        }

        InnerTileNoise = new float[10,10];

        terrain = gameObject.GetComponent<TerrainBehavior>();


    }

    public void SetTileProperties(float Altitude, float Humidity)
    {
        SetBiome(Altitude, Humidity);
        AssignType();
    }

    void SetBiome(float Altitude, float Humidity)
    {
        altitude = Altitude;

        if (TileBiome != BiomeType.Ocean)
        {

            TileBiome = GetBiomeType(Altitude, Humidity);
        }
    }

    BiomeType GetBiomeType(float Altitude, float Humidity)
    {
        Humidity = Mathf.Clamp(Mathf.Pow(Humidity, 1.15f),0,1);

        humidity = Humidity;
        altitude = Altitude;
 
        if (Altitude < 0.35) return BiomeType.Beach;

        if (Altitude > 0.8)
        {
            if (Humidity < 0.5) return BiomeType.Tundra;
            return BiomeType.Moutains;
        }

        if (Altitude > 0.6)
        {
            if (Humidity < 0.33) return BiomeType.Desert;
            return BiomeType.Taiga;
        }

        if (Altitude > 0.3)
        {
            if (Humidity < 0.16) return BiomeType.Desert;
            if (Humidity < 0.50) return BiomeType.Plains;
            return BiomeType.Forest;
        }

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
            HasSnow = true;
            HasTrees = true;
            TreeAmount = Random.Range(3, 10);
        }

        else if (TileBiome == BiomeType.Tundra)
        {
            TileType = BiomeType.Tundra;
            HasSnow = true;
            HasTrees = true;
            TreeAmount = Random.Range(0, 3);
            HasStones = true;
            StoneAmount = Random.Range(0, 5);
        }

        else if(TileBiome == BiomeType.RainForest)
        {
            TileType = BiomeType.RainForest;
            HasTrees = true;
            TreeAmount = Random.Range(5, 10);
        }

        else if(TileBiome == BiomeType.Desert)
        {
            TileType = BiomeType.Desert;
            HasStones = true;
            StoneAmount = Random.Range(0, 5);
        }

        else if(TileBiome == BiomeType.Forest)
        {
            TileType = BiomeType.Forest;
            HasTrees = true;
            TreeAmount = Random.Range(3, 7);
        }

        else if(TileBiome == BiomeType.Plains)
        {
            TileType = BiomeType.Plains;
            HasTrees = true;
            TreeAmount = Random.Range(0, 4);
        }

        else if(TileBiome == BiomeType.Moutains)
        {
            TileType = BiomeType.Moutains;
            HasSnow = true;
            HasTrees = false;
        }

        else if(TileBiome == BiomeType.Ocean)
        {
            TileType = BiomeType.Ocean;
        }

        else if(TileBiome == BiomeType.Beach)
        {
            TileType = BiomeType.Beach;
            HasStones = true;
            StoneAmount = Random.Range(0, 2);
        }

        else if (TileBiome == BiomeType.TropSeasonForest)
        {
            TileType = BiomeType.TropSeasonForest;
            HasTrees = true;
            TreeAmount = Random.Range(5, 10);
        }

        //if (TileBiome == BiomeType.Ocean)
        //    transform.localScale = new Vector3(1f, 2.5f, 1f);
        //else
        //    transform.localScale = new Vector3(1f, Mathf.Lerp(1f, 75f, altitude / 7.5f), 1f);

       transform.localScale = new Vector3(1f, altitude, 1f);



        transform.name = System.Convert.ToString(TileBiome);

      // if (HasTrees && TreeAmount > 0)
      // {
      //     if (TreeHolderObject)
      //     {
      //         Destroy(TreeHolderObject);
      //     }
      //     MeshFilter[] meshFilters = new MeshFilter[TreeAmount];
      //     CombineInstance[] combine = new CombineInstance[meshFilters.Length];
      //     TreeHolderObject = new GameObject();
      //     List<Vector3> TreeTempPoses = new List<Vector3>();
      //
      //     for (int i = 0; i < TreeAmount; i++)
      //     {
      //         GameObject tree = Instantiate(Trees[0]);
      //
      //         tree.AddComponent<MeshCollider>();
      //         RaycastHit hit;
      //         bool Placed = false;
      //         while (!Placed)
      //         {
      //             float a = Random.value * 2 * Mathf.PI;
      //             float r = Mathf.Sqrt(Random.value);
      //
      //             float x = r * Mathf.Cos(a);
      //             float z = r * Mathf.Sin(a);
      //
      //             tree.transform.position = new Vector3(transform.position.x + x, transform.localScale.y / 4, transform.position.z + z);
      //             if (Physics.Raycast(tree.transform.position, -tree.transform.up, out hit, 30f))
      //             {
      //                 if (hit.transform.gameObject == gameObject)
      //                 {
      //                     if (TreeTempPoses.Count > 0)
      //                     {
      //                         bool isSafe = false;
      //                         for (int p = 0; p < TreeTempPoses.Count; p++)
      //                         {
      //                             if (Vector3.Distance(TreeTempPoses[p], tree.transform.position) > 0.2f)
      //                             {
      //                                 if (p == TreeTempPoses.Count - 1)
      //                                 {
      //                                     isSafe = true;
      //                                 }
      //                                 continue;
      //                             }
      //                             else
      //                                 break;
      //                         }
      //                         if (isSafe)
      //                             Placed = true;
      //                     }
      //                     else
      //                         Placed = true;
      //                 }
      //             }
      //         }
      //         TreeTempPoses.Add(tree.transform.position);
      //         tree.transform.position = new Vector3(tree.transform.position.x, transform.localScale.y / 5, tree.transform.position.z);
      //
      //         meshFilters[i] = tree.GetComponentInChildren<MeshFilter>();
      //         combine[i].mesh = meshFilters[i].sharedMesh;
      //         combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
      //         meshFilters[i].GetComponent<MeshRenderer>().enabled = false;
      //
      //         tree.transform.localScale = new Vector3(1, 1, 1);
      //         TempObjects.Add(tree);
      //     }
      //     TreeHolderObject.AddComponent<MeshFilter>();
      //     TreeHolderObject.AddComponent<MeshRenderer>();
      //     TreeHolderObject.layer = 2;
      //     TreeHolderObject.transform.GetComponent<MeshFilter>().mesh = new Mesh();
      //     TreeHolderObject.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
      //
      //     if (HasSnow)
      //         TreeHolderObject.GetComponent<MeshRenderer>().material = TreeMats[1];
      //     else
      //         TreeHolderObject.GetComponent<MeshRenderer>().material = TreeMats[0];
      //
      //
      //     TreeHolderObject.transform.SetParent(transform);
      //     TreeHolderObject.transform.name = "Trees";
      //     TreeHolderObject.transform.gameObject.SetActive(true);
      //     for (int i = 0; i < TempObjects.Count; i++)
      //     {
      //         Destroy(TempObjects[i]);
      //     }
      //     TempObjects.Clear();
      // }
      // if (HasStones && StoneAmount > 0)
      // {
      //     if (StoneHolderObject)
      //     {
      //         Destroy(StoneHolderObject);
      //     }
      //     MeshFilter[] meshFilters = new MeshFilter[StoneAmount];
      //     CombineInstance[] combine = new CombineInstance[meshFilters.Length];
      //     StoneHolderObject = new GameObject();
      //     List<Vector3> StoneTempPoses = new List<Vector3>();
      //     for (int i = 0; i < StoneAmount; i++)
      //     {
      //         GameObject stone = Instantiate(Stones[0], new Vector3(transform.position.x + (Random.Range(-1, 1) * Random.value) / 2.5f, transform.localScale.y / 5, transform.position.z + (Random.Range(-1, 1) * Random.value) / 2.5f), new Quaternion(0, Random.Range(0, 180), 0, 0));
      //
      //
      //         stone.AddComponent<MeshCollider>();
      //         RaycastHit hit;
      //         bool Placed = false;
      //         while (!Placed)
      //         {
      //             float a = Random.value * 2 * Mathf.PI;
      //             float r = Mathf.Sqrt(Random.value);
      //
      //             float x = r * Mathf.Cos(a);
      //             float z = r * Mathf.Sin(a);
      //
      //             stone.transform.position = new Vector3(transform.position.x + x, transform.localScale.y / 4, transform.position.z + z);
      //             if (Physics.Raycast(stone.transform.position, -stone.transform.up, out hit, 30f))
      //             {
      //                 if (hit.transform.gameObject == gameObject)
      //                 {
      //                     if (StoneTempPoses.Count > 0)
      //                     {
      //                         bool isSafe = false;
      //                         for (int p = 0; p < StoneTempPoses.Count; p++)
      //                         {
      //                             if (Vector3.Distance(StoneTempPoses[p], stone.transform.position) > 0.05f)
      //                             {
      //                                 if (p == StoneTempPoses.Count - 1)
      //                                 {
      //                                     isSafe = true;
      //                                 }
      //                                 continue;
      //                             }
      //                             else
      //                                 break;
      //                         }
      //                         if (isSafe)
      //                             Placed = true;
      //                     }
      //                     else
      //                         Placed = true;
      //                 }
      //             }
      //         }
      //         StoneTempPoses.Add(stone.transform.position);
      //         stone.transform.position = new Vector3(stone.transform.position.x, transform.localScale.y / 5, stone.transform.position.z);
      //
      //         meshFilters[i] = stone.GetComponentInChildren<MeshFilter>();
      //         combine[i].mesh = meshFilters[i].sharedMesh;
      //         combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
      //         meshFilters[i].GetComponent<MeshRenderer>().enabled = false;
      //
      //         stone.transform.localScale = new Vector3(1, 1, 1);
      //         TempObjects.Add(stone);
      //     }
      //
      //     StoneHolderObject.AddComponent<MeshFilter>();
      //     StoneHolderObject.AddComponent<MeshRenderer>();
      //     StoneHolderObject.layer = 2;
      //     StoneHolderObject.transform.GetComponent<MeshFilter>().mesh = new Mesh();
      //     StoneHolderObject.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
      //     StoneHolderObject.GetComponent<MeshRenderer>().material = StoneMats[0];
      //     StoneHolderObject.transform.SetParent(transform);
      //     StoneHolderObject.transform.name = "Stones";
      //     StoneHolderObject.transform.gameObject.SetActive(true);
      //
      //
      //     for (int i = 0; i < TempObjects.Count; i++)
      //     {
      //         Destroy(TempObjects[i]);
      //     }
      //     TempObjects.Clear();
      // }
      // if (TreeHolderObject && !HasTrees)
      // {
      //     Destroy(TreeHolderObject);
      // }
      // if (StoneHolderObject && !HasStones)
      //{
      //    Destroy(StoneHolderObject);
      //}
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
        terrain.Biome = TileBiome;
        TowersParent = GetComponentInParent<SpawnGrid>().Towers;
        terrainLoader.LoadTerrain(terrain, TerrainRandomNumber, TowersParent);

        for (int i = 0; i < TileProperties.Length; i++)
            TileProperties[i] = terrain.TileTypeCount[i];
    }


}