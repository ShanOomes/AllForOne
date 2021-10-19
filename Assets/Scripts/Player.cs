using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string name;

    private float balance;

    private int amountOfUnits;

    //Properties
    public string Name { get { return this.name; } set { this.name = value; } }
    public float Balance { get { return this.balance; } }
    public int amountUnits { get { return this.amountOfUnits; } }

    //Custom constructor
    public Player(string name)
    {
        this.name = name;
        this.balance = 100f;
    }

    public void AddUnit()
    {
        amountOfUnits++;
    }
}
