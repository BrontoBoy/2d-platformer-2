using UnityEngine;
using System.Collections;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private float _range = 5f;
    [SerializeField] private float _rate = 0.1f;
    [SerializeField] private LayerMask _targetLayer;
    
    private Transform _player;
    private Coroutine _coroutine;
    private bool _isPlayerDetected = false;
    private bool _isRunning = true;
    
    public Transform Player => _player;
    public bool IsPlayerDetected => _isPlayerDetected;

    private void OnEnable()
    {
        StartDetection();
    }

    private void OnDisable()
    {
        StopDetection();
    }

    private void StartDetection()
    {
        _isRunning = true;
        _coroutine = StartCoroutine(DetectionRoutine());
    }

    private void StopDetection()
    {
        _isRunning = false;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private IEnumerator DetectionRoutine()
    {
        while (_isRunning)
        {
            Detection();
            
            yield return new WaitForSeconds(_rate);
        }
    }

    private void Detection()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _range, _targetLayer);
        
        _isPlayerDetected = targets.Length > 0;
        
        if (targets.Length > 0)
        {
            _player = targets[0].transform;
        }
        else
        {
            _player = null;
        }
    }
}