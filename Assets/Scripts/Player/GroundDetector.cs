using UnityEngine;
using System.Collections.Generic;

public class GroundDetector : MonoBehaviour
{
    private HashSet<Collider2D> _groundCollisions = new HashSet<Collider2D>();
    
    public bool IsGround => _groundCollisions.Count > 0;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Ground>(out _))
        {
            _groundCollisions.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Ground>(out _))
        {
            _groundCollisions.Remove(other);
        }
    }

    private void OnDisable()
    {
        _groundCollisions.Clear();
    }
}