using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    #region public vars
    public float speed = 5f;
    #endregion

    #region local vars
    //Referencing other components
    GameObject myCamera;
    Vector3 lookDirection;
    GameObject myCanvas;

    //item handling
    GameObject[] hands = new GameObject[2];
    bool discardMode = false;
    float pickupRange = 5.0f;

    //inventory
    GameObject[] inventory = new GameObject[4];
    bool invOpen = false;
    GameObject myInventoryDisplay;
    GameObject myInventoryCursor;
    int invAccess = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main.gameObject;
        myCanvas = GameObject.FindGameObjectWithTag("mainCanvas");
        myInventoryDisplay = GameObject.Find("Inventory Display");
        myInventoryCursor = GameObject.Find("Inventory Cursor");

        myInventoryDisplay.SetActive(invOpen);
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

        #region Inventory

        //opening and closing
        if (Input.GetKeyDown(KeyCode.E)) {
            invOpen = !invOpen;
            myInventoryDisplay.SetActive(invOpen);
        }

        if (invOpen) {
            //identify which slot to access
            try { invAccess = int.Parse(Input.inputString) - 1; } catch { } 
            if (invAccess < 0 || invAccess > 3) invAccess = 0;

            //move selection cursor and get selection position
            Vector3 pos = myInventoryCursor.transform.localPosition;
            pos.x = -90 + 60 * invAccess;
            myInventoryCursor.transform.localPosition = pos;
            pos.y = 0;

            //swap items if button clicked
            for (int i = 0; i < 2; i++) {
                if (Input.GetMouseButtonDown(i)) {
                    GameObject invObj = inventory[invAccess];
                    inventory[invAccess] = hands[i];
                    hands[i] = invObj;
                    invObj = inventory[invAccess];

                    //update object positions
                    updateHands(i);
                    if (invObj != null) {
                        invObj.transform.parent = myInventoryDisplay.transform;
                        invObj.transform.localPosition = pos;
                        invObj.transform.localRotation = Quaternion.identity;
                    }
                }
            }
        }

        #endregion

        if (!invOpen) {
            #region Hand action
            discardMode = Input.GetKey(KeyCode.LeftShift);
            for (int i = 0; i < 2; i++) {
                if (Input.GetMouseButtonDown(i)) handAction(i);
            }
            #endregion
        }
        
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
        if (hands[h] != null) {
            hands[h].transform.parent = myCanvas.transform;
            var off = h * 2 - 1;
            hands[h].transform.localPosition = new Vector3(290 * off, -140, 0);
            hands[h].transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 60 * (off * -1)));
        }
    }

}
