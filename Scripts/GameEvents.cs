using _42.Events;
using Assets;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents
{
    public MsgEvent OnShowMsg = new MsgEvent();
    public BodyPartClickEvent OnBodyPartClicked = new BodyPartClickEvent();
    public TileClickEvent OnTileClicked = new TileClickEvent();
    public AlienUpdateEvent OnAlienUpdate = new AlienUpdateEvent();
    public AlienStabilizedEvent OnAlienStabilized = new AlienStabilizedEvent();
    public AlienDiedEvent OnAlienDied = new AlienDiedEvent();
    public DefaultEvent OnToolPickedUp = new DefaultEvent();
    public DefaultEvent OnScannerComplete = new DefaultEvent();
    public DefaultEvent OnTreatmentComplete = new DefaultEvent();
}

public class MsgEvent : UnityEvent<Msg>
{
}

public class BodyPartClickEvent : UnityEvent<BodyPartBehaviour>
{
}

/// <summary>
/// Vector3Int: cell coords
/// Transform: dunno yet, cell transform
/// </summary>
public class TileClickEvent : UnityEvent<Vector3Int> {}