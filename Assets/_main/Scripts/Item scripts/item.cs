﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class item : MonoBehaviour
{
    public string itemName = "itemName";
    public GameObject myPrefab;
    public int damage = 1;
    public float defaultUseTime = 0;
    public bool continuous = false;
    public float attackRange;

    [HideInInspector] public float useTime;
    [HideInInspector] public bool inUse = false;

    protected player myPlayer;
    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        useTime = defaultUseTime;
    }

    private void Update() {
        if (useTime > 0) useTime -= Time.deltaTime;
        if (useTime <= 0 && inUse) {
            use();
            useTime = defaultUseTime;
        }
    }

    public virtual void use() {
        
    }

    public virtual void discard() {
        Vector3 dPos = transform.position;
        transform.parent = null;
        transform.position = dPos;
        rb.isKinematic = false;
    }

    protected void attack() {
        Vector3 pPos = myPlayer.transform.position;
        Collider[] affectedObjects = Physics.OverlapCapsule(pPos, pPos + myPlayer.lookDirection * attackRange, 0.5f);
        for (int i = 0; i < affectedObjects.Length; i++) {
            GameObject obj = affectedObjects[i].gameObject;
            switch (obj.tag) {
                case "enemy":
                    obj.GetComponent<enemy>().damage(damage); 
                    break;
                case "environmentSwitch":
                    obj.GetComponent<environmentSwitch>().active = true;
                    break;
            }
        }
    }
}
