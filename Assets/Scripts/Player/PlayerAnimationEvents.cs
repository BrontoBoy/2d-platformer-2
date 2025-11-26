using UnityEngine;
using System;

public class PlayerAnimationEvents : MonoBehaviour
{
    public event Action AttackStarted;
    public event Action AttackEnded;
    
    public void InvokeAttackStarted() => AttackStarted?.Invoke();
    public void InvokeAttackEnded() => AttackEnded?.Invoke();
}