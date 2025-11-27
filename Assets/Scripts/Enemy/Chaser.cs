using UnityEngine;

public class Chaser : MonoBehaviour 
{
    private Mover _mover;
    
    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }
    
    public void Chase(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        _mover.Move(Mathf.Sign(direction.x));
    }
    
    public void Stop()
    {
        _mover.Move(Mover.StopDirection);
    }
}