using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private int health = 5;

    private void Start()
    {
        playerFirePointTransform = playerFirePointRb.transform;
    }

    private void Update()
    {
        MouseLook();

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
        print("Shoot");

        GameObject bullet = Instantiate(bulletPrefab, playerFirePointTransform.position, playerFirePointTransform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(playerFirePointTransform.up * bulletSpeed, ForceMode2D.Impulse);
    }

    private void Clean()
    {
        print("Clean");
        isCleaning = false;
        Destroy(bloodSplash);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            health -= 1;
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
