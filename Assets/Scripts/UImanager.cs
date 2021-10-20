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

    [Header("Slider Values")]
    public TextMeshProUGUI healthValueText;
    public TextMeshProUGUI strengthValueText;
    public TextMeshProUGUI speedValueText;
    public TextMeshProUGUI DefenseValueText;

    [Header("Sliders")]
    public Slider healthSlider;
    public Slider strengthSlider;
    public Slider speedSlider;
    public Slider defenseSlider;

    private float health;
    private float strength;
    private float speed;
    private float defense;

    public float Health { get { return this.health; } }
    public float Strength { get { return this.strength; } }
    public float Speed { get { return this.speed; } }
    public float Defense { get { return this.defense; } }

    public GameObject selectionPanel;
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

    public void SliderChangeHealth(float value)
    {
        healthValueText.text = value.ToString();
        health = value;
    }

    public void SliderChangeStrength(float value)
    {
        strengthValueText.text = value.ToString();
        strength = value;
    }

    public void SliderChangeSpeed(float value)
    {
        speedValueText.text = value.ToString();
        speed = value;
    }

    public void SliderChangeDefense(float value)
    {
        DefenseValueText.text = value.ToString();
        defense = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
