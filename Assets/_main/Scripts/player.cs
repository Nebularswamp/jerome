using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class player : MonoBehaviour
{
    #region public vars
    public float speed = 5f;
    public craftingList cl;
    public int maxHp = 5;
    public float pickupRange = 5.0f;
    public float defaultStepTime = 0.3f;

    [HideInInspector] public Vector3 lookDirection;
    [HideInInspector] public Vector3 moveDirection = Vector3.zero;
    #endregion

    #region local vars
    //Referencing other components
    GameObject myCamera;
    GameObject myCanvas;
    CharacterController myController;

    //item handling
    GameObject[] hands = new GameObject[2];
    bool discardMode = false;

    //inventory
    GameObject[] inventory = new GameObject[4];
    bool invOpen = false;
    GameObject myInventoryDisplay;
    GameObject myInventoryCursor;
    int invAccess = 0;

    //crafting
    Dictionary<List<string>, GameObject> myCraftingList = new Dictionary<List<string>, GameObject>();

    //Stats
    int hp;
    bool dead = false;

    //Movement and collision
    float hitStun = 0f;

    //sound
    float stepTime;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main.gameObject;
        myCanvas = GameObject.FindGameObjectWithTag("mainCanvas");
        myInventoryDisplay = GameObject.Find("Inventory Display");
        myInventoryCursor = GameObject.Find("Inventory Cursor");

        myInventoryDisplay.SetActive(invOpen);
        myController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;

        //movement
        Physics.IgnoreLayerCollision(10, 9);
        Physics.IgnoreLayerCollision(10, 11);

        //Set stats
        hp = maxHp;
        stepTime = defaultStepTime;
        myCanvas.transform.Find("HP Display").gameObject.GetComponent<Text>().text = "HP: " + hp.ToString();

        //convert crafting list into convenient data structure
        foreach (craftingRecipe i in cl.craftList) {
            List<string> cr = new List<string>();
            for (int j = 0; j < 2; j++) {
                cr.Add(i.ingredients[j].GetComponent<item>().itemName);
            }
            myCraftingList.Add(cr, i.result);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead) {
            lookDirection = myCamera.transform.forward;

            #region Damage and death
            if (hp <= 0) {
                dead = true;
                myCanvas.transform.Find("Death Message").gameObject.SetActive(true);
            }
            #endregion

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


            if (hitStun <= 0) {
                moveDirection = (horizontalDirection * camR + verticalDirection * camF).normalized; //get move direction
                if (moveDirection != Vector3.zero) { //play footstep noises
                    stepTime -= Time.deltaTime;
                    if (stepTime <= 0) {
                        FindObjectOfType<AudioManager>().Play("footstep");
                        stepTime = defaultStepTime;
                    }
                }
                else stepTime = 0;

                moveDirection.y = -1f; //gravity
            }
            else {
                hitStun -= Time.deltaTime;
            }


            myController.Move(moveDirection * speed * Time.deltaTime);
            #endregion

            #region Inventory

            //opening and closing
            if (Input.GetKeyDown(KeyCode.E)) {
                bool checkUse = false;
                foreach (GameObject i in hands) {
                    if(i != null) {
                        item it = i.GetComponent<item>();
                        if (it.useTime > 0 || it.inUse) checkUse = true;
                    }
                }
                if (!checkUse) {
                    invOpen = !invOpen;
                    FindObjectOfType<AudioManager>().Play("bagopen");
                    myInventoryDisplay.SetActive(invOpen);
                }
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
                            invObj.transform.SetParent(myInventoryDisplay.transform);
                            invObj.transform.localPosition = pos;
                            invObj.transform.localRotation = Quaternion.identity;
                            invObj.transform.localScale = new Vector3(100, 100, 100) / invObj.GetComponent<Renderer>().bounds.size.magnitude;
                        }
                    }
                }
            }

            #endregion

            #region Crafting
            if (Input.GetKeyDown(KeyCode.Space) && !hands.Any(i => i == null)) {
                List<string> craftKey = null;
                foreach (List<string> l in myCraftingList.Keys) {
                    if (l.Contains(hands[0].GetComponent<item>().itemName) && l.Contains(hands[1].GetComponent<item>().itemName)) {
                        FindObjectOfType<AudioManager>().Play("crafting");
                        craftKey = l;
                        break;
                    }
                }
                if (craftKey != null) {
                    GameObject craftObject = myCraftingList[craftKey];
                    for (int i = 0; i < hands.Length; i++) {
                        Destroy(hands[i]);
                        hands[i] = null;
                        updateHands(i);
                    }
                    craftObject = Instantiate(craftObject);
                    hands[1] = craftObject;
                    updateHands(1);
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
        else {
            if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }



    void handAction(int h) {
        if(hands[h] == null) {
            Collider[] cItems = Physics.OverlapCapsule(transform.position, transform.position + lookDirection * pickupRange, 0.3f);
            foreach (Collider col in cItems) {
                if(col.transform.tag == "item") {
                    GameObject pickupItem = col.transform.gameObject;
                    if (!pickupItem.GetComponent<Rigidbody>().isKinematic) {
                        hands[h] = pickupItem;
                        updateHands(h);
                        break;
                    }
                }
            }
                
        }
        else {
            item handItem = hands[h].GetComponent<item>();
            if (discardMode) {
                handItem.discard();
                hands[h] = null;
                updateHands(h);
            }
            else if (handItem.continuous) {
                handItem.inUse = !handItem.inUse;
            }
            else if(handItem.useTime <= 0){
                handItem.use();
                handItem.useTime = handItem.defaultUseTime;
            }
        }
    }

    public void updateHands(int h) {
        Text hText = GameObject.Find("UI canvas/Item Name " + h.ToString()).GetComponent<Text>();
        if (hands[h] != null) {
            item hItem = hands[h].GetComponent<item>(); //get item

            //ui
            hands[h].transform.SetParent(myCanvas.transform);
            if (hItem.uiScale != Vector3.zero) hands[h].transform.localScale = hItem.uiScale;
            else hItem.uiScale = hands[h].transform.localScale;
            var off = h * 2 - 1;
            hands[h].transform.localPosition = new Vector3(290 * off, -120, 0);
            hands[h].transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 60 * (off * -1)));
            hText.text = hItem.itemName;
            //physics
            hands[h].GetComponent<Rigidbody>().isKinematic = true;
        }
        else hText.text = "";
    }

    public void damage() {
        FindObjectOfType<AudioManager>().Play("hurtplayer");
        hitStun = 0.09f;
        moveDirection.y = 0;
        hp -= 1;
        myCanvas.transform.Find("HP Display").gameObject.GetComponent<Text>().text = "HP: " + hp.ToString();
    }
}
