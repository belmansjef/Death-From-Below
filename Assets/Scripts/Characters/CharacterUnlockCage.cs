using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterUnlockCage : MonoBehaviour
{
    // Public variables
    public GameObject message;
    public CharacterSelector[] charSelects;
    public SpriteRenderer cagedSR;
    
    // Private variables
    private bool canUnlock;
    private CharacterSelector playerToUnlock;

    private void Start()
    {
        playerToUnlock = charSelects[Random.Range(0, charSelects.Length)];

        cagedSR.sprite = playerToUnlock.playerToSpawn.sr.sprite;
    }

    private void Update()
    {
        if (canUnlock)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerPrefs.SetInt(playerToUnlock.playerToSpawn.name, 1);
                
                Instantiate(playerToUnlock, transform.position, transform.rotation);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canUnlock = true;
            message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canUnlock = false;
            message.SetActive(false);
        }
    }
}
