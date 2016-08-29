using UnityEngine;
using System.Collections;

public class DogBehavior : MonoBehaviour {

    private enum STATE { SLEEP, AWAKE, WANDER }

    public Animator anim;
    public LayerMask willWake;
    public float speed;
    public AudioSource dogSound;

    public float wakeDist;

    private STATE currentState;


	// Use this for initialization
	void Start () {
        dogSound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        EnterStateSleep();
	}
	
	// Update is called once per frame
	void Update () {
        switch (currentState){
            case STATE.SLEEP:
                UpdateSleep();
                break;

            case STATE.AWAKE:
                UpdateAwake();
                break;

            case STATE.WANDER:
                UpdateWander();
                break;
        }
	}

    private void EnterStateSleep(){
        currentState = STATE.SLEEP;
        anim.SetTrigger("Sleep");
    }

    private void UpdateSleep(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, wakeDist, willWake.value);
        Debug.DrawRay(transform.position, Vector3.left, Color.red, 20, true);
        if (hit.collider != null){
            EnterStateAwake();
        }
    }

    private void EnterStateAwake(){
        currentState = STATE.AWAKE;
        anim.SetTrigger("Wake");
        dogSound.Play();
    }

    private void UpdateAwake(){
        // Need Trigger
    }

    private void CheckIfTriggerPressed(){
        // Check trigger
    }

    private void EnterStateWander(){
        currentState = STATE.WANDER;
        anim.SetTrigger("Run");
        // Run to another room
    }

    private void UpdateWander(){
        // Check if reached destination, and SLEEP
        EnterStateSleep();
    }
}
