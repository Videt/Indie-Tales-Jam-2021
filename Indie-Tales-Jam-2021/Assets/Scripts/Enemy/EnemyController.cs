using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int health;
    private bool isAlive = true;

    private Transform cachedTransform;

    [Header("Effects")]
    [SerializeField] private GameObject bloodSplash;
    [SerializeField] private GameObject particleEffect;

    [Header("Movement")]
    [SerializeField] private float speed;
    private Transform playerTransform;

    private AudioManager audioManager;

    private void Awake()
    {
        cachedTransform = transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        Follow();
    }

    private void Follow()
    {
        cachedTransform.position = Vector2.MoveTowards(cachedTransform.position, playerTransform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.tag);

        if (isAlive && collision.CompareTag("Bullet"))
        {
            health -= 1;
        }

        if (isAlive && collision.CompareTag("Player"))
        {
            health = 0;
        }

        if (health <= 0)
        {
            isAlive = false;
            Die();
        }
    }

    private void Die()
    {
        audioManager.Play("EnemyDeathSound");
        Instantiate(bloodSplash, cachedTransform.position, Quaternion.identity);
        Instantiate(particleEffect, cachedTransform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
