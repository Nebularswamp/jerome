using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leafCutters : item
{
    public override void use() {
        base.use();
        attack();
    }
}
