using UnityEngine;
using System.Collections;

public class DoorBehavior : MonoBehaviour
{

    public GameObject exitDoor;
    private playerController control;
    public bool isLocked;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other){
        control = other.gameObject.GetComponent<playerController>();
        if ((!isLocked) && (control.doorDelay <= 0)) {
            if (Input.GetKey(KeyCode.Space)){
                other.gameObject.transform.position = exitDoor.transform.position;
                control.doorDelay = 0.5f;
            }
        }
    }
}
