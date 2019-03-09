using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : environmentalObject
{
    public GameObject[] switches;

    bool open = false;
    bool neverOpened = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
        open = true;
        for (int i = 0; i < switches.Length; i++) {
            if (!switches[i].GetComponent<environmentSwitch>().active) open = false;
        }

        if (open) {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            if (neverOpened) {
                FindObjectOfType<AudioManager>().Play("puzzle");
                neverOpened = false;
            }
        }
        else {
            GetComponent<Renderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
        }
            
        
    }
}
