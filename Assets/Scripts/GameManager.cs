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
        string[] names = { "Blue", "Red" };
        players = new Player[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            players[i] = new Player(names[i]);
        }

        ConfigUnit.instance.StartConfig();
    }

    public Player GetCurrentPlayer()
    {   
        return players[cp];
    }

    public Player GetSpecificPlayer(int index)
    {
        return players[index];
    }

    public void NextPlayer()
    {
        cp++;

        if (cp >= players.Length)
        {
            cp = 0;
        }   
    }

    public void AddUnit(GameObject unit)
    {
        units.Add(unit);
    }

    public void NextPhase()
    {
        Debug.Log(units.Count);
    }
}
