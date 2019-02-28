using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    public float speed = 5f;

    GameObject myCamera;
    Vector3 lookDirection;
    item[] inventory = new item[4];
    item[] hands = new item[2];

    float pickupRange = 3.0f;

    
    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        lookDirection = myCamera.transform.forward;

        #region movement
        float horizontalDirection = Input.GetAxisRaw("Horizontal");
        float verticalDirection = Input.GetAxisRaw("Vertical");

        //vars for moving relative to camera
        Vector3 camR = myCamera.transform.right;
        Vector3 camF = lookDirection;
        camR.y = 0;
        camF.y = 0;
        camR = camR.normalized;
        camF = camF.normalized;

        Vector3 moveDirection = (horizontalDirection * camR + verticalDirection * camF).normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        #endregion

    }

    void handAction(int h) {
        if(hands[h] == null) {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, lookDirection, out hit, pickupRange, 1 << 9)) {
                GameObject pickupItem = hit.transform.gameObject;
                hands[h] = pickupItem.GetComponent<item>();
                Destroy(pickupItem);
            }
        }
    }
}
