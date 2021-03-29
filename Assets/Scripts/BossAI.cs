using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : ShootEnemyAI
{

    protected override void childDie()
    {
        GameController.instance.spawnLadder(GameController.instance.currentRoom);
    }
    
}
