using UnityEngine;

public class Health : MonoBehaviour, IDamageble, IHealable
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth;
    
    public int Value => _currentHealth;
    public bool IsAlive => _currentHealth > 0;
    
    public event System.Action<int> DamageTaken;
    public event System.Action<int> Healed;
    public event System.Action Died;
    
    private void Die()
    {
        Died?.Invoke();
        gameObject.SetActive(false);
    }
    
    public void Heal(int amount)
    {
        if (IsAlive == false)
        {
            return;
        }
        
        int oldHealth = _currentHealth;
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        int actualHeal = _currentHealth - oldHealth;
        
        Healed?.Invoke(actualHeal);
    }
    
    public void TakeDamage(int damage)
    {
        if (IsAlive == false)
        {
            return;
        }
        
        _currentHealth -= damage;
        DamageTaken?.Invoke(damage);
        
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
}