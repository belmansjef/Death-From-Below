using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("UI Elements")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinAmountText;
    public Image fadeScreen;
    public GameObject hitEffect;
    public GameObject deathScreen;

    [Header("Boss elements")]
    public Slider bossHealthBar;

    [Header("Maps")]
    public GameObject miniMap;
    public GameObject fullscreenMap;

    [Header("Level Fade")]
    public float fadeSpeed = 1f;
    private float currentFade = 1f;
    private bool fadeToBlack;
    private bool fadeOutBlack;

    [Header("Scenes")]
    public string newGameScene;
    public string mainMenuScene;

    [Header("Pause")]
    public GameObject PauseMenu;

    [Header("Gun UI")]
    public Image currentGun;
    public Text gunText;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;

        currentGun.sprite = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gunSprite;
        gunText.text = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].weaponName;
    }

    private void Update()
    {
        if (fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }

        if (fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        Destroy(PlayerController.instance.gameObject);
        SceneManager.LoadScene(newGameScene);
        
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        Destroy(PlayerController.instance.gameObject);
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }
}
