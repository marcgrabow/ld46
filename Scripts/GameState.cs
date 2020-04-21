using System.Collections;
using _42.StateMachine;
using UnityEngine;

namespace Templates.SimpleUI
{
    public enum GameStateEnum
    {
        Loading,
        Idle,
        Running,
        GameOver
    }
    
    public class GameState : MonoBehaviour
    {
        public StateMachine<GameStateEnum> fsm;

        private void Awake()
        {
           fsm = StateMachine<GameStateEnum>.Initialize(this, GameStateEnum.Loading);
           fsm.Changed += FsmOnChanged;
        }

        // private IEnumerator Start()
        // {
        //     // fsm.ChangeState(GameStateEnum.Idle);
        //     // yield return new WaitForSeconds(1);
        //     // fsm.ChangeState(GameStateEnum.Running);
        //     // yield return new WaitForSeconds(1);
        //     // fsm.ChangeState(GameStateEnum.GameOver);
        // }

        private void FsmOnChanged(GameStateEnum obj)
        {
            Debug.Log($"Fsm from ${fsm.LastState} to ${fsm.State}");
        }
    }
}