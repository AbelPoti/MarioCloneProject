using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    //To help keep track of which renderer we actually used, helps at the Animation
    private PlayerSpriteRenderer activeRenderer;

    public DeathAnimation deathAnimation;
    private CapsuleCollider2D capsuleCollider;

    public bool Big => bigRenderer.enabled;
    public bool Dead => deathAnimation.enabled;
    public bool Starpower { get; private set; }

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
    }
    public void Hit()
    {
        if(!Dead && !Starpower)
        {
            if(Big)
            {
                Shrink();
            }
            else
            {
                Death();
            }
        }
    }

    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        //Set the collider to a larger fitting size
        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        GameController.Instance.PlayAudio(GameController.Instance.powerupCollectedAudio);

        StartCoroutine(GrowOrShrinkAnimation());
    }

    private void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        //Set the collider to a smaller fitting size
        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        StartCoroutine(GrowOrShrinkAnimation());

    }

    public void StarPower()
    {
        Starpower = true;
        GameController.Instance.PlayAudio(GameController.Instance.powerupCollectedAudio);
        StartCoroutine(StarPowerAnimation());
    }

    private void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameController.Instance.audioSource.Stop();
        GameController.Instance.PlayAudio(GameController.Instance.marioDiesAudio);

        GameController.Instance.ResetLevel(4f);
    }

    private IEnumerator GrowOrShrinkAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        //Disable the collider in case we shrink, for the time of the animation, so it cannot happen that we
        //hit two enemies at the same time and we die instantly (frustrating game experience)
        //if(activeRenderer == bigRenderer)
        //{
        //    capsuleCollider.enabled = false;
        //}

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            //Every 4 frames
            if(Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }
                yield return null;
        }
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
        capsuleCollider.enabled = true;
    }

    private IEnumerator StarPowerAnimation()
    {
        Starpower = true;
        float elapsed = 0f;
        float duration = 10f;
        
        GameController.Instance.audioSource.Stop();
        GameController.Instance.PlayAudio(GameController.Instance.starPowerAudio);

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            //Every 4 frames
            if(Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }
        activeRenderer.spriteRenderer.color = Color.white;
        Starpower = false;

        GameController.Instance.audioSource.Stop();
        GameController.Instance.PlayAudio(GameController.Instance.groundThemeAudio);
    }
}
