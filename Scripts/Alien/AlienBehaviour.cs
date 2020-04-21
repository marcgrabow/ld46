using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _42.StateMachine;
using Assets;
using DG.Tweening;
using Sirenix.Utilities;
using Templates.SimpleUI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


public class AlienBehaviour : MonoBehaviour
{

    public string AlienId = "Alien #1";
        public enum AlienState
        {
            Alive,
            Dead,
            Stable
        }

        /// <summary>
        /// how many litres of blood remain in the alien
        /// 0 = dead
        /// all body parts can continuously draw blood if not repaired, some faster than others
        /// </summary>
        public float Blood=10f;
        public float BloodLoss
        {
            get { return _bodyParts.Sum(o => o.BloodLoss);  }
        }
        public StateMachine<AlienState> fsm;
        private BodyPartBehaviour[] _bodyParts;

        [Inject] private GameEvents _events;
        private void Awake()
        {
            fsm = StateMachine<AlienState>.Initialize(this, AlienState.Alive);
            _bodyParts = GetComponentsInChildren<BodyPartBehaviour>();
            InvokeRepeating("UpdateAlienStatus", 0, 1);
        }

        void UpdateAlienStatus()
        {
            Blood -= BloodLoss;
            if (Blood < 0) Blood = 0;
            _events.OnAlienUpdate.Invoke(AlienId, Blood);
            if (Blood <= 0)
            {
                CancelInvoke("UpdateAlienStatus");
                StartCoroutine(Die());
            }

            if (BloodLoss <= 0)
            {
                CancelInvoke("UpdateAlienStatus");
                StartCoroutine(Stabilized());
            }
        }

        private IEnumerator Stabilized()
        {
            DisableBodyParts();
            _events.OnShowMsg.Invoke(new Msg("Stable!", transform.position+new Vector3(0, 2)));
            yield return new WaitForSeconds(3);
            fsm.ChangeState(AlienState.Stable);
            _events.OnAlienStabilized.Invoke(this);
        }

        void DisableBodyParts()
        {
            // just to be safe
            _bodyParts.ForEach(b => b.Deactivate());
        }

        private IEnumerator Die()
        {
            DisableBodyParts();
            _events.OnShowMsg.Invoke(new Msg("Died...", transform.position+new Vector3(0, 2)));
            var srs = GetComponentsInChildren<SpriteRenderer>();
            var b = Color.black;
            foreach (var sr in srs)
            {
                sr.DOColor(b, Random.Range(1,3));
            }
            yield return new WaitForSeconds(3);
            fsm.ChangeState(AlienState.Dead);
            _events.OnAlienDied.Invoke(this);
        }
}


/// <summary>
/// name, blood amount
/// </summary>
public class AlienUpdateEvent : UnityEvent<string, float> {}

public class AlienDiedEvent : UnityEvent<AlienBehaviour> {}
public class AlienStabilizedEvent : UnityEvent<AlienBehaviour> {}

