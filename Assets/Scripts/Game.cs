using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        private CoinManager coinManager;
        public int coinsCount;

        private PlayerController player;

        public Text timerText;
        public float timer;
        private int currentTime;

        public Text coinsTakenCountText;
        private int currentCoinsTaken = -1;

        public Text messageText;

        public Button restartButton;

        private Vector2 finishPosition;

        private float distanceToWin = 1;

        private bool win;

        private bool loose;

        void Start()
        {
            SetTimerText();

            restartButton.onClick.AddListener(Restart);

            player = new PlayerController();
            player.SpawnPlayer();

            coinManager = new CoinManager();
            coinManager.GenerateCoins(coinsCount);

            var finish = GameObject.Find("Finish").transform.position;
            finishPosition = new Vector2(finish.x, finish.z);
        }

        void Update()
        {
            if (loose)
                return;

            if (win)
            {
                player.Win();
                return;
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if ((int) timer < currentTime)
                {
                    SetTimerText();
                }

                player.Move();
                player.Rotate();
                player.SetAnimatorParams();

                coinManager.CheckDistanceCoinsPlayer(player.position);

                if (coinManager.coinsTaken > currentCoinsTaken)
                {
                    currentCoinsTaken = coinManager.coinsTaken;
                    coinsTakenCountText.text = "Taken coins: " + currentCoinsTaken;
                }

                CheckDistanceToFinish();
            }
            else
            {
                Loose();
            }
        }

        private void SetTimerText()
        {
            currentTime = (int)timer;
            timerText.text = currentTime.ToString();
        }

        private void CheckDistanceToFinish()
        {
            if (Vector2.Distance(finishPosition, player.positionV2) < distanceToWin)
            {
                Win();
            }
        }

        private void Win()
        {
            win = true;
            messageText.text = "Congratulation! Score: " + currentCoinsTaken;
            messageText.gameObject.SetActive(true);
        }

        private void Loose()
        {
            messageText.text = "Game Over!";
            messageText.gameObject.SetActive(true);
            player.Death();
            loose = true;
        }

        private void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}
