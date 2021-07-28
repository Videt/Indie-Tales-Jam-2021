using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private Rigidbody2D playerRb2d;
    [SerializeField] private float moveSpeed;
    private Vector2 movement;

    [Header("Fire")]
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody2D playerFirePointRb;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject bulletPrefab;
    private Transform playerFirePointTransform;
    private bool canShoot = true;
    private bool isShooting;
    private Vector2 mousePos;
    private Vector2 aimDirection;
    private Vector2 lookDirection;

    [Header("Clean")]
    private bool canClean;
    private bool isCleaning;
    private GameObject bloodSplash;

    [Header("Health")]
    [SerializeField] private int health = 5;
    [SerializeField] private int numberHearts;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    public delegate void StainCleaned();
    public static event StainCleaned OnStainCleaned;

    private AudioManager audioManager;

    private void Start()
    {
        playerFirePointTransform = playerFirePointRb.transform;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        MouseLook();

        HealthSystem();

        if (canShoot && isShooting)
        {
            Shoot();
            canShoot = false;
            Invoke(nameof(Reload), reloadTime);
        }

        if (canClean && isCleaning)
        {
            Clean();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        playerRb2d.AddForce(moveSpeed * Time.deltaTime * movement, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, playerFirePointTransform.position, playerFirePointTransform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(playerFirePointTransform.up * bulletSpeed, ForceMode2D.Impulse);

        audioManager.Play("ShootSound");
    }

    private void Clean()
    {
        isCleaning = false;
        Destroy(bloodSplash);
        OnStainCleaned?.Invoke();
    }

    private void Reload()
    {
        canShoot = true;
    }

    private void MouseLook()
    {
        mousePos = cam.ScreenToWorldPoint(aimDirection);
        lookDirection = mousePos - playerFirePointRb.position;
        float angle = (Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg) - 90f;
        playerFirePointTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void HealthSystem()
    {
        if (health > numberHearts)
            health = numberHearts;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;

            if (i < numberHearts)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            health -= 1;

        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("BloodSplash"))
        {
            canClean = true;
            bloodSplash = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BloodSplash"))
        {
            canClean = false;
        }
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }

    public void OnAimDirection(InputAction.CallbackContext value)
    {
        aimDirection = value.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext value)
    {
        if (value.performed)
            isShooting = true;

        if (value.canceled)
            isShooting = false;
    }

    public void OnClean(InputAction.CallbackContext value)
    {
        if (canClean && value.performed)
        {
            isCleaning = true;
        }
    }
}
