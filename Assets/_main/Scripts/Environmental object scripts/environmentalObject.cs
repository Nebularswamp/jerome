﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class environmentalObject : MonoBehaviour
{
    public bool conductive = false;
    public bool burnable = false;
    public bool destructible = false;
    public float destroyTime = 3f;
    public bool onFire = false;
    public float defaultFireTime = 0.5f;

    float fireTime;
    bool electrified;

    // Start is called before the first frame update
    void Start()
    {
        fireTime = defaultFireTime;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (onFire) {
            fireTime -= Time.deltaTime;
            if(fireTime <= 0) {
                stimulus.spawn(stimulusType.fire, transform.position, Vector3.zero, 1);
                fireTime = defaultFireTime;
            }
            if (destructible) Destroy(gameObject, destroyTime);
        }
    }
    
    private void FixedUpdate() {
        electrified = false;
    }

    private void OnCollisionStay(Collision col) {
        GameObject obj = col.gameObject;
        switch (obj.tag){
            case "stimulus":
                stimulus cStim = obj.GetComponent<stimulus>();
                if (conductive && cStim.electrified) electrified = true;
                if (onFire && cStim.myType == stimulusType.water) onFire = false;
                else if (burnable && cStim.hot) onFire = true;
                break;

            case "environmentalObject":
                environmentalObject cE = obj.GetComponent<environmentalObject>();
                if (cE.electrified) electrified = true;
                break;
        }
            
    }
        
    
}
