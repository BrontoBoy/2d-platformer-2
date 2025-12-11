using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private Spell _currentSpell;
    
    public bool TryCastSpell()
    {
        if (_currentSpell == null)
        {
            return false;
        }
        
        return _currentSpell.TryActivate();
    }
    
    public void SetSpell(Spell spell)
    {
        if (_currentSpell != null && _currentSpell.IsActive)
        {
            _currentSpell.Deactivate();
        }
        
        _currentSpell = spell;
    }
}