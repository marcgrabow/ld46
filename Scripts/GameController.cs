using System;
using System.Collections;
using System.Linq;
using _42.Dialogs;
using Templates.SimpleUI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameController : MonoBehaviour
{
    private AlienBehaviour[] _aliens;

    private void Awake()
    {
        _aliens = GameObject.FindObjectsOfType<AlienBehaviour>();
    }

    private void Start()
    {
        InvokeRepeating("CheckAllAliens", 1, 1);
    }

    private void CheckAllAliens()
    {
        if (_gameOver) return;
        var total = _aliens.Length;
        var dead = _aliens.Count(o => o.fsm.State == AlienBehaviour.AlienState.Dead);
        var stable = _aliens.Count(o => o.fsm.State == AlienBehaviour.AlienState.Stable);
        Debug.Log(total+" "+dead+" "+stable);
        
        if (dead == total)
        {
            var txt = "Oh no!";
            if (dead == 1)
            {
                txt += " The alien died!";
            }
            else
            {
                txt += " All aliens died!";
            }
            txt += "\n\nGuess you have to try again?";
            var d = _dialogs.Create("Game Over", txt, new string[] {"Try again", "Quit to main menu"});
            d.OnClose.AddListener((int btn) =>
            {
                if (btn == 0) ReloadScene();
                if(btn==1) GoToMainMenu();
            });
            
            _gameOver = true;
        }

        if (!_gameOver && dead + stable == total)
        {
            var txt = dead > 0 ? "Congratulations!" : "Excellent! Fantastic work!";
            if (dead > 0)
            {
                txt += " Even though "+dead+" alien died, you still saved "+stable+" other alien.";
            }
            else
            {
                if (stable > 1)
                {
                    txt += " You saved all aliens!";
                }
                else
                {
                    txt += " You saved the alien!";
                }
            }
            var d = _dialogs.Create("Well done!", txt, new string[] {"Back to main menu"});
            d.OnClose.AddListener((int btn) => { GoToMainMenu(); });
            _gameOver = true;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("menu");
    }


    [Inject] private DialogFactory _dialogs;
    [Inject] private GameEvents _events;
    private bool _gameOver;
}
