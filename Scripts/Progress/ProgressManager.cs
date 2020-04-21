using Assets;
using UnityEngine;
using Zenject;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;

    [Inject] private GameEvents _gameEvents;
    public FadeMsgBehaviour MsgPrefab;
    public Vector3 Offset;

    public ProgressBarBehaviour Prefab;

    private void Start()
    {
        Instance = this;
        _gameEvents.OnShowMsg.AddListener(OnMsg);
    }

    public float Show(string text, float duration, Vector3 pos)
    {
        var go = Instantiate(Prefab, pos + Offset, Quaternion.identity);
        go.StartProgress(text, duration);
        return duration;
    }

    public void OnMsg(Msg msg)
    {
        var go = Instantiate(MsgPrefab, msg.Pos, Quaternion.identity);
        go.StartProgress(msg.Text);
    }
}