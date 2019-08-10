using UnityEngine;

namespace Assets.Scripts
{
    public class CoinManager
    {
        private readonly GameObject coinGameObject;

        private Vector3 minCoinPosition;
        private Vector3 maxCoinPosition;

        public int coinsTaken;
        private float distanceToTakeCoin = 0.8f;

        private Coin[] coins;

        public CoinManager()
        {
            coinGameObject = Resources.Load<GameObject>("Prefabs/Coin");
            minCoinPosition = GameObject.Find("minCoinPosition").transform.position;
            maxCoinPosition = GameObject.Find("maxCoinPosition").transform.position;
        }

        public void GenerateCoins(int count)
        {
            var parent = new GameObject("Coins");
            coins = new Coin[count];

            for (var i = 0; i < count; i++)
            {
                var coin = new Coin();
                coins[i] = coin;
                coin.SpawnCoin(coinGameObject, FindPositionForCoin(), parent.transform);
            }
        }

        private Vector3 FindPositionForCoin()
        {
            RaycastHit hit;

            do
            {
                var coinPosition = new Vector3(Random.Range(minCoinPosition.x, maxCoinPosition.x), 
                                                100, 
                                                Random.Range(minCoinPosition.z, maxCoinPosition.z));
                Physics.Raycast(coinPosition, Vector3.down, out hit);
            } while (hit.transform.name == coinGameObject.name + "(Clone)");
            
            return hit.point;
        }

        public void CheckDistanceCoinsPlayer(Vector3 playerPosition)
        {
            foreach (var coin in coins)
            {
                if (!coin.taken && Vector3.Distance(coin.position, playerPosition) < distanceToTakeCoin)
                {
                    coinsTaken++;
                    coin.Taken();
                }
            }
        }
    }
}
