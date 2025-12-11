using UnityEngine;
using System.Collections;

public abstract class Spell : MonoBehaviour
{
    [SerializeField] protected GameObject VisualEffectPrefab;
    [SerializeField] protected float Duration = 6f;
    [SerializeField] protected float Cooldown = 4f;
    [SerializeField] protected float RotationSpeed = 180f;
    
    private float _activeTimer;
    private float _cooldownTimer;
    private bool _isActive = false;
    private bool _onCooldown = false;
    private GameObject _currentEffect;
    private Coroutine _activeCoroutine;
    
    public bool IsActive => _isActive;
    public bool OnCooldown => _onCooldown;
    public float TotalCooldown => Duration + Cooldown;
    
    public bool TryActivate()
    {
        if (_isActive || _onCooldown)
            return false;
        
        Activate();
        return true;
    }
    
    public void Activate()
    {
        _isActive = true;
        _activeCoroutine = StartCoroutine(SpellRoutine());
    }
    
    public void Deactivate()
    {
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
            _activeCoroutine = null;
        }
        
        ForceDeactivate();
    }
    
    protected virtual void OnSpellStarted() { }
    
    protected virtual void OnSpellUpdated() { }
    
    protected virtual void OnSpellEnded() { }
    
    private void ForceDeactivate()
    {
        RemoveVisualEffect();
        _isActive = false;
        _onCooldown = false;
        OnSpellEnded();
    }
    
    private void CreateVisualEffect()
    {
        if (VisualEffectPrefab != null)
        {
            _currentEffect = Instantiate(VisualEffectPrefab, transform.position, Quaternion.identity, transform);
        }
    }
    
    private void RemoveVisualEffect()
    {
        if (_currentEffect != null)
        {
            Destroy(_currentEffect);
            _currentEffect = null;
        }
    }
    
    private void RotateVisualEffect()
    {
        if (_currentEffect != null && RotationSpeed > 0f)
        {
            _currentEffect.transform.Rotate(0, 0, 
                RotationSpeed * Time.deltaTime);
        }
    }
    
    private void EndSpell()
    {
        RemoveVisualEffect();
        _isActive = false;
        OnSpellEnded();
    }
    
    private IEnumerator CooldownRoutine()
    {
        _onCooldown = true;
        _cooldownTimer = Cooldown;
        
        while (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
            yield return null;
        }
        
        _cooldownTimer = 0f;
        _onCooldown = false;
    }
    
    private IEnumerator SpellRoutine()
    {
        CreateVisualEffect();
        OnSpellStarted();
        
        _activeTimer = Duration;
        
        while (_activeTimer > 0f)
        {
            _activeTimer -= Time.deltaTime;
            RotateVisualEffect();
            OnSpellUpdated();
            
            yield return null;
        }
        
        _activeTimer = 0f;
        EndSpell();
        
        if (Cooldown > 0f)
        {
            yield return StartCoroutine(CooldownRoutine());
        }
    }
}