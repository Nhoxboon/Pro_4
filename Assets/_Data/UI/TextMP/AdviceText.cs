using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AdviceText : BaseText
{
    [SerializeField] protected string[] adviceTexts;

    protected void OnEnable() => RandomAdvice();

    protected void RandomAdvice()
    {
        int randomIndex = Random.Range(0, adviceTexts.Length);
        textMeshPro.text = adviceTexts[randomIndex];
    }
}