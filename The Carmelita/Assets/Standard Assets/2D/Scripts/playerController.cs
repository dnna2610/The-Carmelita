using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class playerController : MonoBehaviour
{

    public float moveSpeed;
    public float gravity = -20;
    public float velocityXSmoothing;
    public float accelerationTime;
    public Animator animator;
    public SpriteRenderer render;
    Vector3 velocity;

    Controller2D controller;

    // For door uses
    public float doorDelay;

    // Use this for initialization
    void Start()
    {
        doorDelay = 0f;
        controller = GetComponent<Controller2D>();
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doorDelay >=0 ) doorDelay -= Time.deltaTime;
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);

        velocity.y += gravity * Time.deltaTime;
        if (Mathf.Sign(velocity.x) == -1)
        {
            render.flipX = true;
        }
        else
        {
            render.flipX = false;
        }
        animator.SetFloat("move", Mathf.Abs(velocity.x));

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.Z))
        {
            controller.interact();
            animator.SetBool("check", true);
        }
        else
        {
            animator.SetBool("check", false);
        }

    }
}
