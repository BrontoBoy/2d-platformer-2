using UnityEngine;

public class Potion : CollectableItem
{
    [SerializeField]private int healAmount = 25;
    
    public int HealAmount => healAmount;
}