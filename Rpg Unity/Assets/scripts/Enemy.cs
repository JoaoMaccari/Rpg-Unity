using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityStandardAssets.Utility.TimedObjectActivator;

public class Enemy : MonoBehaviour
{


    public float TotalHealt;
    public float CurrentHealt;
    public float AttackDamage;
    public float MovementSpeed;

    public Animator anim;
    public CapsuleCollider cap;

    //controla o campo visual do inimigo
    public float lookRadius = 10f;
    public Transform target;//pega a posição do player
    private NavMeshAgent agent;

    public bool isReady;
    public bool playerIsAlive;

    public float coliderRadius;

    private void Start() {
        cap = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        CurrentHealt = TotalHealt;

        agent = GetComponent<NavMeshAgent>();
        playerIsAlive = true;
       
    }

    public void Update() {

        if (CurrentHealt > 0) {



            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= lookRadius) {

                // agent.isStopped = true;
                //se nao estiver atacando, anda
                if (!anim.GetBool("attacking")) {

                    agent.SetDestination(target.position);//set desination faz ir em direção ao target
                    anim.SetInteger("transition", 2);
                    anim.SetBool("walking", true);
                }



                if (distance <= agent.stoppingDistance) {// a stopDistance é setada no nav mesh agent
                    StartCoroutine(Attack());
                    lookTarget();
                }

                if (distance >= agent.stoppingDistance) {
                    anim.SetBool("attacking", false);
                }


            }
            else {

                anim.SetInteger("transition", 0);
                anim.SetBool("walking", false);
                anim.SetBool("attacking", false);
                //agent.isStopped = false;
            }
        }
    }

    IEnumerator Attack() {

        if (!isReady && playerIsAlive) {

            isReady = true;
            anim.SetBool("attacking", true);
            anim.SetBool("walking", false);
            anim.SetInteger("transition", 1);
            yield return new WaitForSeconds(1f);

            GetEnemy();
            yield return new WaitForSeconds(1.7f);
            isReady = false;

        }

        if (!playerIsAlive) {
            anim.SetInteger("transition", 0);
            anim.SetBool("walking", false);
            anim.SetBool("attacking", false);
            //agent.isStopped = false;
        }
    }

    void GetEnemy() {

        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * coliderRadius), coliderRadius)) {
            if (c.gameObject.CompareTag("Player")) {
                Debug.Log("atacou o player");

                c.gameObject.GetComponent<player>().GetHit(25f);
                playerIsAlive = c.gameObject.GetComponent<player>().isAlive ;
            }
        }
    }

    void lookTarget() {
        //indica a direção que o esqueleto deve andar
        Vector3 direction = (target.position - transform.position).normalized;//normalized faz voltar um valor de no maximo 1
        //indica a rotação que deve fazer para virar pro inimigo
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //passa a informações para de fato alterar a rotacao e direcao do inimigo
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    public void GetHit(float damage) {

        CurrentHealt -= damage;

        if (CurrentHealt > 0) {
            Debug.Log("tomou hit");
            anim.SetInteger("transition", 3);
            StartCoroutine(RecoveryFromHit());

        }
        else {
            anim.SetInteger("transition", 4);
            cap.enabled = false;
        }
        
    }

    IEnumerator RecoveryFromHit() {

        yield return new WaitForSeconds(1f);
        anim.SetInteger("transition", 0);
    } 




    
   
}
