using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : MonoBehaviour
{
    [Header("Chest stats")]
    public GunPickup[] potentialGuns;
    public Sprite openChest;
    public Transform spawnPoint;
    public GameObject notification;    
    private SpriteRenderer sr;
    private bool canOpen;
    private bool isOpen;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {  
        if(canOpen && !isOpen)
        {
            notification.SetActive(true);

            if (Input.GetButtonDown("Interact"))
            {
                int gunSelect = Random.Range(0, potentialGuns.Length);
                Instantiate(potentialGuns[gunSelect], spawnPoint.position, Quaternion.identity);

                iTween.ScaleFrom(gameObject, new Vector3(1.2f, 1.2f, 1.2f), 1f);
                AudioManager.instance.PlaySFX(23);

                sr.sprite = openChest;
                isOpen = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = true;            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
            notification.SetActive(false);
        }
    }
}
