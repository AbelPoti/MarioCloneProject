using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;
    public AudioClip stompAudio;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if(player.Starpower)
            {
                Hit();
            }
            //If Mario stomped it (fell ON it)
            else if(collision.transform.IsScalarProductGreaterThanNumber(transform, Vector2.down, 0.5f))
            {
                Stomp();
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.gameObject.layer == LayerMask.NameToLayer("KoopaShell"))
        {
            Hit();
        }
    }

    private void Stomp()
    {
        GameController.Instance.PlayAudio(stompAudio);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<Animations>().enabled = false;

        GetComponent<SpriteRenderer>().sprite = flatSprite;
        //We don't destroy it right away because we need to see the dead (flat) goomba sprite
        Destroy(gameObject, 0.5f);
    }

    private void Hit()
    {
        GetComponent<Animations>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        GetComponent<SpriteRenderer>().flipY = true;

        Destroy(gameObject, 3f);
    }
}
