using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _textHitPointsValueName;
    [SerializeField] private TMP_Text _textCoinsValueName;
    
    private Player _player;
    private string _hitPointsValueName = "HP";
    private string _coinsValueName = "Coins";
    
    public Collector Collector => _player.Collector;

    private void Start()
    {
        _player = GetComponent<Player>();
        Collector.ItemCollected += OnItemCollected;
        _player.Health.DamageTaken += OnDamageTaken;
        
        UpdatePlayerUI(_textHitPointsValueName, _hitPointsValueName, _player.Health.Value);
        UpdatePlayerUI(_textCoinsValueName, _coinsValueName, _player.Coins);
    }
    
    private void OnDisable()
    {
        Collector.ItemCollected -= OnItemCollected;
        _player.Health.DamageTaken -= OnDamageTaken;
    }
    
    private void OnItemCollected(ICollectable collectedItem)
    {
        if (collectedItem is Potion)
        {
            UpdatePlayerUI(_textHitPointsValueName, _hitPointsValueName, _player.Health.Value);
        }
        
        if (collectedItem is Coin)
        {
            UpdatePlayerUI(_textCoinsValueName, _coinsValueName, _player.Coins);
        }
    }

    private void OnDamageTaken(int amount)
    {
        UpdatePlayerUI(_textHitPointsValueName, _hitPointsValueName, _player.Health.Value);
    }

    private void UpdatePlayerUI(TMP_Text text, string name, int value)
    {
        text.text = $"{name}: {value}";
    }
}