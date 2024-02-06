using System.Collections;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        GameController.Instance.AddCoin();
        
        Vector3 normalPosition = transform.localPosition;
        Vector3 animatedPosition = normalPosition + Vector3.up * 2f;

        yield return Move(normalPosition, animatedPosition);
        yield return Move(animatedPosition, normalPosition);

        Destroy(gameObject);
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.25f;

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
