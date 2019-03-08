using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterBottle : item
{
    public GameObject puddle;
    public override void use() {
        base.use();
        Bounds pBounds = myPlayer.GetComponent<CharacterController>().bounds;
        Vector3 puddleSpawn = myPlayer.transform.position + myPlayer.lookDirection*myPlayer.pickupRange;
        puddleSpawn.y = pBounds.center.y - pBounds.extents.y;
        Instantiate(puddle, puddleSpawn, Quaternion.identity);
    }
}
