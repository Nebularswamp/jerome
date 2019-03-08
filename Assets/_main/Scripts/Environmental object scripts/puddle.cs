using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puddle : environmentalObject
{
    private void Start() {
        Collider[] hit = Physics.OverlapBox(transform.position, GetComponent<Collider>().bounds.extents);
        foreach(Collider i in hit) {
            if (i.gameObject != gameObject && i.transform.tag == "puddle") Destroy(gameObject);
        }
            
    }
}
