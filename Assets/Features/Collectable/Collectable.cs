using System;
using TTT.Utillities.Pooling;
using UnityEngine;

public class Collectable : Spawnable 
{
    public Guid id;
    public Action<Guid> onCollected;
    
    private void Awake()
    {
        OnSpawn += Initialize;
    }

    private void Initialize()
    {
        id = Guid.NewGuid();
    }

    protected virtual void TriggerEnter(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        onCollected?.Invoke(id);
        Despawn();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        TriggerEnter(other);
    }
}
