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
    Color startColor;
    Color activeColor;
    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        startColor = Random.ColorHSV(0, 1, 0.99f, 1, 0.2f, 0.22f);
        activeColor = brightenColor(startColor, 0.3f);
        myMaterial.color = startColor;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (waterSwitch) active = wet;
        if (electricSwitch) active = electrified;
        if (active) myMaterial.color = activeColor;
        else myMaterial.color = startColor;
    }

    private Color brightenColor(Color c, float a) {
        float h;
        float s;
        float v;
        Color.RGBToHSV(c, out h, out s, out v);
        v += a;
        c = Color.HSVToRGB(h, s, v);
        return c;
    }
}
