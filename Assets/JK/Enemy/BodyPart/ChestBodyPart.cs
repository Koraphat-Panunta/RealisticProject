using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBodyPart : BodyPart
{
    public override void GotHit(float damage)
    {
        enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._painState, new BodyHitNormalReaction(enemy));
        enemy.TakeDamage(damage);
    }
}
