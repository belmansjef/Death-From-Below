using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Singleton
    public static PlayerHealthController instance;

    [Header("Player health stats")]
    public int maxHealth;
    public int currentHealth;
    public float damageInvincLength = 1f;
    public float hitEffectTime = 0.05f;
    public Color invincibleColor;
    public Animator anim;
    private float invincCount;

    private void Awake()
    {
       instance = this;
    }

    private void Start()
    {
        maxHealth = CharachterTracker.Instance.maxHealth;
        currentHealth = CharachterTracker.Instance.currentHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UpdateHealthUI();
    }

    private void Update()
    {
        if(currentHealth == 1)
        {
            anim.SetTrigger("isLow");
        }

        if(invincCount > 0)
        {
            invincCount -= Time.deltaTime;
            if(invincCount <= 0)
            {
                PlayerController.instance.sr.color = Color.white;
            }
        }
    }

    public void DamagePlayer()
    {
        if (invincCount <= 0)
        {
            anim.SetTrigger("isHit");

            AudioManager.instance.PlaySFX(11);
            currentHealth--;

            invincCount = damageInvincLength;

            MakeInvincible(invincCount);            

            if (currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false);
                UIController.instance.miniMap.SetActive(false);
                UIController.instance.deathScreen.SetActive(true);
                AudioManager.instance.PlaySFX(8);
                AudioManager.instance.PlayGameOver();
            }

            UpdateHealthUI();
        }
    }

    public void MakeInvincible(float length)
    {
        
        invincCount = length;
        PlayerController.instance.sr.color = invincibleColor;
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthUI();
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UpdateHealthUI();
    }

    public void FlashLowHP()
    {

    }

    public void UpdateHealthUI()
    {
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
