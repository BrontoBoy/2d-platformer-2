using UnityEngine;

[RequireComponent(typeof(Patroller))]        
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(TargetDetector))] 
[RequireComponent(typeof(Chaser))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Attacker))]
public class Enemy : MonoBehaviour
{
    private Health _health;
    private Patroller _patroller;           
    private TargetDetector _targetDetector;
    private Chaser _chaser;
    private Attacker _attacker; 
    
    public Health Health => _health;
    
    private void Awake()
    {
        _health = GetComponent<Health>();
        _patroller = GetComponent<Patroller>();
        _targetDetector = GetComponent<TargetDetector>();
        _chaser = GetComponent<Chaser>();
        _attacker = GetComponent<Attacker>();
    }

    private void OnEnable()
    {
        _targetDetector.PlayerDetected += OnTargetDetected;
        _targetDetector.PlayerLost += OnTargetLost;
    }

    private void OnDisable()
    {
        _targetDetector.PlayerDetected -= OnTargetDetected;
        _targetDetector.PlayerLost -= OnTargetLost;
    }

    private void Update()
    {
        if (CanAttack())
        {
            Attack();  
        }
        else if (_targetDetector.IsTargetDetected && _targetDetector.TargetTransform != null)
        {
            _chaser.Chase(_targetDetector.TargetTransform.position);
        }
        else
        {
            _patroller.Patrol();
        }
    }

    private bool CanAttack()
    {
        return _targetDetector.IsTargetDetected && _targetDetector.TargetTransform != null
                                                && _attacker.CanAttack(_targetDetector.TargetTransform.position)
                                                && _attacker.IsReadyToAttack();
    }

    private void Attack()
    {
        _chaser.Stop();
        
        Vector2 direction = (_targetDetector.TargetTransform.position - transform.position).normalized;
        Vector2 position = (Vector2)transform.position + direction * 0.5f;
        
        _attacker.Attack(position);
    }
    
    private void OnTargetDetected()
    {
    }

    private void OnTargetLost()
    {
        _chaser.Stop();
    }
}