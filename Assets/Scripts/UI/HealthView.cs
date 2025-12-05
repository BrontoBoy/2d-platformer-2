using UnityEngine;

public abstract class HealthView : MonoBehaviour
{
    [SerializeField] protected Health Health;
    
    private void Start()
    {
        Initialize();
    }
    
    protected virtual void OnEnable()
    {
        if (Health != null)
        {
            Health.DamageTaken += OnHealthChanged;
            Health.Healed += OnHealthChanged;
        }
    }
    
    protected virtual void OnDisable()
    {
        if (Health != null)
        {
            Health.DamageTaken -= OnHealthChanged;
            Health.Healed -= OnHealthChanged;
        }
    }
    
    protected virtual void OnHealthChanged(int amount) { }
    
    protected virtual void Initialize() { }
}