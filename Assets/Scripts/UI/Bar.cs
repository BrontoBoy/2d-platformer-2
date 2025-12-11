using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Bar : MonoBehaviour
{
    [SerializeField] protected Image Image;
    [SerializeField] protected float AnimationDuration = 0.3f;
    [SerializeField] protected float Threshold = 0.01f;

    protected Coroutine _smoothCoroutine;
    protected float _targetFillAmount;
    protected float _startFillAmount;
    protected float _animationStartTime;

    protected virtual void Awake()
    {
        if (Image == null)
        {
            Image = GetComponent<Image>();
            
            if (Image == null)
            {
                return;
            }
        }
        
        SetupImage();
    }

    protected virtual void OnEnable()
    {
        Initialize();
    }
    
    protected virtual void OnDisable()
    {
        if (_smoothCoroutine != null)
        {
            StopCoroutine(_smoothCoroutine);
            _smoothCoroutine = null;
        }
    }
    
    protected virtual void SetupImage()
    {
        Image.type = Image.Type.Filled;
        Image.fillMethod = Image.FillMethod.Horizontal;
        Image.fillOrigin = 0;
    }
    
    protected abstract float CalculateFillAmount();
    
    protected virtual void UpdateFillAmount(float targetFillAmount)
    {
        _targetFillAmount = targetFillAmount;
        
        if (_smoothCoroutine != null)
            StopCoroutine(_smoothCoroutine);
        
        _smoothCoroutine = StartCoroutine(UpdateSmoothCoroutine(_targetFillAmount));
    }

    protected virtual IEnumerator UpdateSmoothCoroutine(float targetFillAmount)
    {
        _startFillAmount = Image.fillAmount;
        _animationStartTime = Time.time;
        
        while (Mathf.Abs(Image.fillAmount - _targetFillAmount) > Threshold)
        {
            float elapsedTime = Time.time - _animationStartTime;
            float progress = Mathf.Clamp01(elapsedTime / AnimationDuration);
            float newValue = Mathf.Lerp(_startFillAmount, _targetFillAmount, progress);
            Image.fillAmount = newValue;
            
            yield return null;
        }
        
        Image.fillAmount = _targetFillAmount;
        _smoothCoroutine = null;
    }
    
    public virtual void Initialize()
    {
        _targetFillAmount = CalculateFillAmount();
        Image.fillAmount = _targetFillAmount;
    }
}