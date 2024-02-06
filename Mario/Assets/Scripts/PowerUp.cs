using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(other.CompareTag("Player"))
            {
                Collect(other.gameObject);
            }
        }
    }

    private void Collect(GameObject player)
    {
        switch(type)
        {
            case Type.Coin:
                GameController.Instance.AddCoin();
                break;
            case Type.ExtraLife:
                GameController.Instance.AddLife();
                break;
            case Type.MagicMushroom:
                player.GetComponent<Player>().Grow();
                break;
            case Type.Starpower:
                player.GetComponent<Player>().StarPower();
                break;
            default:
                break;
        }
        Destroy(gameObject);
    }
}
