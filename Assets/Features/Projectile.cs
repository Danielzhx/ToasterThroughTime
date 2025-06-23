using System;
using TTT.Utillities.Pooling;
using UnityEngine;

namespace TTT
{
    public class Projectile : Spawnable
    {
        public bool destroyOnHit;
        public float movementSpeed;
        public float damage;
        public float timeToLive;
        
        protected float ElapsedTime;

        private void Awake()
        {
            OnSpawn += Initialize;
        }

        protected virtual void Initialize()
        {
            ElapsedTime = 0f;
        }

        protected virtual void tick()
        {
            transform.position += transform.right * (movementSpeed * Time.deltaTime);
        }

        private void Update()
        {
            tick();
        }

        protected virtual void TriggerEnter(Collider2D other)
        {
            if (destroyOnHit)
            {
                Despawn();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEnter(other);
        }
    }
}

