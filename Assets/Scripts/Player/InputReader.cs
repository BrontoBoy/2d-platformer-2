using UnityEngine;

public class InputReader : MonoBehaviour
{
    public const string Horizontal = "Horizontal";
    public const int AttackMouseButtonIndex = 0;
    
    private bool _isJump;
    private bool _isAttack;
    
    public float Direction { get; private set; }
    public bool IsJump => _isJump;
    public bool IsAttack => _isAttack;
    
    private void Update()
    {
        Direction = Input.GetAxis(Horizontal);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJump = true;
        }
        
        if (Input.GetMouseButtonDown(AttackMouseButtonIndex))
        {
            _isAttack = true;
        }
    }
    
    public void ResetJump()
    {
        _isJump = false;
    }
    
    public void ResetAttack()
    {
        _isAttack = false;
    }
}