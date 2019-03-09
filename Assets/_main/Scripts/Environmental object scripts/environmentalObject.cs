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
    protected bool electrified = false;
    protected bool wet = false;


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
        environmentCollisions();
    }


    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public static float boundsRadius(Bounds b) {
        Vector3 bEx = b.extents;
        return Mathf.Max(bEx.x, bEx.y, bEx.z);

    }

    public void environmentCollisions() {
        Bounds b = GetComponent<Collider>().bounds;
        float r = boundsRadius(b);
        Collider[] col = Physics.OverlapSphere(b.center, r);
        foreach(Collider c in col){
            GameObject obj = c.gameObject;
            if (obj == gameObject) continue;
            switch (obj.tag) {
                case "stimulus":
                    stimulus cStim = obj.GetComponent<stimulus>();
                    if (conductive && cStim.electrified) electrified = true;
                    if (cStim.myType == stimulusType.water) {
                        if (waterDestructible) Destroy(gameObject);
                        onFire = false;
                        wet = true;
                    }
                    else wet = false;
                    if (burnable && cStim.hot && !wet) onFire = true;
                    break;

                case "environmentalObject":
                    environmentalObject cE = obj.GetComponent<environmentalObject>();
                    if (cE.electrified) electrified = true;
                    break;
            }
        }
    }
}
