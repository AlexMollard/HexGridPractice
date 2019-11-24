using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public enum TowerType { Enviroment, Production, Housing};
    public enum EnviromentTower {Ore, Tree, Shrub, Animal};
    public enum ProductionTower { Sawmill, Mine, AnimalFarm, CropFarm, PowerPlant};
    public enum HousingTower {Hut, Tank, Storage, CampFire};

    public TerrainLoader terrainLoader;
    public TerrainBehavior terrainBehavior;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TerrainLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
