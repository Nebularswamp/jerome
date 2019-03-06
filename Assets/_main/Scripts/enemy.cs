using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    public float speed = 5f;
    public float attackRange = 0.5f;
    public int hp = 3;

    player myPlayer;
    Vector3 moveDirection;
    float hitStun = 0f;
    CharacterController myController;
    // Start is called before the first frame update
    void Start()
    {
        myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        myController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hitStun > 0f) {
            hitStun -= Time.deltaTime;
        }
        else {
            Vector3 pPos = myPlayer.transform.position;
            moveDirection = (pPos - transform.position).normalized;
            if(Vector3.Distance(pPos, transform.position) < attackRange){
                myPlayer.moveDirection = (pPos - transform.position).normalized * 5;
                myPlayer.damage();
                hitStun = 0.5f;
                moveDirection = Vector3.zero;
            }
        }
        moveDirection.y = -1f;
        myController.Move(speed * moveDirection * Time.deltaTime);

        if (hp <= 0) Destroy(gameObject);
    }

    public void damage(int d) {
        hp -= d;
        hitStun = 0.09f;
        moveDirection = (transform.position - myPlayer.transform.position).normalized * 5;
        moveDirection.y = 0;
    }
}
