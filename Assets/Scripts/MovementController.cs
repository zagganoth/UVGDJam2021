using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    GameObject projectileParent;
    [SerializeField]
    float projCooldown;
    [SerializeField]
    float projectileVelocity;
    [SerializeField]
    float projectileTime;
    bool projShooting;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        projShooting = false;
    }
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        rb.velocity = input * moveSpeed;
    }
    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (projShooting)
            {
                StopAllCoroutines();
                projShooting = false;
                return;
            }
            projShooting = true;
            //favour up/down over left/right
            Vector2 shootDirection = context.ReadValue<Vector2>();
            //Debug.Log(shootDirection);




            StartCoroutine(runCooldown(shootDirection));
        }
        if(context.canceled)
        {
            projShooting = false;
            StopAllCoroutines();
        }
    }
    private IEnumerator runCooldown(Vector2 shootDirection)
    {
        while (projShooting)
        {
            Vector3 spawnPosition = transform.position;
            if (shootDirection.x != 0)
            {
                shootDirection = new Vector2(shootDirection.x, 0);
                if (shootDirection.x > 0)
                {
                    spawnPosition = new Vector3(spawnPosition.x + 0.4f, spawnPosition.y, -1);
                }
                else
                {
                    spawnPosition = new Vector3(spawnPosition.x - 0.4f, spawnPosition.y, -1);
                }
            }
            else
            {
                new Vector2(shootDirection.y, 0);
                if (shootDirection.y > 0)
                {
                    spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y + 0.4f, -1);
                }
                else
                {
                    spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y - 0.4f, -1);
                }
            }
            GameObject projObject = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            projObject.GetComponent<Rigidbody2D>().velocity = shootDirection * projectileVelocity;
            projObject.transform.SetParent(projectileParent.transform);
            Projectile proj = projObject.GetComponent<Projectile>();
            Physics2D.IgnoreCollision(proj.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            proj.destroySelf(projectileTime);
            yield return new WaitForSeconds(projCooldown);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
