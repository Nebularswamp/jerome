using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{

    public float speed = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalDirection = Input.GetAxisRaw("Horizontal");
        float verticalDirection = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(horizontalDirection, 0, verticalDirection).normalized;

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

    }
}
