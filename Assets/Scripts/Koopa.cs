using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;

    private bool inShell;
    private bool shellMoving;

    public float shellSpeed = 12f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!inShell && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            
            if(player.Starpower)
            {
                Hit();
            }
            //If Mario stomped it (fell ON it)
            else if(collision.transform.IsScalarProductGreaterThanNumber(transform, Vector2.down, 0.35f))
            {
                EnterShell();
            }
            else
            {
                player.Hit();
            }
        }
    }

    //Mario hit the trigger which activates the shell
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(inShell && other.CompareTag("Player"))
        {
            if(!shellMoving)
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                if(direction.x == 0f)
                {
                    direction.x = 1f;
                }
                PushShell(direction);
            }
            //If we hit a moving shell, we die
            else
            {
                Player player = other.gameObject.GetComponent<Player>();

                if(player.Starpower)
                {
                    Hit();
                }
                else
                {
                    player.Hit();
                }
               
            }
        }
        else if (!inShell && other.gameObject.layer == LayerMask.NameToLayer("KoopaShell"))
        {
            Hit();
        }
    }

    private void EnterShell()
    {
        inShell = true;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<Animations>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
    }

    private void PushShell(Vector2 direction)
    {
        shellMoving = true;
        GetComponent<Rigidbody2D>().isKinematic = false;

        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        //Setting a new layer so we can define new collision rules (koopa shells can collide with anything
        //whereas if we kept the enemy tag on the shelled koopa, it would not collide with enemies, since
        //we set enemy - enemy collision to false)
        gameObject.layer = LayerMask.NameToLayer("KoopaShell");
    }

    private void Hit()
    {
        GetComponent<Animations>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;

        Destroy(gameObject, 3f);
    }

    private void OnBecameInvisible()
    {
        if(shellMoving)
        {
            Destroy(gameObject);
        }
    }
}
