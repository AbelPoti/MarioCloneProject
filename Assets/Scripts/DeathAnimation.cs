using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite deathSprite;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        UpdateSprite();
        DisablePhysics();
        StartCoroutine(Animate());
    }

    private void UpdateSprite()
    {
        if(deathSprite != null)
        {
            spriteRenderer.sprite = deathSprite;
        }
        spriteRenderer.enabled = true;
        //A larger number than any existing layer, so it gets rendered on top
        spriteRenderer.sortingOrder = 7;
    }

    private void DisablePhysics()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }

        //Will not be controlled by physics engine anymore
        GetComponent<Rigidbody2D>().isKinematic = true;
        //Check for null because this applies to Mario and enemies as well, which have one of the 2 Movement scripts
        var playerMovement = GetComponent<PlayerMovement>();
        var entityMovement = GetComponent<EntityMovement>();
        if(playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        if(entityMovement != null)
        {
            entityMovement.enabled = false;
        }
    }

    private IEnumerator Animate()
    {
        float elapsed = 0f;
        float duration = 3f;

        float jumpVelocity = 10f;
        float gravity = -9.81f * 3;

        Vector3 velocity =  Vector3.up * jumpVelocity;

        while(elapsed < duration)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
