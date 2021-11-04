using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string name;

    public float health;
    public float strength;
    public float speed;
    public float defense;

    public string team;

    //Properties
    public string Name { get { return this.name; } set { this.name  = value; } }

    public float Health { get { return this.health; } set { this.health  = value; } }
    public float Strength { get { return this.strength; } set { this.strength  = value; } }
    public float Speed { get { return this.speed; } set { this.speed  = value; } }
    public float Defense { get { return this.defense; } set { this.defense  = value; } }

    public string Team { get { return this.team; } set { this.team = value; } }

    //Standard constructor
    public Unit(){
        name = "Default";
        health = 50f;
        strength = 50f;
        speed = 50f;
        defense = 50f;
        team = "None";
    }

    //Custom constructor
    public Unit(string name, float health, float strength, float speed, float defense, string team){
        this.name = name;
        this.health = health;
        this.strength = strength;
        this.speed = speed;
        this.defense = defense;
        this.team = team;
    }

    public void SetValues(float health, float strength, float speed, float defense, string team)
    {
        this.health = health;
        this.strength = strength;
        this.speed = speed;
        this.defense = defense;
        this.team = team;
    }
}
