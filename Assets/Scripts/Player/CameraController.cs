using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    // Singleton
    public static CameraController instance;

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera bigMapCamera;
    public Transform target;

    [Header("Transition")]
    public float transitionSpeed = 2f;

    private bool bigMapActive;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Character select")
        {
            MoveCamera(target);
        }
        
        if (Input.GetKeyDown(KeyCode.M) && SceneManager.GetActiveScene().name != "Boss")
        {
            if (!bigMapActive)
            {
                ActivateBigMap();
            }
            else
            {
                DeactivateBigMap();
            }
        }        
    }

    public void MoveCamera(Transform target)
    {
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10f);
            return;
        }
        
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(target.position.x, target.position.y, transform.position.z), "time" ,transitionSpeed, "easetype", iTween.EaseType.easeOutExpo));
    }

    public void ActivateBigMap()
    {
        // Don't toggle map if paused
        if (LevelManager.instance.isPaused) return;

        bigMapActive = true;

        mainCamera.enabled = false;
        bigMapCamera.enabled = true;

        UIController.instance.miniMap.SetActive(false);
        UIController.instance.fullscreenMap.SetActive(true);

        PlayerController.instance.canMove = false;
        Time.timeScale = 0f;
    }
    
    public void DeactivateBigMap()
    {
        // Don't toggle map if paused
        if (LevelManager.instance.isPaused) return;

        bigMapActive = false;

        mainCamera.enabled = true;
        bigMapCamera.enabled = false;

        UIController.instance.miniMap.SetActive(true);
        UIController.instance.fullscreenMap.SetActive(false);

        PlayerController.instance.canMove = true;
        Time.timeScale = 1f;
    }
}
