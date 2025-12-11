using UnityEngine;

public class HealthBar : Bar
{
    [SerializeField] private Health _health;
    
    protected override void Awake()
    {
        base.Awake();
        
        if (_health == null)
            _health = GetComponentInParent<Health>();
    }
    
    private void Start()
    {
        UpdateFillAmount(CalculateFillAmount());
    }
    
    protected override void OnEnable()
    {
        SubscribeToHealthEvents();
        base.OnEnable();
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        UnsubscribeFromHealthEvents();
    }
    
    protected override float CalculateFillAmount()
    {
        if (_health == null || _health.MaxValue == 0)
            return 1f;
        
        return Mathf.Clamp01((float)_health.Value / _health.MaxValue);
    }
    
    private void SubscribeToHealthEvents()
    {
        if (_health != null)
        {
            _health.DamageTaken += OnHealthChanged;
            _health.Healed += OnHealthChanged;
        }
    }
    
    private void UnsubscribeFromHealthEvents()
    {
        if (_health != null)
        {
            _health.DamageTaken -= OnHealthChanged;
            _health.Healed -= OnHealthChanged;
        }
    }
    
    private void OnHealthChanged(int amount)
    {
        UpdateFillAmount(CalculateFillAmount());
    }
}