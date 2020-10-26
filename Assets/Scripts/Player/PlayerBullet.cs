using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Objects")]
    public GameObject impactEffect;

    // Private variables
    private float speed;
    private int damage;
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = transform.right * speed;
    }

    public void Setup(float _speed, int _damage)
    {
        speed = _speed;
        damage = _damage;
    }
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().DamageEnemy(damage);
        }
        else if (other.CompareTag("Boss"))
        {
            BossController.instance.TakeDamage(damage);
        }

        AudioManager.instance.PlaySFX(4);

        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
