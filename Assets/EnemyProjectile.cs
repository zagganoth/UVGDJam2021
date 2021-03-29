using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    public float damage;
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameController.instance.takeDamage(damage);
        }
        base.OnCollisionEnter2D(collision);
    }
}
