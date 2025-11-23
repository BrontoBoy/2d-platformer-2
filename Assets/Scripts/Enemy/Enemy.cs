using UnityEngine;

[RequireComponent(typeof(EnemyPatrol))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyAI))] 
[RequireComponent(typeof(Mover))]
public class Enemy : MonoBehaviour
{
    private Health _health;
    private EnemyPatrol _patrol;
    private EnemyAI _aI;
    private Mover _mover;
    
    public Health Health => _health;
    
    private void Awake()
    {
        _health = GetComponent<Health>();
        _patrol = GetComponent<EnemyPatrol>();
        _aI = GetComponent<EnemyAI>();
        _mover = GetComponent<Mover>();
    }
    
    private void Update()
    {
        if (_aI.IsPlayerDetected)
        {
            float direction = _aI.GetDirectionToPlayer();
            _mover.Move(direction);
        }
        else
        {
            _patrol.Patrol();
        }
    }
}