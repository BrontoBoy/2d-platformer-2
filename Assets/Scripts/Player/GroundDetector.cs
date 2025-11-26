using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private int _groundCollisionCount = 0;
    
    public bool IsGround => _groundCollisionCount > 0;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Ground>(out _))
        {
            _groundCollisionCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Ground>(out _))
        {
            _groundCollisionCount--;
        }
    }

    private void OnDisable()
    {
        _groundCollisionCount = 0;
    }
}