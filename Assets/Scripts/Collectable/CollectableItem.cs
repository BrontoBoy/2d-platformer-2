using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class CollectableItem : MonoBehaviour, ICollectable
{
    public event System.Action<ICollectable> Collected;
    
    public virtual void Collect()
    {
        Collected?.Invoke(this);
    }
}