using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puddle : environmentalObject
{

    public float defaultWaterTime = 0.5f;

    float waterTime;

    private void Start() {
        waterTime = defaultWaterTime;   
    }

    public override void Update() {
        base.Update();
        if (waterTime > 0) waterTime -= Time.deltaTime;
        else {
            Vector3 waterSpawn = RandomPointInBounds(GetComponent<Collider>().bounds);
            stimulus.spawn(stimulusType.water, waterSpawn, Vector3.up, 0.3f);
            waterTime = defaultWaterTime;
        }
    }
}
