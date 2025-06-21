using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stat
{
    public event Action OnCurrentValueZero;
    public event Action OnValueDecreased;

    [SerializeField] protected float maxValue;
    public float MaxValue => maxValue;
    [SerializeField] protected float currentValue;

    public float CurrentValue
    {
        get => currentValue;
        protected set
        {
            currentValue = Mathf.Clamp(value, 0f, maxValue);

            if (currentValue <= 0)
            {
                OnCurrentValueZero?.Invoke();
            }
        }
    }

    public bool IsInvincible { get; set; } = false;

    public void SetMaxValue(float maxValueData)
    {
        this.maxValue = maxValueData;
    }

    public void SetInvincible(bool isInvincible)
    {
        IsInvincible = isInvincible;
    }

    public void Init() => CurrentValue = maxValue;

    public void Increase(float amount) => CurrentValue += amount;

    public void Decrease(float amount)
    {
        if (IsInvincible || amount <= 0f || currentValue <= 0f) return;

        float oldValue = currentValue;
        CurrentValue -= amount;

        if (currentValue < oldValue)
            OnValueDecreased?.Invoke();
    }
}