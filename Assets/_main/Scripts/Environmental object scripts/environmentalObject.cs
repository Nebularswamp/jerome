using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class environmentalObject : MonoBehaviour
{
    public bool conductive = false;
    public bool burnable = false;
    public bool waterDestructible = false;
    public bool fireDestructible = false;
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
                Vector3 fireSpawn = RandomPointInBounds(GetComponent<Collider>().bounds);
                stimulus.spawn(stimulusType.fire, fireSpawn, Vector3.zero, 1);
                fireTime = defaultFireTime;
            }
            if (fireDestructible) Destroy(gameObject, destroyTime);
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
                if (cStim.myType == stimulusType.water) {
                    if (waterDestructible) Destroy(gameObject);
                    onFire = false;
                    burnable = false;
                }
                if (burnable && cStim.hot) onFire = true;
                break;

            case "environmentalObject":
                environmentalObject cE = obj.GetComponent<environmentalObject>();
                if (cE.electrified) electrified = true;
                break;
        }
            
    }

    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
