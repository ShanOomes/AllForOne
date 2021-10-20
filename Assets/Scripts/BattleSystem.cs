using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, BLUETURN, REDTURN, WON, LOST }
public enum PlayerTurnState { Blue, Red }
public class BattleSystem : MonoBehaviour
{
    public GameObject unit;

    public BattleState battleState;
    public PlayerTurnState playerState;

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

        battleState = BattleState.START;
        if (SetupBattle())
        {
            playerState = PlayerTurnState.Blue;
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
        switch (playerState)
        {
            case PlayerTurnState.Blue:
                tmp.GetComponent<Unit>().SetValues("Henk", UImanager.instance.Health, UImanager.instance.Strength, UImanager.instance.Speed, UImanager.instance.Defense, Team.Blue);
                break;
            case PlayerTurnState.Red:
                tmp.GetComponent<Unit>().SetValues("Henk", 10f, 10f, 10f, 10f, Team.Red);
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
                            print("Succes");
                            isPlacing = false;
                            if (playerState == PlayerTurnState.Blue)
                            {
                                playerState = PlayerTurnState.Red;
                            }
                            else if (playerState == PlayerTurnState.Red)
                            {
                                playerState = PlayerTurnState.Blue;
                            }
                        }
                    }
                }
            }
        }
    }
}
