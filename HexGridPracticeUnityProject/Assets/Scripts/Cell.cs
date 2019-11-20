using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public enum CellType {Water, Sand, Dirt, Grass, Stone, Snow };
    public enum OreType {Lipidolite, RockSalt, Carallite, Limestone, Cuprites, Bauxite, Zencite, Galena, Magnetite};
    public enum TreeType {Conifer, Deciduous, Spruce, Acacia, SnowPine, RubberTree, JoshuaTree, PaloVerde, Cedar, Oak, Maple, CorkTree, ManGroves, BlackWood, BurntTree, KapokTree, Ceiba, Bamboo};

    public enum ShrubType {BlueBerries, HaircapMoss, Cloudberry, RedBerry, Cactus };
    public enum AnimalType {Cow, Sheep, Deer, Wolf, Snake, Goat };
    // Up to Savanna
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
