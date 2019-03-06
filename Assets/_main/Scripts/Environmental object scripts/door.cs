using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : environmentalObject
{
    public GameObject[] switches;

    bool open = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
        bool check = true;
        for (int i = 0; i < switches.Length; i++) {
            if (!switches[i].GetComponent<environmentSwitch>().active) check = false;
        }

        if (check) {
            Destroy(gameObject);
        }
    }
}
