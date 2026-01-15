using UnityEngine;
using UnityEngine.Playables;

public class Animations : MonoBehaviour
{
    public Sprite[] sprites;
    public float framerate = 1f / 10f;

    private SpriteRenderer spriteRenderer;
    private int frame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(Animate), framerate, framerate);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }


    //Cycle through sprites
    private void Animate()
    {
        ++frame;
        if(frame >= sprites.Length)
        {
            frame = 0;
        }
        spriteRenderer.sprite = sprites[frame];
    }
}
