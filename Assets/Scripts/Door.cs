using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isEnabled = true;
    public String whichScene;
    public String whichDoorGoingTo;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" && isEnabled)
        {
            GameObject.Find("SceneTransitionController").GetComponent<SceneTransitionController>().transitionDoor(whichDoorGoingTo, whichScene);
            isEnabled = false;
        }
        else
        { 

        }

    }

    void OnCollisionExit(Collision collision)
    {
        isEnabled = true;
    }
}
