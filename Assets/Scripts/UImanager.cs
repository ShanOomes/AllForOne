using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UImanager : MonoBehaviour
{

    public static UImanager instance { get; set; }

    public TextMeshProUGUI balanceBlue;
    public TextMeshProUGUI balanceRed;

    public GameObject playerBlue;
    public GameObject playerRed;

    public TMP_InputField input;

    [Header("Slider Values")]
    public List<TextMeshProUGUI> listValues;

    [Header("Sliders")]
    public List<Slider> listSliders;

    private float[] stats = { 0f, 0f, 0f, 0f };

    public GameObject selectionPanel;

    public TextMeshProUGUI textCost;
    private float cost;
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
        for (int i = 0; i < listValues.Count; i++)
        {
            listValues[i].text = "0";
        }
    }

    public void SliderOnchange()
    {
        for (int i = 0; i < listSliders.Count; i++)
        {
            listValues[i].text = listSliders[i].value.ToString();
            stats[i] = listSliders[i].value;
        }

        cost = stats[0] + stats[1] + stats[2] + stats[3];

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
            default:
                break;
        }
        textCost.text = cost.ToString();
    }

    public float GetCost()
    {
        return cost;
    }

    public float[] GetValues()
    {
        return stats;
    }

    public void highlight(string player)
    {
        if(player == "blue")
        {
            playerBlue.SetActive(true);
            playerRed.SetActive(false);
        }

        if(player == "red")
        {
            playerRed.SetActive(true);
            playerBlue.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
