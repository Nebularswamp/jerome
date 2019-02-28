using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    public float speed = 5f;

    GameObject myCamera;
    GameObject myCanvas;
    Vector3 lookDirection;
    item[] inventory = new item[4];
    GameObject[] hands = new GameObject[2];

    float pickupRange = 5.0f;

    
    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main.gameObject;
        myCanvas = GameObject.FindGameObjectWithTag("mainCanvas");
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

        #region Item pickup
        for (int i = 0; i < 2; i++) {
            if (Input.GetMouseButtonDown(i)) handAction(i);
        }
        #endregion

    }

    void handAction(int h) {
        if(hands[h] == null) {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, lookDirection, out hit, pickupRange, 1 << 9)) {
                GameObject pickupItem = hit.transform.gameObject;
                hands[h] = pickupItem;
                updateHands(h);
            }
        }
    }

    void updateHands(int h) {
        hands[h].transform.parent = myCanvas.transform;
        hands[h].transform.localPosition = new Vector3(-140 + 340 * h, 35, 90);
        hands[h].transform.localRotation = Quaternion.Euler(new Vector3(180, 30, 240));
    }
}
