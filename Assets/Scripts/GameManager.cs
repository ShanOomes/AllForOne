using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; set; }
    private Player[] players;
    private List<Unit> units = new List<Unit>();
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

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(players[0].Name + " " + players[0].Balance);
        //Debug.Log(players[1].Name + " " + players[1].Balance);
    }

    public Player GetCurrentPlayer()
    {   
        return players[cp];
    }

    public void NextPlayer()
    {
        cp++;

        if (cp >= players.Length)
        {
            cp = 0;
        }   
    }
}
