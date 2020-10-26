using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossController : MonoBehaviour
{
    // Singleton
    public static BossController instance;

    // Public variables
    [Header("Action sequences")]
    public BossSequence[] sequences;
    private int currentSequence;
    
    public int currentHealth;
    public GameObject deathEffect, hitEffect, levelExit;

    private BossAction[] actions;
    private int currentAction = 0;
    private float actionCounter;
    private float shotCounter;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        actions = sequences[currentSequence].actions;
        
        actionCounter = actions[currentAction].actionDuration;
        rb = GetComponent<Rigidbody2D>();

        UIController.instance.bossHealthBar.maxValue = currentHealth;
        UIController.instance.bossHealthBar.value = currentHealth;
        
    }

    private void Update()
    {
        if (actionCounter > 0)
        {
            actionCounter -= Time.deltaTime;
            
            HandleMovement();

            HandleShooting();
        }
        else
        {
            currentAction = (currentAction + 1) % actions.Length;

            actionCounter = actions[currentAction].actionDuration;
        }
    }

    private void HandleMovement()
    {
        moveDirection = Vector2.zero;

        if (actions[currentAction].shouldMove)
        {
            if (actions[currentAction].shouldChasePlayer)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;
                moveDirection.Normalize();
            }
            else if (actions[currentAction].moveToPoint && Vector2.Distance(transform.position, actions[currentAction].pointToMoveTowards.position) > 0.5f)
            {
                moveDirection = actions[currentAction].pointToMoveTowards.position - transform.position;
                moveDirection.Normalize();
            }
        }
            
        rb.velocity = moveDirection * actions[currentAction].moveSpeed;
    }

    private void HandleShooting()
    {
        if (actions[currentAction].shouldShoot)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = actions[currentAction].timeBetweenShots;

                foreach (Transform t in actions[currentAction].shotPoints)
                {
                    Instantiate(actions[currentAction].bulletPrefab, t.position, t.rotation);
                }
            }
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);

        Instantiate(deathEffect, transform.position, transform.rotation);

        if (Vector2.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
        {
            levelExit.transform.position += transform.right * 4f;
        }
        levelExit.SetActive(true);
        
        UIController.instance.bossHealthBar.gameObject.SetActive(false);
    }
    
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth <= sequences[currentSequence].endSequenceHealth &&
                 currentSequence < sequences.Length - 1)
        {
            currentSequence++;
            actions = sequences[currentSequence].actions;
            currentAction = 0;
            actionCounter = actions[currentAction].actionDuration;
        }

        UIController.instance.bossHealthBar.value = currentHealth;
    }
    
    
}

[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionDuration;

    public bool shouldMove;
    public float moveSpeed;
    public bool shouldChasePlayer;
    public bool moveToPoint;
    
    public Transform pointToMoveTowards;
    
    public bool shouldShoot;
    public float timeBetweenShots;
    public GameObject bulletPrefab;
    public Transform[] shotPoints;
}

[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]
    public BossAction[] actions;
    public int endSequenceHealth;
}
