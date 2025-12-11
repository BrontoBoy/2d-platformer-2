using UnityEngine;

public class Vampirism : Spell
{
    [SerializeField] private float _damagePerTick = 10f;
    [SerializeField] private float _healingPerTick = 5f;
    [SerializeField] private float _tick = 0.5f;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private LayerMask _targetLayer;
    
    private Health _casterHealth;
    private Health _targetHealth;
    private Transform _currentTarget; 
    private float _nextTickTime;
    private Collider2D[] _targets = new Collider2D[100];
    
    protected override void OnSpellStarted()
    {
        _casterHealth = GetComponentInParent<Health>();
        _currentTarget = null;
        _targetHealth = null;
        _nextTickTime = Time.time + _tick;
    }
    
    protected override void OnSpellUpdated()
    {
        if (Time.time >= _nextTickTime)
        {
            FindNearestTarget();
            
            if (_currentTarget != null && _targetHealth != null)
            {
                ApplyVampirismEffect();
            }
            
            _nextTickTime = Time.time + _tick;
        }
    }
    
    protected override void OnSpellEnded()
    {
        _currentTarget = null;
        _targetHealth = null;
    }
    
    private void FindNearestTarget()
    {
        int targetCount = Physics2D.OverlapCircleNonAlloc(transform.position, 
            _radius, _targets, _targetLayer);
        
        if (targetCount == 0)
        {
            _currentTarget = null;
            _targetHealth = null;
            
            return;
        }
        
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;
        
        for (int i = 0; i < targetCount; i++)
        {
            if (_targets[i] != null)
            {
                GameObject target = _targets[i].gameObject;
                
                var health = target.GetComponent<Health>();
                
                if (health == null) 
                    continue;
                
                if (health.IsAlive == false) 
                    continue;
                
                float distance = Vector2.Distance(transform.position,
                    target.transform.position);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = _targets[i].transform;
                }
            }
        }
        
        if (closestTarget != _currentTarget)
        {
            _currentTarget = closestTarget;
            _targetHealth = _currentTarget?.GetComponent<Health>();
        }
    }
    
    private void ApplyVampirismEffect()
    {
        if (_targetHealth == null || !_targetHealth.IsAlive)
        {
            _currentTarget = null;
            _targetHealth = null;
            
            return;
        }
        
        if (_damagePerTick > 0)
        {
            int damageToApply = Mathf.RoundToInt(_damagePerTick);
            
            if (damageToApply > 0)
            {
                _targetHealth.TakeDamage(damageToApply);
            }
        }
        
        if (_healingPerTick > 0 && _casterHealth != null)
        {
            int healingToApply = Mathf.RoundToInt(_healingPerTick);
            
            if (healingToApply > 0)
            {
                _casterHealth.Heal(healingToApply);
            }
        }
    }
}