using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Stats")]
    public bool closeWhenEntered;
    public GameObject[] doors;
    public GameObject mapHider;
    [HideInInspector] public bool roomActive = false;

    private bool isCleared = false;

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
            isCleared = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.instance.MoveCamera(transform);

            if (closeWhenEntered && !isCleared)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            roomActive = true;
            mapHider.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            roomActive = false;
        }
    }
}
