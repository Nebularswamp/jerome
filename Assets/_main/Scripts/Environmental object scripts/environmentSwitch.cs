using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class environmentSwitch : environmentalObject
{

    
    public bool hitSwitch = false;
    public bool electricSwitch = false;
    public bool waterSwitch = false;

    [HideInInspector] public bool active = false;

    Material myMaterial;
    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        myMaterial.color = Random.ColorHSV(0, 1, 0.99f, 1, 0.2f, 0.22f);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (waterSwitch) active = wet;
        if (electricSwitch) active = electrified;
        if (active) {
            float h;
            float s;
            float v;
            Color.RGBToHSV(myMaterial.color, out h, out s, out v);
            v = 0.5f;
            myMaterial.color = Color.HSVToRGB(h, s, v);

        }
    }
}
