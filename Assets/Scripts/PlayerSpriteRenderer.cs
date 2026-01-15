using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    private PlayerMovement playerMovement;

    public Sprite idle;
    public Sprite jump;
    public Animations run;
    public Sprite slide;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        run.enabled = false;
    }

    private void LateUpdate()
    {
        //We enable the run animation of we are running
        run.enabled = playerMovement.Running;

        //Order is important, we want to override everything with jump
        if(playerMovement.Jumping)
        {
            spriteRenderer.sprite = jump;
        }
        else if(playerMovement.Sliding)
        {
            spriteRenderer.sprite = slide;
        }
        else if(!playerMovement.Running)
        {
            spriteRenderer.sprite = idle;
        }
    }
}
