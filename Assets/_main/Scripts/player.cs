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
    bool discardMode = false;
    float pickupRange = 5.0f;

    
    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main.gameObject;
        myCanvas = GameObject.FindGameObjectWithTag("mainCanvas");
       // Cursor.lockState = CursorLockMode.Locked; 
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

        #region Hand action
        discardMode = Input.GetKey(KeyCode.LeftShift);
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
                pickupItem.GetComponent<Rigidbody>().isKinematic = true;
                updateHands(h);
            }
        }
        else {
            item handItem = hands[h].GetComponent<item>();
            if (discardMode) {
                handItem.discard();
                hands[h] = null;
            }
            else handItem.use();
        }
    }

    void updateHands(int h) {
        hands[h].transform.parent = myCanvas.transform;
        var off = h * 2 - 1;
        hands[h].transform.localPosition = new Vector3(290 * off, -140, 0);
        hands[h].transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 60*(off*-1)));
    }
}
