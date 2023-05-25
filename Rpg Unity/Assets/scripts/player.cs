using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class player : MonoBehaviour {
    public float speed;
    public float rotSpeed;
    private float rotation;
    public float gravity;

    public float TotalHealt = 100;
    public float CurrentHealt;

    Vector3 moveDirection;

    CharacterController controler;
    Animator anim;

    public bool isReady;
    public bool isAlive;
    public float enemyDamage = 25f;

    //armazena todos os inimigos que tomar hit na lista
    List<Transform> Enemies = new List<Transform>();
    public float coliderRadius;
    // Start is called before the first frame update


    void Start() {
        controler = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        CurrentHealt = TotalHealt;
        isAlive = true;
    }

    // Update is called once per frame
    void Update() {
        move();
        GetMouseInput();
    }

    void move() {

        if (controler.isGrounded) {
            //Debug.Log("tocou o chao");
            if (Input.GetKey(KeyCode.W)) {

                if (!anim.GetBool("attacking")) {

                    anim.SetBool("walking", true);
                    anim.SetInteger("transition", 1);
                    moveDirection = Vector3.forward * speed;
                    moveDirection = transform.TransformDirection(moveDirection);
                }
                else {
                    anim.SetBool("walking", false);

                    moveDirection = Vector3.zero;
                    StartCoroutine(Attack(1));
                }

            }
            else  {
                anim.SetBool("walking", false);
                anim.SetInteger("transition", 0);
                moveDirection = Vector3.zero;
               // StartCoroutine(Attack(1));
            }


            /* if (Input.GetKeyUp(KeyCode.W)) {

                 anim.SetBool("walking", false);
                 anim.SetInteger("transition", 0);
                 moveDirection = Vector3.zero;
             }*/
        }



        rotation += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rotation, 0);

        moveDirection.y -= gravity * Time.deltaTime;
        controler.Move(moveDirection * Time.deltaTime);
    }

    void GetMouseInput() {

        if (controler.isGrounded) {

            if (Input.GetMouseButtonDown(0)) {

                if (anim.GetBool("walking")) {

                    anim.SetBool("walking", false);
                    anim.SetInteger("transition", 0);
                }

                if (!anim.GetBool("walking")) {
                    StartCoroutine(Attack(0));
                }
            }
        }
    }

    IEnumerator Attack(int transitionValue) {

        if (!isReady) {

            isReady = true;
            anim.SetBool("attacking", true);
            anim.SetInteger("transition", 2);
            yield return new WaitForSeconds(0.5f);

            GetEnemyRange();

            

            foreach (Transform enemies in Enemies) {
                //animação ação dano inimigo

                Debug.Log(Enemies.Count);
                //crio um obj local que recebe que vai receber os inimigos que estão na lista
                Enemy enemy = enemies.GetComponent<Enemy>();

               // Debug.Log(enemy);

                if (enemy != null) {
                    //Debug.Log("dando hit");
                    enemy.GetHit(enemyDamage);
                }
            }

            yield return new WaitForSeconds(1.3f);

            anim.SetInteger("transition", transitionValue);
            anim.SetBool("attacking", false);
            isReady = false;

        }
    }

    void GetEnemyRange(){

        Enemies.Clear();
        //vai percorrer tudo o que o personagem está batendo e vai filtrar pra ver o que é inimigo
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * coliderRadius), coliderRadius))
        {
            if (c.gameObject.CompareTag("Enemy")) {
               // Debug.Log("adicionou na lista");
                Enemies.Add(c.transform);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;    
        Gizmos.DrawSphere(transform.position + transform.forward , coliderRadius);        
    }


    public void GetHit(float damage) {

        CurrentHealt -= damage;

        if (CurrentHealt > 0) {

            //toma hit
            Debug.Log("tomou hit");
            anim.SetInteger("transition", 3);
            StartCoroutine(RecoveryFromHit());

        }
        else {
            //morre
            anim.SetInteger("transition", 4);
            isAlive = false;
            
        }

    }

    IEnumerator RecoveryFromHit() {

        yield return new WaitForSeconds(1.1f);
        anim.SetInteger("transition", 0);
    }


}
