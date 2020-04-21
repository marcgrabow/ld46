using System;
using DG.Tweening;
using UnityEngine;

    public class FadeCircleBehaviour : MonoBehaviour
    {
        public float FadeDuration = 1;
        
        private void Start()
        {
            var sr = GetComponent<SpriteRenderer>();
            sr.DOFade(0, FadeDuration).SetEase(Ease);
            Invoke("KillMe", FadeDuration + 0.25f);
        }

        public Ease Ease;

        private void KillMe()
        {
            Destroy(gameObject);
        }
    }
