using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collector : MonoBehaviour
{   
    public event System.Action<ICollectable> ItemCollected;
    
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.TryGetComponent(out ICollectable collectable)) 
        {
            ItemCollected?.Invoke(collectable);
            collectable.Collect();
        }
    }
}