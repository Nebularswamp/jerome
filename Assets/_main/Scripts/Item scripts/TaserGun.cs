using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaserGun : item
{
    public float zapSpeed = 2f;
    public float zapTime = 3f;
    public override void use()
    {
        stimulus zap = stimulus.spawn(stimulusType.electric, myPlayer.transform.position, myPlayer.lookDirection, 0);
        zap.speed = zapSpeed;
        zap.lifeTime = zapTime;
    }
}
