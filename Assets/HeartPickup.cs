using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : Loot {
    [SerializeField]
    float health;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && GameController.instance.canAddHealth())
        {
            GameController.instance.addHealth(health);
            base.OnTriggerEnter2D(collision);
        }
    }
}
