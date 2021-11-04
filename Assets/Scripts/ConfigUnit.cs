using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigUnit : MonoBehaviour
{
    public static ConfigUnit instance { get; set; }
    public GameObject unit;

    public Player redPlayer;
    public Player bluePlayer;

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

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        placeAbleMask = LayerMask.GetMask("PlaceAble");

        UImanager.instance.selectionPanel.SetActive(true);
        if (SetupBattle())
        {
            updateUI("title", "Turn: blue");
            updateUI("balance", "100");
            RandomizeSliders();
            GameManager.instance.battleState = BattleState.BLUESETUP;
        }
    }

    void Update()
    {
        if (isPlacing)
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, placeAbleMask))
                {
                    if (hit.collider != null)
                    {
                        if (SetUnit(hit.point))
                        {
                            isPlacing = false;
                            if (GameManager.instance.battleState == BattleState.BLUESETUP && redPlayer.Balance >= 10)
                            {
                                GameManager.instance.battleState = BattleState.REDSETUP;
                                updateUI("title", "Turn: Red");
                                StartCoroutine(ResetSetup());
                            }
                            else if (GameManager.instance.battleState == BattleState.REDSETUP && bluePlayer.Balance >= 10)
                            {
                                GameManager.instance.battleState = BattleState.BLUESETUP;
                                updateUI("title", "Turn: blue");
                                StartCoroutine(ResetSetup());
                            }
                            else
                            {
                                print("Start of the game");
                            }
                        }
                    }
                }
            }
        }
    }

    bool SetUnit(Vector3 pos)
    {
        GameObject tmp = Instantiate(unit, pos, Quaternion.identity);
        List<Slider> sliders = UImanager.instance.GetValues();
        switch (GameManager.instance.battleState)
        {
            case BattleState.BLUESETUP:
                tmp.GetComponent<Unit>().SetValues(sliders[0].value, sliders[1].value, sliders[2].value, sliders[3].value, Team.Blue);
                tmp.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                break;
            case BattleState.REDSETUP:
                tmp.GetComponent<Unit>().SetValues(sliders[0].value, sliders[1].value, sliders[2].value, sliders[3].value, Team.Red);
                tmp.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                break;
            default:
                break;
        }
        return true;
    }

    public void ButtonClick()
    {
        UImanager manager = UImanager.instance;
        if(GameManager.instance.battleState == BattleState.BLUESETUP)
        {
            if (bluePlayer.CheckBalance(manager.GetTotalCost()))
            {
                bluePlayer.ReduceBalance(manager.GetTotalCost());
                updateUI("balance", redPlayer.Balance.ToString());
                manager.selectionPanel.SetActive(false);
                isPlacing = true;
            }
        }
        else
        {
            if (redPlayer.CheckBalance(manager.GetTotalCost()))
            {
                redPlayer.ReduceBalance(manager.GetTotalCost());
                updateUI("balance", bluePlayer.Balance.ToString());
                manager.selectionPanel.SetActive(false);
                isPlacing = true;
            }
        }
    }

    bool SetupBattle()
    {
        redPlayer = new Player("Red");
        bluePlayer = new Player("Blue");
        return true;
    }

    private void updateUI(string action = "", string value = "")
    {
        UImanager manager = UImanager.instance;
        switch (action)
        {
            case "title":
                manager.title.text = value;
                break;
            case "balance":
                manager.balance.text = value;
                break;
        }
    }

    public IEnumerator ResetSetup()
    {
        yield return new WaitForSeconds(1f);
        UImanager manager = UImanager.instance;
        manager.selectionPanel.SetActive(true);
        manager.textCost.color = new Color(255, 255, 255, 1);
        RandomizeSliders();
    }

    public void RandomizeSliders(){
        UImanager manager = UImanager.instance;
        for (int i = 0; i < manager.listSliders.Count; i++)
        {
            manager.listSliders[i].value = Random.Range(1, 100);
        }
    } 
}
