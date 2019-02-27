using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour //moves character relative 
{

    public float speed = 5f;

    GameObject myCamera;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalDirection = Input.GetAxisRaw("Horizontal");
        float verticalDirection = Input.GetAxisRaw("Vertical");

        //vars for moving relative to camera
        Vector3 camR = myCamera.transform.right;
        Vector3 camF = myCamera.transform.forward;
        camR.y = 0;
        camF.y = 0;
        camR = camR.normalized;
        camF = camF.normalized;

        Vector3 moveDirection = (horizontalDirection * camR + verticalDirection * camF).normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

    }
}
