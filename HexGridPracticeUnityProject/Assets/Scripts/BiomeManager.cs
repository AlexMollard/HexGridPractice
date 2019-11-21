using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileProperties
{
    public int CellType;
    public int Towertype;
    public int TowerIndex;
    public Color BiomeFilter;
}

public class BiomeManager : MonoBehaviour
{
    //public enum BiomeType { Taiga, Savanna, Tundra, RainForest, Desert, Forest, Plains, Snow, Ocean, Beach, Bare, Scorched, HotRainForest, WetDesert, TropSeasonForest, DeepOcean }
    public enum CellType { Water, Sand, Dirt, Grass, Stone, Snow };
    public enum TowerType {Ore,Tree,Shrub,Animal};
    public enum OreType { Lipidolite, RockSalt, Carallite, Limestone, Cuprites, Bauxite, Zencite, Galena, Magnetite };
    public enum TreeType { Conifer, Deciduous, Spruce, Acacia, SnowPine, RubberTree, JoshuaTree, PaloVerde, Cedar, Oak, Maple, CorkTree, ManGroves, BlackWood, BurntTree, KapokTree, Ceiba, Bamboo };
    public enum ShrubType { BlueBerries, HaircapMoss, Cloudberry, RedBerry, Cactus };
    public enum AnimalType { Cow, Sheep, Deer, Wolf, Snake, Goat };

    TileProperties properties;

    private void Start()
    {
        properties = new TileProperties();
    }

    public void ResetProperties()
    {
        properties.CellType = 0;
        properties.Towertype = 0;
        properties.TowerIndex = 0;
    }


    public TileProperties Taiga(float Altitude, float Humidity)
    {
        ResetProperties();

        properties.BiomeFilter = new Color(0.1f, 0.4f, 0.1f);
        if (Altitude < 0.3f)
        {
            properties.CellType = (int)CellType.Water;
            properties.Towertype = (int)TowerType.Shrub;
             properties.TowerIndex = 404;

        }
        else if (Altitude > 0.7f)
        {
            properties.CellType = (int)CellType.Stone;
            properties.Towertype = (int)TowerType.Shrub;
            if (Humidity < 0.1f)  properties.TowerIndex = 0;
            else if (Humidity < 0.2f)  properties.TowerIndex = 1;
            else if (Humidity < 0.5f)  properties.TowerIndex = 2;
            else  properties.TowerIndex = 404;
        }

        else if (Altitude > 0.4f)
        {
            properties.CellType = (int)CellType.Grass;
            properties.Towertype = (int)TowerType.Tree;
            if (Humidity < 0.33f)  properties.TowerIndex = 1;
            else if (Humidity < 0.5f)  properties.TowerIndex = 404;
            else if (Humidity < 0.75f)  properties.TowerIndex = 404;
            else  properties.TowerIndex = 2;
        }

        else
        {
            properties.CellType = (int)CellType.Sand;
            properties.Towertype = (int)TowerType.Ore;

            if (Humidity < 0.16f)  properties.TowerIndex = 0;
            else if (Humidity < 0.25f)  properties.TowerIndex = 1;
            else if (Humidity < 0.3f)  properties.TowerIndex = 2;
            else  properties.TowerIndex = 404;
        }

        return properties;
    }

    //public Vector3 Savanna(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //
    //public Vector3 Tundra(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 RainForest(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 Desert(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 Forest(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 Plains(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 Snow(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 Ocean(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 Beach(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 Bare(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 Scorched(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 HotRainForest(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 WetDesert(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //public Vector3 TropSeasonForest(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
    //
    //public Vector3 DeepOcean(float Altitude, float Humidity)
    //{
    //    ResetProperties();
    //    return properties;
    //}
}
