using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    //Concrete location of where Mario will exit (where the piipe leads)
    public Transform connection;
    public KeyCode enterKeyCode = KeyCode.S;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;

    private void OnTriggerStay2D(Collider2D other)
    {
        //Check if the pipe Mario wants to enter leads somewhere
        if(connection != null && other.CompareTag("Player"))
        {
            if(Input.GetKeyDown(enterKeyCode))
            {
                StartCoroutine(Enter(other.transform));
            }
        }
    }

    private IEnumerator Enter(Transform player)
    {
        GameController.Instance.audioSource.Stop();
        GameController.Instance.PlayAudio(GameController.Instance.pipeAudio);

        player.GetComponent<PlayerMovement>().enabled = false;
        
        Vector3 enteredPosition = transform.position + enterDirection;
        //Scale Mario down so he does fit visually into the pipe (Big version might overlap)
        Vector3 enteredScale = Vector3.one * 0.5f;

        yield return Move(player, enteredPosition, enteredScale);
        yield return new WaitForSeconds(3f);

        bool underground = connection.position.y < 0f;
        Camera.main.GetComponent<CameraScrolling>().SetUnderground(underground);

        //If we exit through a pipe, not just a specific spot on the map
        if(exitDirection != Vector3.zero)
        {
            GameController.Instance.audioSource.Stop();
            GameController.Instance.PlayAudio(GameController.Instance.pipeAudio);
            //Then we do the animations reversed
            player.position = connection.position - exitDirection;
            yield return Move(player, connection.position + exitDirection, Vector3.one);
        }
        //Just a specific spot, move Mario there and reset his size
        else
        {
            player.position = connection.position;
            player.localScale = Vector3.one;
        }
        
        if(underground)
        {
            GameController.Instance.PlayAudio(GameController.Instance.undergroundThemeAudio);
        }
        else
        {
            GameController.Instance.PlayAudio(GameController.Instance.groundThemeAudio);
        }

        player.GetComponent<PlayerMovement>().enabled = true;
    }

    private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale)
    {
        float elapsed = 0f;
        float duration = 1f;

        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale;

        while(elapsed < duration)
        {
            float ratio = elapsed / duration;

            player.position = Vector3.Lerp(startPosition, endPosition, ratio);
            player.localScale = Vector3.Lerp(startScale, endScale, ratio);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
        player.localScale = endScale;
    }
}
