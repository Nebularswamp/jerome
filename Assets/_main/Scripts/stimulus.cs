using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stimulus : MonoBehaviour
{
    public enum stimulusType{
        water,
        fire,
        electric
    }

    [HideInInspector] public stimulusType myType;
    [HideInInspector] public Vector3 moveDirection;

    bool electrified = false;
    bool hot = true;

    float lifeTime = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (myType == stimulusType.electric) electrified = true;
        if (myType == stimulusType.water) hot = false;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0) {
            Destroy(gameObject);
        }
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

    public void spawn(stimulusType t, Vector3 dir, float randomRange) {
        float dx = Random.Range(dir.x-randomRange,dir.x+randomRange);
        float dy = Random.Range(dir.y - randomRange, dir.y + randomRange);
        float dz = Random.Range(dir.z - randomRange, dir.z + randomRange);

        GameObject newStimObj = (GameObject)Instantiate(Resources.Load("stimulus"));
        newStimObj.transform.position = transform.position;
        stimulus newStim = newStimObj.GetComponent<stimulus>();
        newStim.myType = t;
        newStim.moveDirection = new Vector3(dx, dy, dz).normalized;
    }
}
