using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TMPro;

namespace Buga
{

    public enum GameState
    {
        Menu,
        Game,
        End
    }

    public class GameManager : MonoBehaviour
    {
        public GameState state = GameState.Menu;

        [SerializeField]
        protected int mayhemScore = 0;

        [SerializeField]
        protected float sessiontime;

        protected float currentSessionTime;
        public float CurrentSessionTime
        {
            get => currentSessionTime;
        }


        public string startButton = "P1XButton";

        public GameObject titleScreen;
        public GameObject endScreen;


        public static UnityAction OnGameStarted;
        public static UnityAction OnGameEnded;

        public TextMeshProUGUI timeText;
        public TextMeshProUGUI mayhemtext;

        private void Awake()
        {
            Breakable.OnBreakableBroken += OnBreakableBroken;
            RepairController.OnRepairCompleted += OnRepairCompleted;
            ResetGame();
        }
        public void StartGame()
        {
            titleScreen.SetActive(false);
            currentSessionTime = sessiontime;
            OnGameStarted?.Invoke();
            state = GameState.Game;

            mayhemtext.gameObject.SetActive(true);
            mayhemtext.text = $"Mayhem {mayhemScore}";

            timeText.gameObject.SetActive(true);



        }

        public void EndGame()
        {
            state = GameState.End;
            OnGameEnded?.Invoke();

            endScreen.SetActive(true);
        }


        private void OnDestroy()
        {
            Breakable.OnBreakableBroken -= OnBreakableBroken;
        }

        protected void OnRepairCompleted(bool status, int score)
        {
            mayhemScore += score;
        }

        protected void OnBreakableBroken()
        {
            mayhemScore++;
        }

        public void ResetGame()
        {

            titleScreen.SetActive(true);
            endScreen.SetActive(false);
            state = GameState.Menu;
        }


        private void Update()
        {

            switch (state)
            {
                case GameState.Menu:
                    if (Input.GetButtonDown(startButton))
                    {
                        Debug.Log(startButton + " pressed");
                        StartGame();
                    }
                    break;
                case GameState.Game:
                    RunGame();
                    if (currentSessionTime <= 0)
                    {
                        EndGame();
                    }
                    break;
                case GameState.End:
                    if (Input.GetButtonDown(startButton))
                    {
                        ResetGame();
                    }

                    break;
                default:
                    break;
            }
        }


        protected void RunGame()
        {

            mayhemtext.text = $"Mayhem {mayhemScore}";

            currentSessionTime -= Time.deltaTime;

            int totalSeconds = (int)currentSessionTime;
            int seconds = totalSeconds % 60;
            int minutes = totalSeconds / 60;
            string time = minutes + ":" + seconds;

            timeText.text = $"Time left: {time}";

        }
    }
}
