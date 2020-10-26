using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [Header("Stats")]
    public Gun gun;
    public float waitToBeCollected = .5f;

    private void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && waitToBeCollected <= 0)
        {
            bool hasGun = false;

            foreach(Gun gunToCheck in PlayerController.instance.availableGuns)
            {
                if(gun.weaponName == gunToCheck.weaponName)
                    hasGun = true;
            }

            if(!hasGun)
            {
                Gun gunObject = Instantiate(gun);
                gunObject.transform.parent = PlayerController.instance.gunArm;
                gunObject.transform.position = PlayerController.instance.gunArm.position;
                gunObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                gunObject.transform.localScale = Vector3.one;
                
                PlayerController.instance.availableGuns.Add(gunObject);
                PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                PlayerController.instance.SwitchGun();
            }

            AudioManager.instance.PlaySFX(6);
            Destroy(gameObject);
        }
    }
}
