using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            
            GameController.Instance.audioSource.Stop();
            GameController.Instance.ResetLevel(4f);
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
