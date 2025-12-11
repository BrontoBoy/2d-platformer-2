using UnityEngine;

[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Collector))] 
[RequireComponent(typeof(Attacker))]
[RequireComponent(typeof(Wallet))]
public class Player : MonoBehaviour
{
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private Transform _sprite;
    [SerializeField] private Transform _attackPoint;
    
    private AnimatorHandler _animatorHandler;
    private PlayerAnimationEvents _animationEvents;
    private InputReader _inputReader;
    private Mover _mover;
    private SpriteRotator _spriteRotator;
    private Health _health;
    private Collector _collector;
    private Attacker _attacker; 
    private Wallet _wallet; 
    private bool _wasRunning = false;
    private SpellCaster _spellCaster;
    private Vampirism _vampirismSpell;

    public Health Health => _health;
    public Wallet Wallet => _wallet;
    
    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _mover = GetComponent<Mover>();
        _health = GetComponent<Health>();
        _collector = GetComponent<Collector>();
        _attacker = GetComponent<Attacker>();
        _wallet = GetComponent<Wallet>();
        _spellCaster = GetComponent<SpellCaster>();
        _vampirismSpell = GetComponent<Vampirism>();
        
        if (_spellCaster != null && _vampirismSpell != null)
        {
            _spellCaster.SetSpell(_vampirismSpell);
        }
        
        if (_sprite != null )
        {
            _animatorHandler = _sprite.GetComponent<AnimatorHandler>();
            _spriteRotator = _sprite.GetComponent<SpriteRotator>();
            _animationEvents = _sprite.GetComponent<PlayerAnimationEvents>();
        }
        
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
    
    private void Update()
    {
        HandleSpell();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
        
        float direction = _inputReader.Direction;

        if (direction != 0 && _wasRunning == false)
        {
            if (_animatorHandler != null)
                _animatorHandler.PlayRun();
            
            _wasRunning = true;
        }
        else if (direction == 0 && _wasRunning)
        {
            if (_animatorHandler != null)
                _animatorHandler.StopRun();
            
            _wasRunning = false;
        }

        if (direction != 0)
        {
            _mover.Move(direction);
            
            if (_spriteRotator != null)
                _spriteRotator.TryRotateTowards(direction); 
        }

        if (_inputReader.IsJump && _groundDetector.IsGround)
        {
            _mover.Jump();
            
            if (_animatorHandler != null)
                _animatorHandler.PlayJump();
            
            _inputReader.ResetJump();
        }
        
        if (_inputReader.IsAttack)
        {
            if (_animatorHandler != null)
                _animatorHandler.PlayAttack();
            
            _inputReader.ResetAttack();
        }
        
        if (_animatorHandler != null)
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
    
    private void HandleMovement()
    {
        float direction = _inputReader.Direction;
        
        if (direction != 0 && _wasRunning == false)
        {
            if (_animatorHandler != null)
                _animatorHandler.PlayRun();
            
            _wasRunning = true;
        }
        else if (direction == 0 && _wasRunning)
        {
            if (_animatorHandler != null)
                _animatorHandler.StopRun();
            
            _wasRunning = false;
        }
        
        if (direction != 0)
        {
            _mover.Move(direction);
            
            if (_spriteRotator != null)
                _spriteRotator.TryRotateTowards(direction);
        }
        
        if (_inputReader.IsJump && _groundDetector.IsGround)
        {
            _mover.Jump();
            
            if (_animatorHandler != null)
                _animatorHandler.PlayJump();
            
            _inputReader.ResetJump();
        }
        
        if (_inputReader.IsAttack)
        {
            if (_animatorHandler != null)
                _animatorHandler.PlayAttack();
            
            _inputReader.ResetAttack();
        }
        
        if (_animatorHandler != null)
            _animatorHandler.SetGrounded(_groundDetector.IsGround);
    }
    
    
    private void HandleSpell()
    {
        if (_inputReader.IsCastSpell)
        {
            if (_spellCaster != null)
            {
                _spellCaster.TryCastSpell();
            }
            
            _inputReader.ResetSpellCast();
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
        
        if (collectedItem is Coin)
        {
            _wallet.AddCoins();
        }
    }
    
    private void OnAttackStarted()
    {
        Vector2 attackPosition = GetAttackPosition();
        _attacker.Attack(attackPosition);
    }
    
    private void OnAttackEnded()
    {
    }
    
    private Vector2 GetAttackPosition()
    {
        if (_attackPoint != null)
        {
            return _attackPoint.position;
        }
        
        float facingDirection = _spriteRotator.GetFacingDirection();
        Vector2 offset = new Vector2(1f, 0.5f) * facingDirection;
        
        return (Vector2)transform.position + offset;
    }
}