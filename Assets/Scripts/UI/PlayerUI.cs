using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _textHitPointsValueName;
    [SerializeField] private TMP_Text _textCoinsValueName;
    [SerializeField] private string _hitPointsValueName = "HP";
    [SerializeField] private string _coinsValueName = "Coins";
    
    private Player _player;
    
    private void Start()
    {
        _player = GetComponent<Player>();
        
        _player.Health.DamageTaken += OnDamageTaken;
        _player.Health.Healed += OnHealed;
        _player.Wallet.CoinsChanged += OnCoinsChanged;  
        
        UpdatePlayerUI(_textHitPointsValueName, _hitPointsValueName, _player.Health.Value);
        UpdatePlayerUI(_textCoinsValueName, _coinsValueName, _player.Wallet.CoinsValue);
    }
    
    private void OnDisable()
    {
        if (_player != null && _player.Health != null)
        {
            _player.Health.DamageTaken -= OnDamageTaken;
            _player.Health.Healed -= OnHealed;
        }
        
        if (_player != null && _player.Wallet != null)
        {
            _player.Wallet.CoinsChanged -= OnCoinsChanged;
        }
    }
    
    private void OnDamageTaken(int amount)
    {
        UpdatePlayerUI(_textHitPointsValueName, _hitPointsValueName, _player.Health.Value);
    }
    
    private void OnHealed(int amount)
    {
        UpdatePlayerUI(_textHitPointsValueName, _hitPointsValueName, _player.Health.Value);
    }
    
    private void OnCoinsChanged(int coins)
    {
        UpdatePlayerUI(_textCoinsValueName, _coinsValueName, coins);
    }
    
    private void UpdatePlayerUI(TMP_Text text, string name, int value)
    {
        text.text = $"{name}: {value}";
    }
}