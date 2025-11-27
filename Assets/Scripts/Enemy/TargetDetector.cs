using UnityEngine;
using System.Collections;
using System;

public class TargetDetector : MonoBehaviour
{
    [SerializeField] private float _range = 5f;
    [SerializeField] private float _rate = 0.1f;
    [SerializeField] private LayerMask _targetLayer;
    
    private Transform _targetTransform;
    private Coroutine _coroutine;
    private bool _isTargetDetected = false;
    private bool _isRunning = true;
    
    public event Action PlayerDetected;
    public event Action PlayerLost;
    
    public Transform TargetTransform => _targetTransform;
    public bool IsTargetDetected => _isTargetDetected;

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
        bool wasDetected = _isTargetDetected;
        
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _range, _targetLayer);
        
        _isTargetDetected = targets.Length > 0;
        
        if (targets.Length > 0)
        {
            _targetTransform = targets[0].transform;
            float sqrDistance = transform.position.SqrDistance(_targetTransform.position);
            float sqrRange = _range * _range;
            
            if (sqrDistance > sqrRange)
            {
                _isTargetDetected = false;
                _targetTransform = null;
            }
        }
        else
        {
            _targetTransform = null;
        }
        
        if (wasDetected == false && _isTargetDetected)
        {
            PlayerDetected?.Invoke();
        }
        else if (wasDetected && _isTargetDetected == false)
        {
            PlayerLost?.Invoke();
        }
    }
}