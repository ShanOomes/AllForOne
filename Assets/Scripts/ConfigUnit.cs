using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigUnit : MonoBehaviour
{
    public static ConfigUnit instance { get; set; }
    public GameObject unit;

    private Camera cam;
    private Ray ray;

    private int placeAbleMask;

    private bool isPlacing = false;
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

    public void StartConfig()//Init the config phase of the game
    {
        cam = Camera.main;
        placeAbleMask = LayerMask.GetMask("PlaceAble");

        UImanager.instance.selectionPanel.SetActive(true);

        //Set UI for start of game
        UImanager.instance.UpdateUI();
        UImanager.instance.RandomizeSliders();

        UImanager.instance.Tunnels(0.5f);
    }

    void Update()
    {
        if (isPlacing)//check if player can place his created unit
        {

            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, placeAbleMask))//Raycasting for placing the unit
                {
                    if (hit.collider != null)
                    {
                        if (SetUnit(hit.point))
                        {
                            isPlacing = false;
                            GameManager.instance.NextPlayer();//Moves to next player
                            if (GameManager.instance.GetCurrentPlayer().HasEnough())//Check if next player has balance left
                            {
                                StartCoroutine(UImanager.instance.ResetSetup());//Next plapyer can configure their unit
                            }
                            else
                            {
                                //Debug.Log("Nobody has money left, start next phase of game");//Every player has no balance left
                                GameManager.instance.NextPhase();
                            }
                        }
                    }
                }
            }
        }
    }

    bool SetUnit(Vector3 pos)//Create and apply the UI sliders to the created unit
    {
        GameObject tmp = Instantiate(unit, pos, Quaternion.identity);
        List<Slider> sliders = UImanager.instance.GetValues();

        Color c = GameManager.instance.GetCurrentPlayer().UnitColor;
        c.a = .1f;

        tmp.GetComponent<Unit>().SetValues(sliders[0].value, sliders[1].value, sliders[2].value, sliders[3].value, GameManager.instance.GetCurrentPlayer().Name);//Set stats of created unit
        tmp.transform.GetChild(1).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", GameManager.instance.GetCurrentPlayer().UnitColor);
        tmp.transform.GetChild(3).GetComponent<Renderer>().material.SetColor("_Color", c);
        GameManager.instance.AddUnit(tmp);//Add created unit to global list of units
        return true;
    }

    public void ButtonClick()//Button to confirm the configuration of the unit
    {
        UImanager UImanager = UImanager.instance;
        GameManager gameManager = GameManager.instance;

        if (gameManager.GetCurrentPlayer().ReduceBalance(UImanager.GetTotalCost()))
        {
            UImanager.selectionPanel.SetActive(false);
            isPlacing = true;
        }
    }
}
