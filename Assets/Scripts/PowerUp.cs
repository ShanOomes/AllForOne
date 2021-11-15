using UnityEngine;

[CreateAssetMenu(fileName = "New Powerup", menuName = "Powerup")]
public class PowerUp : ScriptableObject
{
    public new string name;

    public float duration;

    public int power;

    public GameObject Object;
}
