using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityStandardAssets.Utility.TimedObjectActivator;

public class Chest : MonoBehaviour
{
    public Animator anim;
    public float ColliderRadius;
    public bool IsOpened;
    public float coliderRadius;

    public List<Item> Items = new List<Item>(); 
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        Getplayer();
    }

    void Getplayer() {

        if (!IsOpened) {

            foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * coliderRadius), coliderRadius)) {
                if (c.gameObject.CompareTag("Player")) {

                    if (Input.GetMouseButtonDown(0)) {
                        openChest();

                    }
                }
            }
        }

    }

    void openChest() {

        foreach (Item i in Items) {
            i.GetAction();
        }

        anim.SetTrigger("Open");
        IsOpened = true;
    }
}
