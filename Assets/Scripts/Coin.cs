using UnityEngine;

namespace Assets.Scripts
{
    public class Coin
    {
        public bool taken;
        public Vector3 position;

        private GameObject coinGameObject;

        public void SpawnCoin(GameObject coinGO, Vector3 coinPostion, Transform parent)
        {
            coinGameObject = Object.Instantiate(coinGO, coinPostion, Quaternion.identity, parent);
            position = coinPostion;
        }

        public void Taken()
        {
            taken = true;
            Object.Destroy(coinGameObject);
        }
    }
}
