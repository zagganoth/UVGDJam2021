using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Loot : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}
