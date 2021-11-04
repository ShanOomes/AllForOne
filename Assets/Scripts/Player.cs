using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string name;

    public float balance;

    private int amountOfUnits;

    //Properties
    public string Name { get { return this.name; } set { this.name = value; } }
    public float Balance { get { return this.balance; } }
    public int amountUnits { get { return this.amountOfUnits; } }


    //standard constructor
    public Player()
    {
        this.name = "Default";
        this.balance = 100f;
    }

    //Custom constructor
    public Player(string name)
    {
        this.name = name;
        this.balance = 100f;
    }

    public bool ReduceBalance(int cost)//Reduce balance of player
    {
        //Check if balance is within 
        if ((balance - cost) >= 0 && cost >= 10)
        {
            balance = balance - cost;
            //increment unit count of player
            amountOfUnits++;
            //Debug.Log(name + " balance reduced by: " + cost);
            return true;
        }
        return false;
    }

    public bool CheckBalance(int cost)
    {
        if ((balance - cost) >= 0 && cost >= 10)
        {
            return true;
        }
        return false;
    }

    public bool HasEnough()
    {
        if(balance >= 10)
        {
            return true;
        }
        return false;
    }
}
