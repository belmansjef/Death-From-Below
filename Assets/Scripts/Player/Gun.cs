using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun stats")]
    public string weaponName;
    public Sprite gunSprite;
    public float speed;
    public int damage;
    public float timeBetweenShots;
    
    [Header("Objects")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Shop stats")]
    public int itemCost;
    public Sprite gunShopSprite;

    [Header("Shotgun")]
    public bool isShotgun;
    public Transform[] firePoints;

    private float shotCounter;
    
    private void Update()
    {
        if (!PlayerController.instance.canMove || LevelManager.instance.isPaused) return;
        
        if(shotCounter > 0)
        {
            shotCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
            {
                if (isShotgun)
                {
                    foreach (Transform t in firePoints)
                    {
                        GameObject bulletGo = Instantiate(bulletPrefab, t.position, t.rotation);
                        bulletGo.GetComponent<PlayerBullet>().Setup(speed, damage);
                    }
                }
                else
                {
                    GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    bulletGo.GetComponent<PlayerBullet>().Setup(speed, damage);
                }

                switch (weaponName)
                {
                    case "Pistol":
                        AudioManager.instance.PlaySFX(12);
                        break;
                    case "Shotgun":
                        AudioManager.instance.PlaySFX(21);
                        break;
                    case "Uzi":
                        AudioManager.instance.PlaySFX(22);
                        break;
                    case "Minigun":
                        AudioManager.instance.PlaySFX(22);
                        break;
                }
                    
                shotCounter = timeBetweenShots;
            }
        }
    }
}
