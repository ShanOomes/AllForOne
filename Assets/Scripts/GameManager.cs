using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, BLUETURN, REDTURN, REDSETUP, BLUESETUP, WON, LOST }
public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; set; }
    public BattleState battleState;

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
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
