using Unity.VisualScripting;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Vector2 velocity;

    //left because most enemies start by walking to the left
    public Vector2 direction = Vector2.left;
    public float speed = 2f;
    public Vector2 gravity;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
        //Normal gravity seems to be too slow
        gravity.y = -9.81f * 3f;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.Sleep();
    }

    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += gravity.y * Time.fixedDeltaTime;

        //We multiply by fixedDeltaTime again because acceleration is measured in distance / (time)^2
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        //If it hits something, we change it's horizontal direction
        if(rigidbody.Circlecast(direction))
        {
            direction.x = -direction.x;
        }

        //If it is grounded
        if(rigidbody.Circlecast(Vector2.down))
        {   
            //So it does not build up its negative y speed
            velocity.y = Mathf.Max(velocity.y, 0f);
        }
    }
}
