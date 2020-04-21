    using System;
    using TMPro;
    using UnityEngine;
    using Zenject;

    public class GameStatusDisplay : MonoBehaviour
    {
        private TextMeshProUGUI _txt;
        
        [Multiline]
        public string Format;
        
        [Inject] private GameEvents _events;
        
        private void Awake()
        {
            _txt = GetComponent<TextMeshProUGUI>();
            _events.OnAlienUpdate.AddListener(AlienUpdate);    
        }

        private void AlienUpdate(string name, float blood)
        {
            _txt.text = string.Format(Format, name, blood);
        }
    }
