﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class item : MonoBehaviour
{
    public string itemName = "itemName";
    public GameObject myPrefab;
    public int damage = 1;

    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void use() {
        
    }

    public virtual void discard() {
        Vector3 dPos = transform.position;
        transform.parent = null;
        transform.position = dPos;
        rb.isKinematic = false;
    }

    
}
