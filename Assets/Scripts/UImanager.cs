using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UImanager : MonoBehaviour
{

    public static UImanager instance { get; set; }

    public TextMeshProUGUI title;
    public TextMeshProUGUI balance;

    [Header("Slider Values")]
    public List<TextMeshProUGUI> listValues;

    [Header("Sliders")]
    public List<Slider> listSliders;

    public GameObject selectionPanel;

    public TextMeshProUGUI textCost;
    private int cost;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //UpdateSliderValues();
    }

    public void SliderOnchange()
    {
        UpdateSliderValues();
        UpdateText();
    }

    private void UpdateSliderValues()
    {
        int value = 0;
        for (int i = 0; i < listSliders.Count; i++)
        {
            listValues[i].text = listSliders[i].value.ToString();

            if(i == 0 || i == 2)
            {
                value += (int)map(listSliders[i].value, 1, 100, 3, 30);
            }
            else
            {
                value += (int)map(listSliders[i].value, 1, 100, 2, 20);
            }
            
        }
        cost = value;
        textCost.text = cost.ToString();
    }

    private void UpdateText()
    {
        switch (BattleSystem.instance.battleState)
        {
            case BattleState.REDSETUP:
                if (!BattleSystem.instance.redPlayer.CheckBalance(cost))
                {
                    textCost.color = new Color(1, 0, 0, 255);
                }
                else
                {
                    textCost.color = new Color(255, 255, 255, 255);
                }
                break;
            case BattleState.BLUESETUP:
                if (!BattleSystem.instance.bluePlayer.CheckBalance(cost))
                {
                    textCost.color = new Color(1, 0, 0, 255);
                }
                else
                {
                    textCost.color = new Color(255, 255, 255, 255);
                }
                break;
        }
    }

    public int GetTotalCost()
    {
        return cost;
    }

    public List<Slider> GetValues()
    {
        return listSliders;
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
