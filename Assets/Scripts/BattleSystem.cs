using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BattleState { START, BLUETURN, REDTURN, WON, LOST }
public enum PlayerTurnState { Blue, Red }
public class BattleSystem : MonoBehaviour
{
    public GameObject unit;

    public BattleState battleState;
    public PlayerTurnState playerState;

    public TextMeshProUGUI balanceBlue;
    public TextMeshProUGUI balanceRed;

    private Player redPlayer;
    private Player bluePlayer;

    //public GameObject selectionPanel;

    private Camera cam;
    private Ray ray;

    private int placeAbleMask;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        placeAbleMask = LayerMask.GetMask("PlaceAble");

        battleState = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        Player redPlayer = new Player("Red");
        Player bluePlayer = new Player("Blue");

        balanceRed.text = redPlayer.Balance.ToString();
        balanceBlue.text = bluePlayer.Balance.ToString();
    }

    Unit CreateUnit(string name, float health, float strength, float speed, float defense)
    {
        Unit unit;
        switch (playerState)
        {
            case PlayerTurnState.Blue:
                unit = new Unit(name, health, strength, speed, defense, Team.Blue);
                return unit;
            case PlayerTurnState.Red:
                unit = new Unit(name, health, strength, speed, defense, Team.Red);
                return unit;
            default:
                break;
        }
        return null;
    }

    void PlaceUnit(Vector3 pos)
    {
        Instantiate(unit, pos, Quaternion.identity).GetComponent<Unit>().SetValues("Henk", 10f, 10f, 10f, 10f, Team.Blue);
    }
    void ApplyUnit()
    {
        //Panel close
        //Activate placing sequance
        //Placing places instance of Unity prefab with the correct stats
    }

    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, placeAbleMask))
            {
                if (hit.collider != null)
                {
                    PlaceUnit(hit.point);
                }
            }
        }
    }
}
