using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    public IEnumerator destroySelf(float aliveTime)
    {
        yield return new WaitForSeconds(aliveTime);
        Destroy(this.gameObject);
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
