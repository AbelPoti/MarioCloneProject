using System.Collections;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    void Start()
    {   
        GameController.Instance.PlayAudio(GameController.Instance.powerupAppearsAudio);
        StartCoroutine(Animate());
    }

    //Through linear interpolation of the object, we create an animation of the block item ascending one unit
    private IEnumerator Animate()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        CapsuleCollider2D physicsCollider = GetComponent<CapsuleCollider2D>();
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        rigidbody.isKinematic = true;
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;
        //Turn the spriteRenderer off for a split second so the spawned object does not appear right when it gets 'activated'
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.25f);

        spriteRenderer.enabled = true;

        float elapsed = 0f;
        float duration = 0.5f;

        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = startPosition + Vector3.up;

        while(elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        rigidbody.isKinematic = false;
        physicsCollider.enabled = true;
        triggerCollider.enabled = true;
    }
}
