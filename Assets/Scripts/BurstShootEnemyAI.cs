using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstShootEnemyAI : ShootEnemyAI
{
    [SerializeField]
    protected int burstShots;
    [SerializeField]
    protected float burstCooldown;
    protected override IEnumerator shoot()
    {
        while (!move)
        {
            yield return null;
        }
        Direction[] directions = { Direction.RIGHT, Direction.LEFT };
        while (move)
        {

            int numProjectiles = 0;
            while (numProjectiles < burstShots && move)
            {
                yield return new WaitForSeconds(shootCooldown);
                shootRandomDir(directions);
                numProjectiles += 1;
            }
            yield return new WaitForSeconds(burstCooldown);
        }
        shootActive = false;
    }
}
