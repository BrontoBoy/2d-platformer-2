using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _rate = 0.2f;
    [SerializeField] private float _range = 1f;
    [SerializeField] private LayerMask _targetLayer;

    private bool _isAttacking = false;
    private Coroutine _сoroutine;

    private void OnEnable()
    {
        StartAttack();
    }

    private void OnDisable()
    {
        StopAttack();
    }

    private void StartAttack()
    {
        _isAttacking = true;
        _сoroutine = StartCoroutine(AttackRoutine());
    }

    private void StopAttack()
    {
        _isAttacking = false;

        if (_сoroutine != null)
        {
            StopCoroutine(_сoroutine);
            _сoroutine = null;
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (_isAttacking)
        {
            ApplyDamage();

            yield return new WaitForSeconds(_rate);
        }
    }

    private void ApplyDamage()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _range, _targetLayer);

        foreach (Collider2D targetCollider in targets)
        {
            if (targetCollider.TryGetComponent(out Player player))
            {
                player.Health.TakeDamage(_damage);
            }
        }
    }
}