using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{

    const float skinWidth = .015f;

    public LayerMask collisionMask;
    public int horizontalRayCount = 4;
    public int verticleRayCount = 4;
    public float maxClimbAngle;


    float horizontalRaySpacing;
    float verticleRaySpacing;

    private BoxCollider2D collider;
    private raycastOrigins origins;
    public CollisionInfo collisions;

    // Use this for initialization
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        calcRaySpacing();
    }


    void HorizontalCollision(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin;

            if (directionX == -1)
            {
                rayOrigin = origins.bottomLeft;
            }
            else
            {
                rayOrigin = origins.bottomRight;
            }

            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);


            if (hit)
            {

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.prevSlopeAngle)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    climbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticleCollision(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticleRayCount; i++)
        {
            Vector2 rayOrigin;

            if (directionY == -1)
            {
                rayOrigin = origins.bottomLeft;
            }
            else
            {
                rayOrigin = origins.topLeft;
            }

            rayOrigin += Vector2.right * (verticleRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    public void Move(Vector3 velocity)
    {
        UpdateOrigins();
        collisions.reset();
        if (velocity.x != 0)
        {
            HorizontalCollision(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticleCollision(ref velocity);
        }


        transform.Translate(velocity);

    }

    void UpdateOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);
        origins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        origins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        origins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        origins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void climbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        velocity.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
        collisions.climbingSlope = true;
        collisions.slopeAngle = slopeAngle;
    }

    void calcRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticleRayCount = Mathf.Clamp(verticleRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticleRaySpacing = bounds.size.x / (verticleRayCount - 1);
    }

    public void interact()
    {
        // code here
    }

    // Store collide info

    struct raycastOrigins
    {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below, left, right, climbingSlope;

        public float slopeAngle, prevSlopeAngle;

        public void reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;

            prevSlopeAngle = slopeAngle;
            slopeAngle = 0;

        }
    }
}
