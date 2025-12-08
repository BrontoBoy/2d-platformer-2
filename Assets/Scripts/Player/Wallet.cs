using System;
using UnityEngine;


public class Wallet : MonoBehaviour
{
    [SerializeField] private int _coinsValue = 0;
    
    public event Action<int> CoinsChanged;
    
    public int CoinsValue => _coinsValue;
    
    public void AddCoins()
    {
        _coinsValue++;
        CoinsChanged?.Invoke(_coinsValue);
    }
}