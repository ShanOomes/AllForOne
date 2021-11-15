using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDisplay : MonoBehaviour
{
    public PowerUp powerUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(powerUp.name + " has been picked up by " + GameManager.instance.GetCurrentPlayer().Name);
            GameManager.instance.GetCurrentPlayer().AddPowerUp(this.powerUp);
            Destroy(this.gameObject);
        }
    }
}
