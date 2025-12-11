using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    [SerializeField] private Spell _spell;
    [SerializeField] private Image _barImage;
    
    private float _timer;
    
    private void Awake()
    {
        if (_barImage == null)
            _barImage = GetComponent<Image>();

        _barImage.type = Image.Type.Filled;
        _barImage.fillMethod = Image.FillMethod.Horizontal;
        _barImage.fillOrigin = 0;
    }
    
    private void Update()
    {
        if (_spell == null || _barImage == null) return;
        
        if (_spell.IsActive && _timer <= 0f)
        {
            StartCooldown();
        }
        
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            UpdateBarVisual();
        }
    }
    
    private void StartCooldown()
    {
        _timer = _spell.TotalCooldown;
        UpdateBarVisual();
    }
    
    private void UpdateBarVisual()
    {
        float progress = 1f - (_timer / _spell.TotalCooldown);
        _barImage.fillAmount = progress;
    }
}