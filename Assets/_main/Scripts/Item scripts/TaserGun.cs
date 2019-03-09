using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaserGun : item
{
    public override void use()
    {
        stimulus.spawn(stimulusType.electric, transform.position, myPlayer.lookDirection, 0.3f);
    }
}
