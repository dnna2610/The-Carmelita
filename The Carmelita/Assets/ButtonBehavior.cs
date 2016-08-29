using UnityEngine;
using System.Collections;

public class ButtonBehavior : MonoBehaviour {

    public GameObject door;
    public Sprite openDoorTop;
    public Sprite openDoorBottom;
    public Sprite buttonPressed;
    
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D player){
        DoorBehavior doorControl = door.GetComponent<DoorBehavior>();
        if (Input.GetKey(KeyCode.Space)) {

            // Render button
            SpriteRenderer buttonRenderer = this.GetComponent<SpriteRenderer>();
            buttonRenderer.sprite = buttonPressed;

            // Render Top
            GameObject top = door.transform.Find("Top").gameObject;
            SpriteRenderer topRender = top.GetComponent<SpriteRenderer>();
            topRender.sprite = openDoorTop;

            // Render Bottom
            GameObject bottom = door.transform.Find("Bottom").gameObject;
            SpriteRenderer botRender = bottom.GetComponent<SpriteRenderer>();
            botRender.sprite = openDoorBottom;

            // Unlocking door
            doorControl.isLocked = false;
        }
    }
}
