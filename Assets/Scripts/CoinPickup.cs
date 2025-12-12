using UnityEngine;

public class CoinPickup : Pickupable
{
    public override void OnPickup(GameObject player)
    {
        // Here you can add code to increase the player's coin count, play a sound, etc.
        Debug.Log("Coin picked up!");

        // Destroy the coin object after pickup
        Destroy(gameObject);
    }
}
