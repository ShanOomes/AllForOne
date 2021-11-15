using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    private Player[] players;
    private List<GameObject> units = new List<GameObject>();
    public int cp = 0;

    public PowerUp[] arrPowerups = new PowerUp[3];
    public GameObject terrain;
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
        Color[] colors = { Color.blue, Color.red };

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

    public void RemoveUnit(GameObject unit)//Remove unit from global list of units created by players
    {
        units.Remove(unit);
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

    public void SpawnPowerup()
    {
        MeshCollider col = terrain.GetComponent<MeshCollider>();

        int xRandom = 0;
        int zRandom = 0;

        xRandom = (int)Random.Range(col.bounds.min.x, col.bounds.max.x);
        zRandom = (int)Random.Range(col.bounds.min.z, col.bounds.max.z);

        Instantiate(arrPowerups[Random.Range(0,arrPowerups.Length)].Object, new Vector3(xRandom, 0.5f, zRandom), Quaternion.identity);
    }

    public void CheckUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            Unit tmp = units[i].GetComponent<Unit>();
            if(!tmp.CheckRoof()){
                Destroy(units[i]);
                RemoveUnit(units[i]);
            }
        }
    }
}
