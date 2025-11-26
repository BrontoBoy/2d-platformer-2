using UnityEngine;

public class Chaser : MonoBehaviour 
{
    private Mover _mover;
    private PlayerDetector _detector;
    
    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }
    
    public void Initialize(PlayerDetector detector)
    {
        _detector = detector;
    }
    
    public void Chase()
    {
        if (_detector == null || _detector.Player == null) return;
        
        Vector2 direction = _detector.Player.position - transform.position;
        _mover.Move(Mathf.Sign(direction.x));
    }
    
    public void Stop()
    {
        _mover.Move(Mover.StopDirection);
    }
}