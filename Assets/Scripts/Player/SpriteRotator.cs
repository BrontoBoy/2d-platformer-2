using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    private const float RightAngle = 0f;
    private const float LeftAngle = 180f;
    
    private float _currentDirection = 1f;
    private Quaternion _rightRotation;
    private Quaternion _leftRotation;
    
    private void Awake()
    {
        _rightRotation = Quaternion.Euler(0f, RightAngle, 0f);
        _leftRotation = Quaternion.Euler(0f, LeftAngle, 0f);
    }
    
    private bool IsNeedRotation(float movementDirectionSign)
    {
        return movementDirectionSign != _currentDirection;
    }
    
    private void ApplyRotation(float directionSign)
    {
        _currentDirection = directionSign;
        transform.rotation = directionSign > 0 ? _rightRotation : _leftRotation;
    }
    
    public void TryRotateTowards(float movementDirection)
    {
        if (movementDirection == 0)
        {
            return;
        }
        
        float movementSign = Mathf.Sign(movementDirection);
        
        if (IsNeedRotation(movementSign))
        {
            ApplyRotation(movementSign);
        }
    }
}