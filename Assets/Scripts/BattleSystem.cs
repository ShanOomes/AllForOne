using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, BLUETURN, REDTURN, REDSETUP, BLUESETUP, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public GameObject unit;

    public BattleState battleState;

    private Player redPlayer;
    private Player bluePlayer;

    private Camera cam;
    private Ray ray;

    private int placeAbleMask;

    private bool isPlacing = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        placeAbleMask = LayerMask.GetMask("PlaceAble");

        if (SetupBattle())
        {
            battleState = BattleState.BLUESETUP;
            UImanager.instance.title.text = "Turn: Blue";
        }
    }

    bool SetupBattle()
    {
        Player redPlayer = new Player("Red");
        Player bluePlayer = new Player("Blue");

        UImanager.instance.balanceRed.text = redPlayer.Balance.ToString();
        UImanager.instance.balanceBlue.text = bluePlayer.Balance.ToString();
        return true;
    }

    bool SetUnit(Vector3 pos)
    {
        GameObject tmp = Instantiate(unit, pos, Quaternion.identity);
        switch (battleState)
        {
            case BattleState.BLUESETUP:
                tmp.GetComponent<Unit>().SetValues(UImanager.instance.input.text, UImanager.instance.Health, UImanager.instance.Strength, UImanager.instance.Speed, UImanager.instance.Defense, Team.Blue);
                tmp.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                break;
            case BattleState.REDSETUP:
                tmp.GetComponent<Unit>().SetValues(UImanager.instance.input.text, UImanager.instance.Health, UImanager.instance.Strength, UImanager.instance.Speed, UImanager.instance.Defense, Team.Red);
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
        UImanager.instance.selectionPanel.SetActive(false);
        isPlacing = true;
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
                            if (battleState == BattleState.BLUESETUP)
                            {
                                battleState = BattleState.REDSETUP;
                                UImanager.instance.title.text = "Turn: Red";
                            }
                            else if (battleState == BattleState.REDSETUP)
                            {
                                battleState = BattleState.BLUESETUP;
                                UImanager.instance.title.text = "Turn: Blue";
                            }
                            StartCoroutine(ResetSetup());
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

        UImanager.instance.healthSlider.value = 0;
        UImanager.instance.strengthSlider.value = 0;
        UImanager.instance.speedSlider.value = 0;
        UImanager.instance.defenseSlider.value = 0;
    }
}
