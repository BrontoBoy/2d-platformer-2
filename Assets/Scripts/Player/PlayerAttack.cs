using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 50;
    [SerializeField] private float _range = 1.5f;
    [SerializeField] private Vector2 _offset = new Vector2(0f, 1f);
    [SerializeField] private LayerMask _targetLayer;

    private bool _isAttacking = false;
    
    private void ApplyDamage()
    {
        if (_isAttacking == false)
            return;
        
        Vector2 attackPosition = GetAttackPosition();

        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPosition, _range, _targetLayer);

        foreach (Collider2D targetCollider in targets)
        {
            if (targetCollider.TryGetComponent(out Enemy enemy))
            {
                enemy.Health.TakeDamage(_damage);
            }
        }
    }

    private Vector2 GetAttackPosition()
    {
        float facingDirection = GetFacingDirection();
        Vector2 rotatedOffset = _offset;
        rotatedOffset.x *= facingDirection; 
        
        return (Vector2)transform.position + rotatedOffset;
    }
    
    private float GetFacingDirection()
    {
        if (transform.rotation.eulerAngles.y == 0f)
        {
            return 1f;
        }
        else if (transform.rotation.eulerAngles.y == 180f)
        {
            return -1f;
        }
        else
        {
            return 1f;
        }
    }
    
    public void Enable()
    {
        _isAttacking = true;
        ApplyDamage();
    }

    public void Disable()
    {
        _isAttacking = false;
    }
}