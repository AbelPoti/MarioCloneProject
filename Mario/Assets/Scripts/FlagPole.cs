using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
            if(other.CompareTag("Player"))
            {
                StartCoroutine(MoveToAnimation(flag, poleBottom.position));
                
                GameController.Instance.audioSource.Stop();
                GameController.Instance.PlayAudio(GameController.Instance.flagpoleAudio);

                StartCoroutine(LevelCompleteSequence(other.transform));
            }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        yield return MoveToAnimation(player, poleBottom.position);
        
        yield return new WaitForSeconds(1f);
        GameController.Instance.PlayAudio(GameController.Instance.stageClearAudio);

        yield return MoveToAnimation(player, player.position + Vector3.right);
        yield return MoveToAnimation(player, player.position + Vector3.right + Vector3.down);
        yield return MoveToAnimation(player, castle.position);

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(4f);

        GameController.Instance.LoadNextLevel();
    }

    private IEnumerator MoveToAnimation(Transform subject, Vector3 destination)
    {
        const float Rho = 0.125f;
        while(Vector3.Distance(subject.position, destination) > Rho)
        {
            subject.position = Vector3.MoveTowards(subject.position, destination, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = destination;
    }
}
