using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : HealthView
{
    [SerializeField] private Image _image;
    [SerializeField] private float _smoothSpeed = 3f;
    [SerializeField] private float _threshold = 0.1f;

    private Coroutine _smoothCoroutine;
    private float _targetFillAmount;

    private void Awake()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
            if (_image == null)
            {
                return;
            }
        }
        
        SetupImage();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        if (_smoothCoroutine != null)
        {
            StopCoroutine(_smoothCoroutine);
            _smoothCoroutine = null;
        }
    }
    
    protected override void Initialize()
    {
        _targetFillAmount = CalculateFillAmount();
        _image.fillAmount = _targetFillAmount;
    }
    
    private void SetupImage()
    {
        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Horizontal;
        _image.fillOrigin = 0;
    }
    
    private float CalculateFillAmount()
    {
        if (Health == null || Health.MaxValue == 0)
            return 1f;
        
        return (float)Health.Value / Health.MaxValue;
    }
    
    protected override void OnHealthChanged(int amount)
    {
        _targetFillAmount = CalculateFillAmount();
        
        if (_smoothCoroutine != null)
            StopCoroutine(_smoothCoroutine);
        
        _smoothCoroutine = StartCoroutine(UpdateSmoothCoroutine(_targetFillAmount));
    }

    private IEnumerator UpdateSmoothCoroutine(float targetFillAmount)
    {
        while (Mathf.Abs(_image.fillAmount - _targetFillAmount) > _threshold)
        {
            float newValue = Mathf.MoveTowards(_image.fillAmount, _targetFillAmount,
                _smoothSpeed * Time.deltaTime);
            
            _image.fillAmount = newValue;
            
            yield return null;
        }
        
        _image.fillAmount = _targetFillAmount;
        _smoothCoroutine = null;
    }
}