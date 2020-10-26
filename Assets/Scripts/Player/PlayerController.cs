using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
    // Singleton
    public static PlayerController instance;
    
    [Header("Player stats")]
    public float moveSpeed = 5f;   
    public Transform gunArm;
    public SpriteRenderer sr;
    private float activeMoveSpeed;
    [HideInInspector] public bool canMove = true;

    [Header("Dash settings")]
    public float dashSpeed = 8f;
    public float dashLength = 0.5f;
    public float dashCooldown = 1f;
    public float dashInvincibility = 0.5f;
    [HideInInspector] public float dashCounter;
    private float dashCoolDownCounter;

    [Header("Guns")]
    public List<Gun> availableGuns = new List<Gun>();
    [HideInInspector] public int currentGun;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 moveInput;    

    private void Awake()
    {
        if (instance == null) instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // cam = Camera.main;
        activeMoveSpeed = moveSpeed;

        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunSprite;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }

    private void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            // Get player input
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            // Make player move at the same speed in every direction
            moveInput.Normalize();

            // Set player position
            rb.velocity = moveInput * activeMoveSpeed;

            // Vectors used to calculate arm rotation
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;
            }

            // Rotate gun arm
            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0f, 0f, angle);

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if(availableGuns.Count > 0)
                {
                    currentGun = (currentGun + 1) % availableGuns.Count;
                    SwitchGun();
                }
                else
                {
                    Debug.LogError("Player has no guns!");
                }
            }

            if (Input.GetButtonDown("Dash"))
            {
                if (dashCoolDownCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    if (moveInput.x >= 0)
                        anim.SetTrigger("dashForward");
                    else if (moveInput.x < 0)
                        anim.SetTrigger("dashBackward");

                    AudioManager.instance.PlaySFX(8);
                    PlayerHealthController.instance.MakeInvincible(dashInvincibility);
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolDownCounter = dashCooldown;
                }
            }

            if (dashCoolDownCounter > 0)
            {
                dashCoolDownCounter -= Time.deltaTime;
            }

            if (moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
    }

    public void SwitchGun()
    {
        foreach(Gun gun in availableGuns)
        {
            gun.gameObject.SetActive(false);
        }

        availableGuns[currentGun].gameObject.SetActive(true);
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunSprite;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }
}
