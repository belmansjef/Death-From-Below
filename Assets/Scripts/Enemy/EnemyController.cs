using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Basic enemy stats")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int health = 150;
    [SerializeField] private SpriteRenderer sr = null;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody2D rb = null;
    private Animator anim = null;

    [Header("Drop stats")]
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent = 25f;

    [Header("Chase player stats")]
    [SerializeField] private bool shouldChasePlayer = false;
    [SerializeField] private float rangeToChasePlayer;             

    [Header("Run away stats")]    
    [SerializeField] private bool shouldRunAway = false;
    [SerializeField] private float runAwayRange = 5f;

    [Header("Wander stats")]
    [SerializeField] private bool shouldWander = false;
    [SerializeField] private float wanderLength = 5f;
    [SerializeField] private float pauseLength = 1f;
    private float wanderCounter;
    private float pauseCounter;
    private Vector3 wanderDirection;

    [Header("Patrol stats")]
    [SerializeField] private bool shouldPatrol;
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolPoint;

    [Header("Shooting")]
    [SerializeField] private bool canShoot;
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private float fireRate;
    [SerializeField] private float shootRange;
    private float fireCounter;

    [Header("Hit Effect")]
    [SerializeField] private GameObject[] deathSplatters = null;
    [SerializeField] private Color hitColor;
    [SerializeField] private GameObject hitEffect = null;    

    private void Start()
    {
        iTween.Init(sr.gameObject);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if(shouldWander) pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
    }

    private void Update()
    {
        if(sr.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                if (shouldWander)
                {
                    if(wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        moveDirection = wanderDirection;

                        if(wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                        }
                    }

                    if(pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;
                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }

                if (shouldPatrol)
                {
                    moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;
                    if(Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < .1f)
                    {
                        currentPatrolPoint++;

                        if (currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                }
            }

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runAwayRange && shouldRunAway)
            {
                moveDirection = transform.position - PlayerController.instance.transform.position;
            }

            moveDirection.Normalize();
            rb.velocity = moveDirection * moveSpeed;

            if (canShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= shootRange)
            {
                fireCounter -= Time.deltaTime;

                if (fireCounter <= 0)
                {
                    fireCounter = fireRate;
                    Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
                    AudioManager.instance.PlaySFX(13);
                }
            }
        }

        else
        {
            rb.velocity = Vector3.zero;
        }

        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void DamageEnemy(int damage)
    {
        AudioManager.instance.PlaySFX(2);
        health -= damage;
        Instantiate(hitEffect, transform.position, transform.rotation);
        sr.material.color = Color.white;

        iTween.ColorFrom(sr.gameObject, hitColor, .1f);
        
        if (health <= 0)
        {                        
            int selectedSplatter = Random.Range(0, deathSplatters.Length);
            int rotation = Random.Range(0, 4);
            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90f));
            AudioManager.instance.PlaySFX(1);

            if (shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);

                // Drop random item
                if (dropChance < itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);
                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }            

            Destroy(gameObject);
        }
    }
}
