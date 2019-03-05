using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flamethrower : item
{
    public override void use() {
        stimulus.spawn(stimulusType.fire, transform.position, myPlayer.lookDirection, 0.3f);
    }
}
