using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public AudioSource audioSource;
    public AudioClip groundThemeAudio;
    public AudioClip undergroundThemeAudio;
    public AudioClip coinAudio;
    public AudioClip oneUpAudio;
    public AudioClip starPowerAudio;
    public AudioClip flagpoleAudio;
    public AudioClip pipeAudio;
    public AudioClip powerupCollectedAudio;
    public AudioClip powerupAppearsAudio;
    public AudioClip jumpLowAudio;
    public AudioClip jumpHighAudio;
    public AudioClip stageClearAudio;
    public AudioClip marioDiesAudio;

    public GameObject pauseCanvas;

    public int World { get; private set; }
    public int Stage { get; private set; }
    public int Lives {get; private set; }
    public int Coins {get; private set; }
    public int Time {get; private set; }
    
    //Singleton pattern implemented with Unity functions
    private void Awake()
    {   
        DontDestroyOnLoad(pauseCanvas);
        if(Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        pauseCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        Destroy(pauseCanvas);
        if(Instance == this)
        {
            Instance = null;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Victory"))
        {
            Pause();
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        StartNewGame();
    }

    public void StartNewGame()
    {
        Lives = 3;
        Coins = 0;
        World = 1;
        Stage = 1;

        PlayAudio(groundThemeAudio);
        Debug.Log("Starting new game");
        SceneManager.LoadScene("1 - 1");
    }

    public void LoadNextLevel()
    {   
        Debug.Log("Stepped into LoadNextLevel");
        audioSource.Stop();
        if(Stage < 1)
        {
            LoadLevel(World, Stage + 1);
        }
        else
        {
            Victory();
        }
    }

    public void LoadLevel(int world, int stage)
    {
        this.World = world;
        this.Stage = stage;


        SceneManager.LoadScene($"{World} - {Stage}");
        PlayAudio(groundThemeAudio);
    }

    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        if(--Lives > 0)
        {
            LoadLevel(World, Stage);
        }
        else
        {
            GameOver();
        }
    }

    public void AddCoin()
    {
        ++Coins;
        if(Coins == 100)
        {
            AddLife();
            Coins = 100;
        }

        //Collecting many coins can get obnoxiously loud, hence the lower volume
        PlayAudio(coinAudio, 0.5f);
    }

    public void AddLife()
    {
        ++Lives;
        PlayAudio(oneUpAudio);
    }

    private void GameOver()
    {
        audioSource.Stop();
        SceneManager.LoadScene("Game over");
    }

    private void Victory()
    {
        audioSource.Stop();
        
        SceneManager.LoadScene("Victory");
    }

    public void PlayAudio(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }

    public void PlayAudio(AudioClip audio, float volumeScale)
    {
        audioSource.PlayOneShot(audio, volumeScale);
    }

    private void Pause()
    {
        //Freeze game
        UnityEngine.Time.timeScale = 0f;
        pauseCanvas.SetActive(true);
    }

    public void ContinueGame()
    {
        pauseCanvas.SetActive(false);
        UnityEngine.Time.timeScale = 1f;
    }
}
