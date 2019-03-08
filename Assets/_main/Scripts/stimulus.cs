﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum stimulusType {
    water,
    fire,
    electric
}

public class stimulus : MonoBehaviour
{
    public float speed = 0.5f;

    [HideInInspector] public stimulusType myType;
    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector] public bool hot = true;
    [HideInInspector] public bool electrified = false;


    float lifeTime = 4.0f;
    Material myMaterial;

    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        switch (myType){
            case stimulusType.water:
                hot = false;
                myMaterial.color = new Color(0, 0, 1);
                break;
            case stimulusType.fire:
                hot = true;
                myMaterial.color = new Color(1, 0, 0);
                break;
            case stimulusType.electric:
                hot = true;
                electrified = true;
                myMaterial.color = new Color(1, 1, 0);
                break;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0) {
            Destroy(gameObject);
        }

        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "stimulus") {
            stimulus cStim = col.gameObject.GetComponent<stimulus>();

            if(myType == stimulusType.water) {
                if (cStim.myType == stimulusType.electric) electrified = true;
            }
            if(myType == stimulusType.fire) {
                if (cStim.myType == stimulusType.water) Destroy(gameObject);
            }
        }
    }

    public static void spawn(stimulusType t, Vector3 pos, Vector3 dir, float randomRange) {
        float dx = Random.Range(dir.x-randomRange,dir.x+randomRange);
        float dy = Random.Range(dir.y - randomRange, dir.y + randomRange);
        float dz = Random.Range(dir.z - randomRange, dir.z + randomRange);

        GameObject newStimObj = (GameObject)Instantiate(Resources.Load("stimulus"));
        newStimObj.transform.position = pos;
        stimulus newStim = newStimObj.GetComponent<stimulus>();
        newStim.myType = t;
        newStim.moveDirection = new Vector3(dx, dy, dz).normalized;
    }
}
