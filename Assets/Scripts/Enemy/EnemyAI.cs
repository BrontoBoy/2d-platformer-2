using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Collider2D _detectionZone;
    
    private Transform _player;
    private bool _isPlayerDetected = false;
    
    public bool IsPlayerDetected => _isPlayerDetected;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            _player = player.transform;
            _isPlayerDetected = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            _player = null;
            _isPlayerDetected = false;
        }
    }
    
    public float GetDirectionToPlayer()
    {
        if (_player == null) return 0f;
        
        Vector2 direction = _player.position - transform.position;
        return Mathf.Sign(direction.x);
    }
}