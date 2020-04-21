using UnityEngine;

[CreateAssetMenu(menuName = "ld46/Tool")]
public class ToolConfig  :ScriptableObject
{
    public string Title = "Tool";
    public ToolTypeEnum ToolType;
    public Sprite Sprite;
    public float PickupDuration = 1;
    public string PickupText = "Picking up tool";
    public bool OneTimeUse = false;
}

public enum ToolTypeEnum {
    None,
    BoneSaw,
    Syringe,
    Bandaid,
    Flamethrower,
    Scanner,
    Forceps,
    DuctTape,
    Vaccuum,
    Stapler
}


