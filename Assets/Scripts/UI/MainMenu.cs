using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Stats")]
    public string levelToLoad;
    public GameObject deletionPrompt;

    public CharacterSelector[] charsToDelete;

    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void DeleteSave()
    {
        deletionPrompt.SetActive(true);
    }

    public void ConfirmDelete()
    {
        foreach (CharacterSelector var in charsToDelete)
        {
            PlayerPrefs.SetInt(var.playerToSpawn.name, 0);
        }
        deletionPrompt.SetActive(false);
    }

    public void CancelDelete()
    {
        deletionPrompt.SetActive(false);
    }
}
