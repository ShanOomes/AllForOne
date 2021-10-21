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

    public TextMeshProUGUI title;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
