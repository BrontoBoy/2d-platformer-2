using UnityEngine;

[RequireComponent(typeof(Patroller))]        
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerDetector))] 
[RequireComponent(typeof(Chaser))]
[RequireComponent(typeof(Mover))]
public class Enemy : MonoBehaviour
{
    private Health _health;
    private Patroller _patroller;           
    private PlayerDetector _playerDetector;
    private Chaser _chaser;
    
    public Health Health => _health;
    
    private void Awake()
    {
        _health = GetComponent<Health>();
        _patroller = GetComponent<Patroller>();
        _playerDetector = GetComponent<PlayerDetector>();
        _chaser = GetComponent<Chaser>();
        
        _chaser.Initialize(_playerDetector);
    }
    
    private void Update()
    {
        if (_playerDetector.IsPlayerDetected)
        {
            _chaser.Chase();
        }
        else
        {
            _chaser.Stop();
            _patroller.Patrol();
        }
    }
}