using DG.Tweening;
using TMPro;
using UnityEngine;

public class FadeMsgBehaviour : MonoBehaviour
{
    public TextMeshPro Text;
    public SpriteRenderer SR;
    public float Duration = 5;
    
    internal void StartProgress(string text, float duration = 3)
    {
        duration = Duration;
        Text.text = text;
        transform.DOMoveY(transform.position.y + 1f, duration);
        Text.DOFade(0, duration);
        SR.DOFade(0, duration);
        Invoke("KillMe", duration + 0.25f);
    }

    private void KillMe()
    {
        Destroy(gameObject);
    }
}