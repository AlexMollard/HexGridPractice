using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeManager : MonoBehaviour
{
    //public enum BiomeType { Taiga, Savanna, Tundra, RainForest, Desert, Forest, Plains, Snow, Ocean, Beach, Bare, Scorched, HotRainForest, WetDesert, TropSeasonForest, DeepOcean }
    public enum CellType { Water, Sand, Dirt, Grass, Stone, Snow, Ice, Lava };
    public enum TowerType {Ore,Tree,Shrub,Animal};
    public enum OreType { Lipidolite, RockSalt, Carnallite, Limestone, Cuprites, Bauxite, Zencite, Galena, Magnetite, Gold, Coal, Oil};
    public enum TreeType { Conifer, Deciduous, Spruce, Acacia, SnowPine, RubberTree, JoshuaTree, PaloVerde, Cedar, Oak, Maple, CorkTree, ManGroves, BlackWood, BurntTree, KapokTree, Ceiba, Bamboo };
    public enum ShrubType { BlueBerries, HaircapMoss, Cloudberry, RedBerry, Cactus, WaxMyrtle };
    public enum AnimalType { Cow, Sheep, Deer, Wolf, Snake, Goat };

    public float[] BiomeSeaLevel = {0.35f,0.1f,0.2f};

    public Cell Taiga(Cell cell, int seed)
    {
        if (cell.CellType != CellType.Water)
        {
            // Debug.Log(cell.TilePostition + "> H: " + cell.Humidity + " A: " + cell.Altitude);

            cell.TowerIndex = 404;

            if (cell.Altitude < 0.65f)
            {
                cell.CellType = CellType.Grass;

                if (cell.Humidity > 0.45f)
                    cell.CellType = CellType.Dirt;

                if (UnityEngine.Random.value / 2 > cell.Humidity)
                {
                    cell.TowerType = TowerType.Tree;
                    cell.TowerIndex = (int)TreeType.Deciduous;
                }

            }
            else if (cell.Altitude < 0.85f)
                cell.CellType = CellType.Stone;
            else
                cell.CellType = CellType.Snow;



        }
        else
        {
            cell.TowerIndex = 404;
        }

        return cell;
    }

    public Cell Ocean(Cell cell, int seed)
    {
        cell.Altitude -= (cell.TilePostition.x / 20) + (cell.TilePostition.y / 20);
        cell.CellType = CellType.Water;
        cell.TowerType = TowerType.Shrub;
        cell.TowerIndex = 404;

        if (cell.Altitude > 0.2f && cell.Altitude < 0.4f)
        {
            cell.CellType = CellType.Sand;
            if (cell.Humidity < 0.4f)
            {
                cell.CellType = CellType.Sand;
                cell.TowerType = TowerType.Tree;
                cell.TowerIndex = (int)TreeType.ManGroves;
            }
        }
        return cell;
    }
    public Cell Beach(Cell cell, int seed)
    {
        cell.Altitude -= (cell.TilePostition.x / 50) + (cell.TilePostition.y / 50);

        cell.CellType = CellType.Water;
        cell.TowerType = TowerType.Shrub;
        cell.TowerIndex = 404;

        if (cell.Altitude > 0.8f)
        {
            cell.CellType = CellType.Dirt;
            if (cell.Humidity < 0.4f)
            {
                cell.CellType = CellType.Dirt;
                cell.TowerType = TowerType.Tree;
                cell.TowerIndex = (int)TreeType.Maple;
            }
            else if (cell.Humidity > 0.6f)
            {
                cell.CellType = CellType.Dirt;
                cell.TowerType = TowerType.Shrub;
                cell.TowerIndex = (int)ShrubType.RedBerry;
            }
        }
        else if (cell.Altitude > 0.2f)
        {
            cell.CellType = CellType.Sand;
            if (cell.Humidity < 0.4f)
            {
                cell.CellType = CellType.Sand;
                cell.TowerType = TowerType.Tree;
                cell.TowerIndex = (int)TreeType.ManGroves;
            }
            else if (cell.Humidity > 0.6f)
            {
                cell.CellType = CellType.Sand;
                cell.TowerType = TowerType.Shrub;
                cell.TowerIndex = (int)ShrubType.WaxMyrtle;
            }
        }

        return cell;
    }
    public Cell DeepOcean(Cell cell, int seed)
    {
        cell.CellType = CellType.Water;
        cell.TowerType = TowerType.Shrub;
        cell.TowerIndex = 404;

        if (cell.Altitude > 0.2f && cell.Altitude < 0.32f)
        {
            cell.CellType = CellType.Sand;
            if (cell.Humidity < 0.4f)
            {
                cell.CellType = CellType.Water;
                cell.TowerType = TowerType.Ore;
                cell.TowerIndex = (int)OreType.Oil;
            }

        }
        return cell;
    }
}
