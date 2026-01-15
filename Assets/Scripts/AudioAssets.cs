using UnityEngine;

public class AudioAssets : MonoBehaviour
{
    private static AudioAssets _Instance;

    public static AudioAssets Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = (Instantiate(Resources.Load("AudioAssets")) as GameObject).GetComponent<AudioAssets>();
            }
            return _Instance;
        }
    }

    public AudioClip groundTheme;
    public AudioClip undergroundTheme;
}
