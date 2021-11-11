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

    [Header("Loading bar")]
    public GameObject progressBar;
    private float currentTime;
    public Image loadingBar;
    public TextMeshProUGUI durationText;


    public bool debugMode = false;
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

        loadingBar = progressBar.transform.GetChild(0).GetComponent<Image>();
    }

    public void SliderOnchange()//Called when slider value changed
    {
        UpdateSliderValues();
        UpdateTextCost();
    }

    private void UpdateSliderValues()//Calculates the pricing of every slider and sum it up
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

    private void UpdateTextCost()//Updates the cost text in the UI, if the player has enough balance left
    {
        Player cp = GameManager.instance.GetCurrentPlayer();
        if(cp.CheckBalance(cost))
        {
            textCost.color = new Color(255, 255, 255, 255);
        }
        else
        {
            textCost.color = new Color(1, 0, 0, 255);
        }
    }

    public void UpdateUI()//Update UI when a new player is selected
    {
        Player cp = GameManager.instance.GetCurrentPlayer();

        title.text = cp.Name;
        balance.text = cp.Balance.ToString();
    }
    
    public int GetTotalCost()//Return the total cost
    {
        return cost;
    }

    public List<Slider> GetValues()//Return list of sliders
    {
        return listSliders;
    }

    public void RandomizeSliders()//Randomize the config sliders
    {
        if(!debugMode)
        {
            for (int i = 0; i < listSliders.Count; i++)
            {
                listSliders[i].value = Random.Range(1, 100);
            }
        }
        else
        {
            for (int i = 0; i < listSliders.Count; i++)
            {
                listSliders[i].value = 100;
            }
        }
    }

    public IEnumerator ResetSetup()//Reset of the Config UI panel
    {
        yield return new WaitForSeconds(1f);
        selectionPanel.SetActive(true);
        textCost.color = new Color(255, 255, 255, 1);

        UpdateUI();
        RandomizeSliders();
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    private IEnumerator Timer(float duration)
    {
        progressBar.SetActive(true);
        currentTime = duration;
        while(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            loadingBar.fillAmount = currentTime / duration;
            durationText.text = currentTime.ToString("F0");
            yield return null;
        }
        progressBar.SetActive(false);
    }
}
