using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public int Cost;
    public int Power;

    public TMP_Text CostText;
    public TMP_Text PowerText;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    public void IncreaseCost(int value)
    {
        Cost += value;
        CostText.text = $"Cost: {Cost}";
    }
    public void IncreasePower(int value)
    {
        Power += value;
        //PowerText.text = $"공격력: {Cost}";
    }

    public void DecreaseCost(int value)
    {
        Cost -= value;
        CostText.text = $"Cost: {Cost}";
    }
    public void DecreasePower(int value)
    {
        Power -= value;
        //PowerText.text = $"공격력: {Cost}";
    }
}
