//using System.Numerics;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    //public GameObject groundedOn;

    private new Rigidbody2D rigidbody;
    //we will access this to influence the camera's scrolling in CameraScrolling.cs
    private new Camera camera;
    private new Collider2D collider;

    private float inputAxis;
    private Vector2 velocity;
    public float moveSpeed = 10f;
    public float maxJumpHeight = 6f;
    public float maxJumpTime = 1f;

    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravityForce => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2);

    public bool Grounded { get; private set; }
    public bool Jumping { get; private set; }
    public bool Running => Mathf.Abs(velocity.x) > 0.1f;
    //Changing direction
    public bool Sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        camera = Camera.main;
    }
    
    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        collider.enabled = true;
        velocity = Vector2.zero;
        Jumping = false;
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        velocity = Vector2.zero;
        Jumping = false;
    }

    //Runs every single frame
    private void Update()
    {
        HorizontalMovement();

        //We check whether there is a collider direclty below Mario
        Grounded = rigidbody.Circlecast(Vector2.down);
        if(Grounded)
        {
            //To prevent negative y velocity building up because of the ApplyGravity function
            velocity.y = Mathf.Max(velocity.y, 0f);
            GroundedMovement();
        }

        ApplyGravity();
    }

    //Runs at a set interval, frame independently
    private void FixedUpdate()
    {
        Vector2 nextPosition = rigidbody.position;
        nextPosition += velocity * Time.deltaTime;
        
        //We do this to limit Mario from exiting the screen to the left (since the screen never moves to the left)
        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        //+- 0.5f because Mario's position is his sprite's center, so some parts of him would get out of screen
        nextPosition.x = Mathf.Clamp(nextPosition.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(nextPosition);
    }

    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        //maxDelta = moveSpeed * Time.deltaTime = how much it can move in a frame
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime * 2);

        //If we touch an object that's to the right/left of us, we set out velocity to zero,
        //else it would keep growing until it reaches the maximum
        //We multiply by our velocity.x to cast either to the left or right (x is negative if going to the left)
        if(rigidbody.Circlecast(Vector2.right * velocity.normalized.x))
        {
            velocity.x = 0f;
        }

        //Rotate Mario's body if he moves to the left
        if(velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void GroundedMovement()
    {
        Jumping = velocity.y > 0f;
        if(Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            Jumping = true;
            GameController.Instance.PlayAudio(GameController.Instance.jumpLowAudio);
        }
    }

    private void ApplyGravity()
    {
        //second term - if we do not hold jump button
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        //If we let go of input, gravity gets stronger, so the sense of jumping is shorter
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravityForce * multiplier * Time.deltaTime;
        //Limit a terminal velocity to falling
        //velocity.y = Mathf.Max(velocity.y, gravityForce / 2f);
    }

    //Enter when Mario collides with something
    //Mario sort of 'sticks' to surfaces if he bonks his head on them, and does not start to fall down
    //We sort this problem out woth this function
    private void OnCollisionEnter2D(Collision2D collision)
    {   
        //if Mario collides with an enemy
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //If he stomps them
            if(transform.IsScalarProductGreaterThanNumber(collision.transform, Vector2.down, 0.5f))
            {
                //Make himb bounce off
                velocity.y = jumpForce / 2f;
                Jumping  = true;
            }
        }
        //check if object hit something from below (to set Mario's y velocity to 0 once he bonks his head)
        //Otherwise he would stick to surfaces
        //We do not consider power-ups in this case
        else if(collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if(transform.IsScalarProductGreaterThanNumber(collision.transform, Vector2.up, Mathf.Sqrt(2)/2))
            {
                velocity.y = 0f;
            }
        }
    }
}
