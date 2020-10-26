using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Singleton
    public static LevelManager instance;

    [Header("Stats")]
    public float waitToLoad = 4f;
    public string nextLevel;
    public int currentCoins;

    public bool isPaused = false;

    [Header("Objects")]
    public Transform startPoint;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        currentCoins = CharachterTracker.Instance.currentCoins;
        
        PlayerController.instance.transform.position = startPoint.position;
        PlayerController.instance.canMove = true;
        
        UIController.instance.coinAmountText.text = currentCoins.ToString();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public IEnumerator LevelEnd()
    {
        AudioManager.instance.PlayLevelWin();
        PlayerController.instance.canMove = false;
        UIController.instance.StartFadeToBlack();
        yield return new WaitForSeconds(waitToLoad);

        CharachterTracker.Instance.currentCoins = currentCoins;
        CharachterTracker.Instance.maxHealth = PlayerHealthController.instance.maxHealth;
        CharachterTracker.Instance.currentHealth = PlayerHealthController.instance.currentHealth;
        
        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnpause()
    {
        if (!isPaused)
        {
            UIController.instance.PauseMenu.SetActive(true);
            isPaused = true;

            Time.timeScale = 0f;
        }
        else
        {
            UIController.instance.PauseMenu.SetActive(false);
            isPaused = false;

            Time.timeScale = 1f;
        }
    }

    public void GetCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;

        if(currentCoins < 0)
        {
            currentCoins = 0;
        }

        UpdateCoinUI();
    }


    private void UpdateCoinUI()
    {
        UIController.instance.coinAmountText.text = LevelManager.instance.currentCoins.ToString();
    }
}
