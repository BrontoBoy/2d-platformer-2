using UnityEngine;

public class Patroller : MonoBehaviour
{
    private const float MinMoveDistance = 0.01f;
    
    [SerializeField] private float _reachThreshold = 0.1f;
    [SerializeField] private float _stuckThreshold = 2f; 
    [SerializeField] private Transform[] _waypoints;
    
    private int _currentWaypointIndex = 0;
    private float _stuckTimer = 0f;
    private Vector3 _lastPosition;
    private Mover _mover;
    
    public Transform CurrentWaypoint => _waypoints[_currentWaypointIndex];
    
    private void Awake() 
    {
        _lastPosition = transform.position;
        _mover = GetComponent<Mover>();
        
        if (_waypoints == null || _waypoints.Length == 0)
        {
            enabled = false;
            
            return;
        }
    }
    
    public void Patrol()
    {
        if (_mover == null)
            return;
        
        float direction = GetDirectionToWaypoint();
        _mover.Move(direction);
        float sqrDistance = transform.position.SqrDistance(CurrentWaypoint.position);
        float reachThresholdSqr = _reachThreshold * _reachThreshold;
        
        if (IsStuck())
        {
            SelectNextWaypoint();
            _stuckTimer = 0f;
        }
        
        if (sqrDistance <= reachThresholdSqr)
        {
            SelectNextWaypoint();
        }
        
        _lastPosition = transform.position;
    }
    
    public void SelectNextWaypoint()
    {
        _currentWaypointIndex = ++_currentWaypointIndex % _waypoints.Length;
        _stuckTimer = 0f;
    }
    
    private float GetDirectionToWaypoint()
    {
        Vector2 direction = CurrentWaypoint.position - transform.position;
        
        return Mathf.Sign(direction.x);
    }
    
    private bool IsStuck()
    {
        float distanceMoved = Mathf.Abs(transform.position.x - _lastPosition.x);
        
        if (distanceMoved < MinMoveDistance)
        {
            _stuckTimer += Time.deltaTime;
            
            return _stuckTimer >= _stuckThreshold;
        }
        else
        {
            _stuckTimer = 0f; 
            
            return false;
        }
    }
}