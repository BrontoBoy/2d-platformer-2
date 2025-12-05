using UnityEngine;

public class Health : MonoBehaviour, IDamageble, IHealable
{
    [SerializeField] private int _maxValue = 100;
    private int _currentValue;
    
    public event System.Action<int> DamageTaken;
    public event System.Action<int> Healed;
    public event System.Action Died;
    
    public int MaxValue => _maxValue;
    public int Value => _currentValue;
    public bool IsAlive => _currentValue > 0;

    private void Awake()
    {
        _currentValue = _maxValue;
    }
    
    public void Heal(int amount)
    {
        if (amount <= 0)
            return;
        
        if (IsAlive == false)
            return;
        
        int oldHealth = _currentValue;
        _currentValue = Mathf.Clamp(_currentValue + amount, 0, _maxValue);
        int actualHeal = _currentValue - oldHealth;
        
        Healed?.Invoke(actualHeal);
    }
    
    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            return;
        
        if (IsAlive == false)
            return;
        
        int oldHealth = _currentValue;
        _currentValue = Mathf.Clamp(_currentValue - damage, 0, _maxValue);
        int actualDamage = oldHealth - _currentValue;
        
        DamageTaken?.Invoke(actualDamage);
        
        if (_currentValue <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        Died?.Invoke();
        gameObject.SetActive(false);
    }
}