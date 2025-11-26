using UnityEngine;

[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(AnimatorHandler))]
[RequireComponent(typeof(SpriteRotator))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Collector))] 
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private PlayerAnimationEvents _animationEvents;
    
    private InputReader _inputReader;
    private Mover _mover;
    private AnimatorHandler _animatorHandler;
    private SpriteRotator _spriteRotator;
    private Health _health;
    private Collector _collector;
    private int _coins;
    private bool _wasRunning = false;

    public Health Health => _health;
    public int Coins => _coins;
    public Collector Collector => _collector;
    
    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _mover = GetComponent<Mover>();
        _animatorHandler = GetComponent<AnimatorHandler>();
        _spriteRotator = GetComponent<SpriteRotator>();
        _health = GetComponent<Health>();
        _collector = GetComponent<Collector>();
        
        _collector.ItemCollected += OnItemCollected;
        _health.Died += OnDied;
    }
    
    
    private void OnEnable()
    {
        if (_animationEvents != null)
        {
            _animationEvents.AttackStarted += OnAttackStarted;
            _animationEvents.AttackEnded += OnAttackEnded;
        }
    }

    private void OnDisable()
    {
        if (_animationEvents != null)
        {
            _animationEvents.AttackStarted -= OnAttackStarted;
            _animationEvents.AttackEnded -= OnAttackEnded;
        }
    }
    
    private void FixedUpdate()
    {
        float direction = _inputReader.Direction;

        if (direction != 0 && _wasRunning == false)
        {
            _animatorHandler.PlayRun();
            _wasRunning = true;
        }
        else if (direction == 0 && _wasRunning)
        {
            _animatorHandler.StopRun();
            _wasRunning = false;
        }

        if (direction != 0)
        {
            _mover.Move(direction);
            _spriteRotator.TryRotateTowards(direction); 
        }

        if (_inputReader.IsJump && _groundDetector.IsGround)
        {
            _mover.Jump();
            _animatorHandler.PlayJump();
            _inputReader.ResetJump();
        }
        
        if (_inputReader.IsAttack)
        {
            _animatorHandler.PlayAttack();
            _inputReader.ResetAttack();
        }
        
        _animatorHandler.SetGrounded(_groundDetector.IsGround);
    }
    
    private void OnDestroy()
    {
        if (_health != null)
        {
            _collector.ItemCollected -= OnItemCollected;
            _health.Died -= OnDied;
        }
    }
    
    private void OnDied()
    {
        this.enabled = false; 
    }
    
        
    private void OnItemCollected(ICollectable collectedItem)
    {
        if (collectedItem is Potion potion)
        {
            _health.Heal(potion.HealAmount);
        }
        
        if (collectedItem is Coin coin)
        {
            _coins++;
        }
    }
    
    private void OnAttackStarted()
    {
        _playerAttack?.Enable();
    }
    
    private void OnAttackEnded()
    {
        _playerAttack?.Disable();
    }
}