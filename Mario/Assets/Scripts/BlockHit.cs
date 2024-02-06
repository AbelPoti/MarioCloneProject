using System.Collections;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject itemInside;
    //Either empty mystery box or broken brick
    public Sprite brokenSprite;
    //default -1 means it can be hit infinitely
    public int maxHits = -1;
    private bool animating;

    public AudioClip bumpAudio;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if Mario hit it
        if(!animating && collision.gameObject.CompareTag("Player"))
        {
            //from below
            if(collision.transform.IsScalarProductGreaterThanNumber(transform, Vector2.up, 0.5f))
            {
                if(maxHits != 0)
                {
                    Hit();
                }
                else
                {
                    GameController.Instance.PlayAudio(bumpAudio);
                }
            }
        }
    }

    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //If we have a hidden block
        spriteRenderer.enabled = true;
        if(--maxHits == 0)
        {
            spriteRenderer.sprite = brokenSprite;
        }

        //If there is an item inside the block, we spawn it
        if(itemInside != null)
        {
            Instantiate(itemInside, transform.position, Quaternion.identity);
        }

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        animating = true;

        Vector3 normalPosition = transform.localPosition;
        Vector3 animatedPosition = normalPosition + Vector3.up * 0.5f;

        yield return Move(normalPosition, animatedPosition);
        yield return Move(animatedPosition, normalPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.125f;

        while(elapsed < duration)
        {
            //Gradually moving the sprite up using linear interpolation
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = to;
    }
}
