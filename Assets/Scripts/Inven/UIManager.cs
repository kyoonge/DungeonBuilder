using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public int Cost;
    public int HP = 100;
    

    public TMP_Text CostText;
    public TMP_Text HPText;
    public TMP_Text WarningText;

    public GameObject startButton;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void InitHP()
    {
        
        HP = 100;
        HPText.text = $"HP: {HP}";
    }

    public void setActiveStartButton()
    {
        if (GridManager.Instance.mode.isOn)
        {
            startButton.SetActive(false);
        }
        else
        {
            GridManager.Instance.StartInvenMode();
            startButton.SetActive(true);
        }
    }

    // Update is called once per frame
    public void IncreaseCost(int value)
    {
        Cost += value;
        CostText.text = $"Cost: {Cost}";
    }
    public void IncreaseHP(int value)
    {
        HP += value;

        if (HP > 100)
        {
            HP = 100;
        }
        HPText.text = $"HP: {HP}";
    }

    public void DecreaseCost(int value)
    {
        Cost = Cost - value;
        CostText.text = $"Cost: {Cost}";
    }
    public void DecreaseHP(int value)
    {
        Debug.Log("Demage:"+value);
        HP = HP - value;
        HPText.text = $"HP: {HP}";
    }

    public void PrintWarning()
    {
        // 활성화
        WarningText.gameObject.SetActive(true);

        // 3초 후에 비활성화
        StartCoroutine(DisableTextAfterDelay(1.5f));
    }

    private IEnumerator DisableTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        WarningText.gameObject.SetActive(false);
    }
}
