    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Coin Stats")]
    public int coinValue = 1;
    public float waitToBeCollected = .5f;

    private void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && waitToBeCollected <= 0)
        {
            LevelManager.instance.GetCoins(coinValue);
            AudioManager.instance.PlaySFX(5);
            Destroy(gameObject);
        }
    }
}
