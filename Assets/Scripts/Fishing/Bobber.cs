using System;
using UnityEngine;

public class Bobber : MonoBehaviour

{
    public GameObject Player;
    void Start()
    {
        Physics.IgnoreCollision(Player.GetComponent<CapsuleCollider>(), GetComponent<Collider>(), true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
        }
        if (collision.gameObject.layer == 9)
        {
            Debug.Log("woah is that a bream in your pocket or  are you just hppy top see me");
            //addsomethingabout it being is trigger
            Pond pond = collision.gameObject.GetComponent<Pond>();
            pond.assignBobber(gameObject);
        }
        if (collision.gameObject.layer != 3 && collision.gameObject.layer != 9)
        {
            {
                  Destroy(gameObject);  
        }
            //
            
        }
                
    }
    
    //void OnCollisionExit(Collision collision)
    //ontriggerenter for fishing when collider has istrigger


    void Update()
    {
        
    }
}
