using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    [Header("Break stats")]
    public GameObject[] brokenPieces;
    public int maxPieces = 5;

    [Header("Drop stats")]
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent = 25f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(PlayerController.instance.dashCounter > 0)
            {
                DestroyBreakable();             
            }            
        }
        else if (other.CompareTag("Bullet"))
        {
            DestroyBreakable();
        }
    }

    private void DestroyBreakable()
    {
        Destroy(gameObject);

        // Play SFX
        AudioManager.instance.PlaySFX(0);

        // Show broken pieces
        int piecesToDrop = Random.Range(1, maxPieces);
        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);
            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }

        if (shouldDropItem)
        {
            float dropChance = Random.Range(0f, 100f);

            // Drop random item
            if(dropChance < itemDropPercent)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);
                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }
}
