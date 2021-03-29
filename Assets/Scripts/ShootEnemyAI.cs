using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyAI : EnemyAI
{
    [SerializeField]
    protected float shootCooldown;
    [SerializeField]
    protected GameObject projectilePrefab;
    [SerializeField]
    protected float projectileVelocity;
    [SerializeField]
    protected float projectileTime;
    protected Transform projectileParent;
    protected bool shootActive;
    public override void childUpdate()
    {
        if (hitActive || !move)
        {
            StopCoroutine("shoot");
        }
        else if (!shootActive && move)
        {
            shootActive = true;
            StartCoroutine(shoot());
        }
    }
    public override void childStart()
    {
        StartCoroutine(shoot());
        shootActive = true;
        projectileParent = FindObjectOfType<Parent>().transform;
    }
    protected enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    protected virtual IEnumerator shoot()
    {
        while (!move)
        {
            yield return null;
        }
        Direction[] directions = { Direction.UP, Direction.DOWN, Direction.RIGHT, Direction.LEFT };
        while (!hitActive && move)
        {
            yield return new WaitForSeconds(shootCooldown);
            shootRandomDir(directions);
        }
        shootActive = false;
    }
    protected virtual void shootRandomDir(Direction[] directions)
    {
        Direction spawnDirection = directions[Random.Range(0, directions.Length)];
        Vector3 spawnPosition;
        Vector2 shootDirection;
        if (spawnDirection == Direction.UP)
        {
            spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.3f, -1);
            shootDirection = new Vector2(0, 1);
        }
        else if (spawnDirection == Direction.DOWN)
        {
            spawnPosition = new Vector3(transform.position.x, transform.position.y - 0.3f, -1);
            shootDirection = new Vector2(0, -1);
        }
        else if (spawnDirection == Direction.LEFT)
        {
            spawnPosition = new Vector3(transform.position.x - 0.3f, transform.position.y, -1);
            shootDirection = new Vector2(-1, 0);
        }
        else //right
        {
            spawnPosition = new Vector3(transform.position.x + 0.3f, transform.position.y, -1);
            shootDirection = new Vector2(1, 0);
        }
        GameObject projObject = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projObject.GetComponent<Rigidbody2D>().velocity = shootDirection * projectileVelocity;
        projObject.transform.SetParent(projectileParent.transform);
        EnemyProjectile proj = projObject.GetComponent<EnemyProjectile>();
        proj.damage = damage;
        Physics2D.IgnoreCollision(proj.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        proj.destroySelf(projectileTime);
    }
}
