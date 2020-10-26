using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ItemType
{
    healthRestore,
    healthUpgrade,
    weapon
}

public class ShopItem : MonoBehaviour
{
    [Header("Stats")]
    public GameObject buyMessage;
    public ItemType item;
    public int itemCost;

    [Header("Health Upgrade")]
    public int upgradeHealthAmount;

    [Header("Gun item")]
    public Gun[] potentialGuns;    
    public TextMeshProUGUI infoText;
    private SpriteRenderer gunSprite;
    private Gun gun;
    
    private bool inBuyZone = false;

    private void Start()
    {
        gunSprite = GetComponent<SpriteRenderer>();

        if(item == ItemType.weapon)
        {
            int selectGun = Random.Range(0, potentialGuns.Length);
            gun = potentialGuns[selectGun];

            itemCost = gun.itemCost;
            gunSprite.sprite = gun.gunShopSprite;
            infoText.text = gun.weaponName + "\r\n- " + gun.itemCost + " Gold -";
        }
    }

    private void Update()
    {
        if (inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(LevelManager.instance.currentCoins >= itemCost)
                {
                    LevelManager.instance.SpendCoins(itemCost);
                    switch (item)
                    {
                        case ItemType.healthRestore:
                            PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);
                            break;

                        case ItemType.healthUpgrade:
                            PlayerHealthController.instance.IncreaseMaxHealth(upgradeHealthAmount);
                            break;

                        case ItemType.weapon:
                            Gun gunObject = Instantiate(gun);
                            gunObject.transform.parent = PlayerController.instance.gunArm;
                            gunObject.transform.position = PlayerController.instance.gunArm.position;
                            gunObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                            gunObject.transform.localScale = Vector3.one;

                            PlayerController.instance.availableGuns.Add(gunObject);
                            PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                            PlayerController.instance.SwitchGun();
                            break;
                    }

                    // Deactivate shop item
                    gameObject.SetActive(false);
                    inBuyZone = false;

                    AudioManager.instance.PlaySFX(18);
                }
                else
                {
                    AudioManager.instance.PlaySFX(19);
                }
            }           
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            buyMessage.SetActive(true);
            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            buyMessage.SetActive(false);
            inBuyZone = false;
        }
    }
}
