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
    public Transform target;//pega a posi��o do player
    private NavMeshAgent agent;

    public bool isReady;

    private void Start() {
        cap = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        CurrentHealt = TotalHealt;

        agent = GetComponent<NavMeshAgent>();
       
    }

    public void Update() {
       
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= lookRadius) {

           // agent.isStopped = true;
            //se nao estiver atacando, anda
            if (!anim.GetBool("attacking")) {

                agent.SetDestination(target.position);//set desination faz ir em dire��o ao target
                anim.SetInteger("transition", 2);
                anim.SetBool("walking", true);
            }



            if (distance <= agent.stoppingDistance) {// a stopDistance � setada no nav mesh agent
                StartCoroutine(Attack());
                lookTarget();
            }


        }
        else {

            anim.SetInteger("transition", 0);
            anim.SetBool("walking", false);
            anim.SetBool("attacking", false);
            //agent.isStopped = false;
        }
    }

    IEnumerator Attack() {

        if (!isReady) {

            isReady = true;
            anim.SetBool("attacking", true);
            anim.SetBool("walking", false);
            anim.SetInteger("transition", 1);
            yield return new WaitForSeconds(1f);

            
            isReady = false;

        }
    }

    void lookTarget() {
        //indica a dire��o que o esqueleto deve andar
        Vector3 direction = (target.position - transform.position).normalized;//normalized faz voltar um valor de no maximo 1
        //indica a rota��o que deve fazer para virar pro inimigo
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //passa a informa��es para de fato alterar a rotacao e direcao do inimigo
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
            Die();
        }
        
    }

    IEnumerator RecoveryFromHit() {

        yield return new WaitForSeconds(1f);
        anim.SetInteger("transition", 0);
    } 


    void Die() {

        if (CurrentHealt <= 0) {

            anim.SetInteger("transition", 4);
            cap.enabled = false;
        }

        
    }

    
   
}
