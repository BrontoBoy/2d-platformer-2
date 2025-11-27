using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _range = 1f;
    [SerializeField] private float _сooldown = 1f;
    [SerializeField] private LayerMask _targetLayer;
    
    private float _lastAttackTime;
    
    public void Attack(Vector2 attackPosition)
    {
        if (Time.time - _lastAttackTime < _сooldown)
        {
            return;
        }
        
        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPosition, _range, _targetLayer);
        
        foreach (Collider2D targetCollider in targets)
        {
            if (targetCollider.TryGetComponent(out Health health))
            {
                health.TakeDamage(_damage);
            }
        }
        
        _lastAttackTime = Time.time;
    }
    
    public bool CanAttack(Vector2 targetPosition)
    {
        float distance = Vector2.Distance(transform.position, targetPosition);
        return distance <= _range;
    }
    
    public bool IsReadyToAttack()
    {
        return Time.time - _lastAttackTime >= _сooldown;
    }
}