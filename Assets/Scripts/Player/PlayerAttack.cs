using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 50;
    [SerializeField] private Collider2D _attackCollider;
    
    private bool _isAttacking = false;

    private void Awake()
    {
        if (_attackCollider != null)
        {
            _attackCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isAttacking && other.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.Health.TakeDamage(_damage);
        }
    }
    
    public void EnableAttack()
    {
        _isAttacking = true;
        
        if (_attackCollider != null)
        {
            _attackCollider.enabled = true;
        }
    }
    
    public void DisableAttack()
    {
        _isAttacking = false;
        
        if (_attackCollider != null)
        {
            _attackCollider.enabled = false;
        }
    }
}