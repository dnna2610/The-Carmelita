using UnityEngine;
using System.Collections;

public class DogBehavior : MonoBehaviour {

    public float speed;
    public int direction;
    public float timeMove;
    private float time;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += Vector3.right * speed * direction;
        time -= Time.deltaTime;
        if (time < 0 ) {
            time = timeMove;
            direction = 0 - direction;
        }
	}
}
