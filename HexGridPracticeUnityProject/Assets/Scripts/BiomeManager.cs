using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileProperties
{
    public int CellType;
    public int Towertype;
    public int TowerIndex;
    public Color BiomeFilter;
    public float TileAltitude;
    public float TileHumidity;
    public Vector2 TilePos;
}

public class BiomeManager : MonoBehaviour
{
    //public enum BiomeType { Taiga, Savanna, Tundra, RainForest, Desert, Forest, Plains, Snow, Ocean, Beach, Bare, Scorched, HotRainForest, WetDesert, TropSeasonForest, DeepOcean }
    public enum CellType { Water, Sand, Dirt, Grass, Stone, Snow, Ice, Lava };
    public enum TowerType {Ore,Tree,Shrub,Animal};
    public enum OreType { Lipidolite, RockSalt, Carnallite, Limestone, Cuprites, Bauxite, Zencite, Galena, Magnetite, Gold, Coal, Oil};
    public enum TreeType { Conifer, Deciduous, Spruce, Acacia, SnowPine, RubberTree, JoshuaTree, PaloVerde, Cedar, Oak, Maple, CorkTree, ManGroves, BlackWood, BurntTree, KapokTree, Ceiba, Bamboo };
    public enum ShrubType { BlueBerries, HaircapMoss, Cloudberry, RedBerry, Cactus, WaxMyrtle };
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
        properties.TileHumidity = Humidity;
        properties.TileAltitude = Altitude;
        properties.BiomeFilter = new Color(0.1f, 0.4f, 0.1f);
        if (Altitude < 0.25f)
        {
            properties.CellType = (int)CellType.Water;
            properties.Towertype = (int)TowerType.Shrub;
             properties.TowerIndex = 404;

        }
        else if (Altitude > 0.7f)
        {
            properties.CellType = (int)CellType.Snow;
            properties.Towertype = (int)TowerType.Shrub;
            if (Random.value > 0.25f)
            {
                if (Humidity < 0.1f) properties.TowerIndex = 0;
                else if (Humidity < 0.2f) properties.TowerIndex = 1;
                else if (Humidity < 0.5f) properties.TowerIndex = 2;
                else properties.TowerIndex = 404;
            }
            else properties.TowerIndex = 404;

        }

        else if (Altitude > 0.5f)
        {
            properties.CellType = (int)CellType.Grass;
            properties.Towertype = (int)TowerType.Tree;
            if (Random.value > 0.45f)
            {
                if (Humidity < 0.73f) properties.TowerIndex = 404;
                else if (Humidity < 0.80f) properties.TowerIndex = (int)TreeType.Spruce;
                else if (Humidity < 0.90f) properties.TowerIndex = (int)TreeType.Conifer;
                else properties.TowerIndex = (int)TreeType.Deciduous;
            }
            else
                properties.TowerIndex = 404;
        }
        else if (Altitude > 0.35f)
        {
            properties.CellType = (int)CellType.Dirt;
            properties.Towertype = (int)TowerType.Shrub;

            if (Random.value > 0.25f)
            {
                if (Humidity > 0.50f) properties.TowerIndex = (int)ShrubType.HaircapMoss;
                else if (Humidity < 0.73f) properties.TowerIndex = 404;
                else properties.TowerIndex = (int)ShrubType.BlueBerries;
            }
            else
                properties.TowerIndex = 404;
        }

        else
        {
            properties.CellType = (int)CellType.Sand;
            properties.Towertype = (int)TowerType.Ore;
            if (Random.value > 0.65f)
            {
                if (Humidity < 0.25f) properties.TowerIndex = (int)OreType.Magnetite;
                else if (Humidity < 0.45f && Humidity > 0.35f) properties.TowerIndex = (int)OreType.Coal;
                else if (Humidity < 0.9f && Humidity > 0.95f) properties.TowerIndex = (int)OreType.Gold;
                else properties.TowerIndex = 404;
            }
            else properties.TowerIndex = 404;
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
    public TileProperties Ocean(float Altitude, float Humidity, Vector2 tilePos)
    {
        ResetProperties();
        Altitude -= (tilePos.x / 20) + (tilePos.y / 20);
        properties.TileHumidity = Humidity;
        properties.TileAltitude = Altitude;
        properties.CellType = (int)CellType.Water;
        properties.Towertype = (int)TowerType.Shrub;
        properties.TowerIndex = 404;

        if (Altitude > 0.2f && Altitude < 0.4f)
        {
            properties.CellType = (int)CellType.Sand;
            if (Humidity < 0.4f)
            {
                properties.CellType = (int)CellType.Sand;
                properties.Towertype = (int)TowerType.Tree;
                properties.TowerIndex = (int)TreeType.ManGroves;
            }
        }
        return properties;
    }
    public TileProperties Beach(float Altitude, float Humidity,Vector2 tilePos)
    {
        ResetProperties();
        Altitude -= (tilePos.x / 50) + (tilePos.y / 50);
        properties.TileHumidity = Humidity;
        properties.TileAltitude = Altitude;
        properties.TilePos = tilePos;

        properties.CellType = (int)CellType.Water;
        properties.Towertype = (int)TowerType.Shrub;
        properties.TowerIndex = 404;

        if (Altitude > 0.8f)
        {
            properties.CellType = (int)CellType.Dirt;
            if (Humidity < 0.4f)
            {
                properties.CellType = (int)CellType.Dirt;
                properties.Towertype = (int)TowerType.Tree;
                properties.TowerIndex = (int)TreeType.Maple;
            }
            else if (Humidity > 0.6f)
            {
                properties.CellType = (int)CellType.Dirt;
                properties.Towertype = (int)TowerType.Shrub;
                properties.TowerIndex = (int)ShrubType.RedBerry;
            }
        }
        else if (Altitude > 0.2f)
        {
            properties.CellType = (int)CellType.Sand;
            if (Humidity < 0.4f)
            {
                properties.CellType = (int)CellType.Sand;
                properties.Towertype = (int)TowerType.Tree;
                properties.TowerIndex = (int)TreeType.ManGroves;
            }
            else if (Humidity > 0.6f)
            {
                properties.CellType = (int)CellType.Sand;
                properties.Towertype = (int)TowerType.Shrub;
                properties.TowerIndex = (int)ShrubType.WaxMyrtle;
            }
        }

        return properties;
    }
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
    public TileProperties DeepOcean(float Altitude, float Humidity)
    {
        ResetProperties();
        properties.TileHumidity = Humidity;
        properties.TileAltitude = Altitude;
        properties.CellType = (int)CellType.Water;
        properties.Towertype = (int)TowerType.Shrub;
        properties.TowerIndex = 404;

        if (Altitude > 0.2f && Altitude < 0.32f)
        {
            properties.CellType = (int)CellType.Sand;
            if (Humidity < 0.4f)
            {
                properties.CellType = (int)CellType.Water;
                properties.Towertype = (int)TowerType.Ore;
                properties.TowerIndex = (int)OreType.Oil;
            }

        }
        return properties;
    }
}
