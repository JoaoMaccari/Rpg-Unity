using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float speed;
    public float rotSpeed;
    public float rotation;
    public float gravity;

    Vector3 moveDirection;

    CharacterController controler;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        controler = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    void move() {

        if (Input.GetKey(KeyCode.W)) {

            moveDirection = Vector3.forward * speed;
            moveDirection = transform.TransformDirection(moveDirection);
        }


        if (Input.GetKeyUp(KeyCode.W)) {
            moveDirection = Vector3.zero;
        }


        rotation += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rotation, 0);

        moveDirection.y = gravity * Time.deltaTime;
        controler.Move(moveDirection * Time.deltaTime) ;
    }

   
}
