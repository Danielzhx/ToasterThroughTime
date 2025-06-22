using System;
using System.Collections.Generic;
using TTT.UnityExtensions;
using TTT.Utillities;
using TTT.Utillities.Pooling;
using UnityEngine;

namespace TTT
{
    public class CollectablesManager : SingletonMonobehaviour<CollectablesManager>
    {
        [Header("Toast Coin Settings")]
        public ToastCoin toastCoinPrefab;
        public Transform toastCoinContainer;
        public Transform[] fixedSpawnLocations;
    
        private ObjectPool<ToastCoin> toastCoinPool;
        private List<ToastCoin> activeToastCoins;
        [HideInInspector] public int coinsCollected = 0;
    
        private void Awake()
        {
            ResetPools();
            activeToastCoins = new();
        
            foreach (Transform trans in fixedSpawnLocations)
            {
                ToastCoin spawn = toastCoinPool.Spawn(trans.position);
                spawn.onCollected += (Guid id) => activeToastCoins.Remove(spawn);
                spawn.onCollected += (Guid id) => coinsCollected++;
                activeToastCoins.Add(spawn);
            }
        }

        public void SubscribeToAllCoins(Action<Guid> meth)
        {
            foreach (var coin in activeToastCoins)
            {
                coin.onCollected += meth;
            }
        }

        private void ResetPools(bool reMakePools = true)
        {
            toastCoinContainer.Clear();
            PoolManager.Instance.Remove(toastCoinPrefab.name);
            if (reMakePools)
            {
                toastCoinPool =
                    new ObjectPool<ToastCoin>().CreateObjectPool(toastCoinPrefab, toastCoinContainer,
                        (uint)fixedSpawnLocations.Length);
            }
        }
    }
}

