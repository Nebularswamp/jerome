using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpsCam : MonoBehaviour {

    public float defSensitivity = 100f; //stores default sensitivity to reset true sensitivity if needed
    public bool lockCam; //in case you don't want the camera to move

    float sensitivity;
    Vector3 rot; //used to save current rotation for easy modification

    // Use this for initialization
    void Start() {
        rot = transform.rotation.eulerAngles;
        sensitivity = defSensitivity;
    }

    // Update is called once per frame
    void Update() {

        //input
        float rX = Input.GetAxis("Look Vertical");
        float rY = Input.GetAxis("Look Horizontal");


        if (!lockCam) rot += new Vector3(rX, rY * 16/9, 0) * sensitivity * Time.deltaTime; 
        rot.x = Mathf.Clamp(rot.x, -90, 90); //clamps up-down camera rotation (no backflips allowed)
        transform.rotation = Quaternion.Euler(rot); //apply rotation

    }
}
