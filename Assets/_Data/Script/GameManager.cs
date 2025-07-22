using System;
using UnityEngine;

public class GameManager : NhoxBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] protected int currency = 500;
    public int Currency => currency;
    [SerializeField] protected int maxHP = 100;
    public int MaxHP => maxHP;
    [SerializeField] protected int currentHP;
    public int CurrentHP => currentHP;

    protected bool isInGame;
    public bool IsInGame => isInGame;
    
    public Action OnHPChanged;
    public Action OnCurrencyChanged;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            // DebugTool.LogError("Only one GameManager allowed to exist");
            return;
        }

        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        currentHP = maxHP;
        OnHPChanged?.Invoke();
        OnCurrencyChanged?.Invoke();
    }

    public void SetInGame(bool value) => isInGame = value;

    public void UpdateHP(int amount)
    {
        currentHP += amount;
        OnHPChanged?.Invoke();
        UI.Instance.InGameUI.ShakeHPUI();
    }
    
    public void UpdateCurrency(int amount)
    {
        currency += amount;
        OnCurrencyChanged?.Invoke();
    }

    public bool HasEnoughCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            OnCurrencyChanged?.Invoke();
            return true;
        }
        return false;
    }
}