using DG.Tweening;
using TMPro;
using UnityEngine;

public class ProgressBarBehaviour : MonoBehaviour
{
    public SpriteRenderer ProgressLayer;
    public TextMeshPro Text;

    // Use this for initialization
    private void Awake()
    {
        ProgressLayer.transform.DOScaleX(0, 0);
    }

    internal void StartProgress(string text, float duration)
    {
        Text.text = text;
        ProgressLayer.transform.DOScaleX(1, duration);
        Invoke("KillMe", duration + 0.25f);
    }

    private void KillMe()
    {
        Destroy(gameObject);
    }
}