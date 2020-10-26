using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    // Public variables
    public GameObject messsage;
    public PlayerController playerToSpawn;
    public bool standardCharachter;
    
    // Private variables
    private bool canSelect;

    private void Start()
    {
        if(standardCharachter) return;
        
        if (PlayerPrefs.HasKey(playerToSpawn.name) && PlayerPrefs.GetInt(playerToSpawn.name) == 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (canSelect)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 playerPos = PlayerController.instance.transform.position;
                
                Destroy(PlayerController.instance.gameObject);

                PlayerController newPlayer = Instantiate(playerToSpawn, playerPos, playerToSpawn.transform.rotation);
                PlayerController.instance = newPlayer;
                
                gameObject.SetActive(false);

                CameraController.instance.target = newPlayer.transform;

                CharacterSelectManager.instance.activePlayer = newPlayer;
                CharacterSelectManager.instance.activeCharacterSelector.gameObject.SetActive(true);
                CharacterSelectManager.instance.activeCharacterSelector = this;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canSelect = true;
            messsage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canSelect = false;
        messsage.SetActive(false);
    }
}
