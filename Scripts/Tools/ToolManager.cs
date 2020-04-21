using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public const string WateringCan = "WateringCan";
    public const string Rake = "Rake";
    public const string Gloves = "Gloves";
    public const string WeedPuller = "WeedPuller";
    public const string WeedKiller = "WeedKiller";

    public static ToolManager Instance;

    public ToolBehaviour[] Tools;


    // Use this for initialization
    private void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    //internal void Use(string tool, TileBehaviour tile, Vector3 arg0)
    //{


    //    if (tool.StartsWith("Seed:"))
    //    {
    //        var pm = PlantManager.Instance;
    //        var seed = tool.Replace("Seed:", "");
    //        if (pm.CanPlant(seed))
    //        {
    //            pm.Plant(seed, arg0);
    //        }
    //        else
    //        {
    //            Debug.Log("Not enough seeds");
    //        }
    //    }

    //    if (tool.StartsWith("Tool:"))
    //    {

    //        var toolName = tool.Replace("Tool:", "");

    //            Debug.Log("Use "+toolName);

    //        tile.GrowWeed();

    //    }

    //}
}