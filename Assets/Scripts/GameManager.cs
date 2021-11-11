using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    private Player[] players;
    private List<GameObject> units = new List<GameObject>();
    public int cp = 0;
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
        string[] names = { "Blue", "Red", "Yellow" };
        Color[] colors = { Color.blue, Color.red, Color.yellow };

        players = new Player[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            players[i] = new Player(names[i]);
            players[i].UnitColor = colors[i];
        }

        ConfigUnit.instance.StartConfig();
    }

    public Player GetCurrentPlayer()//Returns the current player
    {   
        return players[cp];
    }

    public Player GetSpecificPlayer(int index)//Returns specific player based on index
    {
        return players[index];
    }

    public void NextPlayer()//Iterate through array of players
    {
        cp++;

        if (cp >= players.Length)
        {
            cp = 0;
        }   
    }

    public void AddUnit(GameObject unit)//Add unit to global list of units created by players
    {
        units.Add(unit);
    }

    public void NextPhase()//Called when every player has created their units and have no balance left
    {
        cp = 0;
        BattleSystem.instance.StartConfig();
    }

    public void VisualizeUnits(bool state)//Visualize all units created by players
    {
        for (int i = 0; i < units.Count; i++)
        {
            if(units[i].GetComponent<Unit>().Team == GetCurrentPlayer().Name)
            {
                units[i].transform.GetChild(3).gameObject.SetActive(state);
            }
        }
    }
}
