using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, BLUETURN, REDTURN, REDSETUP, BLUESETUP, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public static BattleSystem instance { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public GameObject unit;

    public BattleState battleState;

    public Player redPlayer;
    public Player bluePlayer;

    private Camera cam;
    private Ray ray;

    private int placeAbleMask;

    private bool isPlacing = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        placeAbleMask = LayerMask.GetMask("PlaceAble");

        UImanager.instance.selectionPanel.SetActive(true);
        if (SetupBattle())
        {
            battleState = BattleState.BLUESETUP;
            UImanager.instance.highlight("blue");
        }
    }

    bool SetupBattle()
    {
        redPlayer = new Player("Red");
        bluePlayer = new Player("Blue");

        UImanager.instance.balanceRed.text = redPlayer.Balance.ToString();
        UImanager.instance.balanceBlue.text = bluePlayer.Balance.ToString();
        return true;
    }

    bool SetUnit(Vector3 pos)
    {
        GameObject tmp = Instantiate(unit, pos, Quaternion.identity);
        float[] values = UImanager.instance.GetValues();
        switch (battleState)
        {
            case BattleState.BLUESETUP:
                tmp.GetComponent<Unit>().SetValues(UImanager.instance.input.text, values[0], values[1], values[2], values[3], Team.Blue);
                tmp.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                break;
            case BattleState.REDSETUP:
                tmp.GetComponent<Unit>().SetValues(UImanager.instance.input.text, values[0], values[1], values[2], values[3], Team.Red);
                tmp.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                break;
            default:
                break;
        }
        return true;
    }
    void ApplyUnit()
    {
        //Panel close
        //Activate placing sequance
        //Placing places instance of Unity prefab with the correct stats
    }

    public void ButtonClick()
    {
        UImanager manager = UImanager.instance;
        if(battleState == BattleState.BLUESETUP)
        {
            if (bluePlayer.CheckBalance(manager.GetCost()))
            {
                bluePlayer.ReduceBalance(manager.GetCost());
                UImanager.instance.balanceBlue.text = bluePlayer.Balance.ToString();
                UImanager.instance.selectionPanel.SetActive(false);
                isPlacing = true;
            }
        }
        else
        {
            if (redPlayer.CheckBalance(manager.GetCost()))
            {
                redPlayer.ReduceBalance(manager.GetCost());
                UImanager.instance.balanceRed.text = redPlayer.Balance.ToString();
                UImanager.instance.selectionPanel.SetActive(false);
                isPlacing = true;
            }
        }
    }

    // Update is called once per frame
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
                            if (battleState == BattleState.BLUESETUP && redPlayer.Balance >= 10)
                            {
                                battleState = BattleState.REDSETUP;
                                UImanager.instance.highlight("red");
                                StartCoroutine(ResetSetup());
                            }
                            else if (battleState == BattleState.REDSETUP && bluePlayer.Balance >= 10)
                            {
                                battleState = BattleState.BLUESETUP;
                                UImanager.instance.highlight("blue");
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

    public IEnumerator ResetSetup()
    {
        yield return new WaitForSeconds(1f);
        UImanager.instance.selectionPanel.SetActive(true);
        UImanager.instance.textCost.color = new Color(255, 255, 255, 1);
        for (int i = 0; i < UImanager.instance.listSliders.Count; i++)
        {
            UImanager.instance.listSliders[i].value = 0;
        }
    }
}
