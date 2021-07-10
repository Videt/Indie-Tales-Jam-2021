using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Health")]
    private int health = 3;
    private bool isAlive = true;

    private Transform cachedTransform;

    [Header("Effects")]
    [SerializeField] private GameObject bloodSplash;

    [Header("Movement")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float speed;

    public int Health { get => health; }

    public bool IsAlive { get => isAlive; }

    private void Awake()
    {
        cachedTransform = transform;
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
        print("Died");
        Instantiate(bloodSplash, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
