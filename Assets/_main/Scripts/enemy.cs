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

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        myController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (hitStun > 0f) {
            hitStun -= Time.deltaTime;
        }
        else {
            Vector3 pPos = myPlayer.transform.position;
            Vector3 pDir = (pPos - transform.position).normalized;
            RaycastHit hit;
            Physics.Raycast(transform.position, pDir, out hit);
            if (hit.transform.tag == "Player") {
                animator.SetBool("Move", true);
                animator.SetBool("Damage", false);
                animator.SetBool("Attacking", false);
                moveDirection = pDir;
                if (Vector3.Distance(pPos, transform.position) < attackRange) {
                    myPlayer.moveDirection = pDir * 5;
                    myPlayer.damage();
                    FindObjectOfType<AudioManager>().Play("enemyslash");
                    animator.SetBool("Attacking", true);
                    animator.SetBool("Damage", false);
                    animator.SetBool("Move", false);
                    hitStun = 0.5f;
                    moveDirection = Vector3.zero;
                }
            }
        }

        moveDirection.y = -1f;
        myController.Move(speed * moveDirection * Time.deltaTime);

        if (hp <= 0) Destroy(gameObject);
    }

    public void damage(int d) {
        FindObjectOfType<AudioManager>().Play("hurtenemy");
        animator.SetBool("Damage", true);
        animator.SetBool("Move", false);
        animator.SetBool("Attacking", false);
        hp -= d;
        hitStun = 0.09f;
        moveDirection = (transform.position - myPlayer.transform.position).normalized * 5;
        moveDirection.y = 0;
    }

    private void OnCollisionEnter(Collision collision) {
        GameObject obj = collision.gameObject;
        if(obj.tag == "stimulus") {
            if (obj.GetComponent<stimulus>().hot) {
                damage(1);
                Destroy(obj);
            }
        }
    }
}
