using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject highScoreHolder, endGameScore, upperScreenScore;
    private float mainMenuMusicVolume = 0.2f, inGameMusicVolume = 1f;
    public AudioSource backgroundSource, soundSource;
    public AudioClip pauseButtonSound, restartButtonSound, muteButtonSound, quitButtonSound, backgroundMusic, dumpSound;
    private TextMeshProUGUI scoreTmPro, highScoreTmPro;
    public Button pauseButton, closeButton, soundButton, restartButton, exitButton;
    public GameObject UIObject, soundGameObject, tapToPlayObject, scoreObject, panelObject, logoObject;
    public Sprite noSoundSprite, fullSoundSprite;

    private void OnEnable()
    {
        BirdControl.OnGameStarted += () =>
        {
            backgroundSource.volume = inGameMusicVolume;
        };

        BirdControl.OnHit += () =>
        {
            soundSource.PlayOneShot(dumpSound);
        };

        BirdControl.OnGameEnd += SetEndGameScore;
    }

    private void OnDisable()
    {
        BirdControl.OnGameStarted -= () =>
        {
            backgroundSource.volume = inGameMusicVolume;
        };

        BirdControl.OnHit -= () =>
        {
            soundSource.PlayOneShot(dumpSound);
        };

        BirdControl.OnGameEnd -= SetEndGameScore;
    }

    private void Awake()
    {
        highScoreTmPro = highScoreHolder.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        highScoreTmPro.text = "High Score: " + HighScoreManager.GetHighScore().ToString();

        backgroundSource.volume = mainMenuMusicVolume;
        scoreTmPro = scoreObject.gameObject.GetComponent<TextMeshProUGUI>();

        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);
        soundButton.onClick.AddListener(OnMuteButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        exitButton.onClick.AddListener(OnQuitButtonClicked);
    }
    private void Update()
    {
        if(highScoreTmPro.text != HighScoreManager.GetHighScore().ToString())
        {
            highScoreTmPro.text = "High Score: " + HighScoreManager.GetHighScore().ToString();
        }

        if (GameManager.Instance.gameState == GameManager.GameState.SET)
        {
            scoreObject.gameObject.SetActive(false);
            tapToPlayObject.gameObject.SetActive(true);
            panelObject.gameObject.SetActive(true);
            logoObject.gameObject.SetActive(true);
            highScoreHolder.gameObject.SetActive(true);
        }
        else
        {
            scoreObject.gameObject.SetActive(true);
            tapToPlayObject.gameObject.SetActive(false);
            panelObject.gameObject.SetActive(false);
            logoObject.gameObject.SetActive(false);
            highScoreHolder.gameObject.SetActive(false);
        }

        if (GameManager.Instance.gameState == GameManager.GameState.RUNNING)
        {
            pauseButton.gameObject.SetActive(true);
            upperScreenScore.SetActive(true);
        }
        else
        {
            pauseButton.gameObject.SetActive(false);
            upperScreenScore.SetActive(false);
        }

        if(GameManager.Instance.gameState == GameManager.GameState.ENDED)
        {
            UIObject.SetActive(true);
            closeButton.gameObject.SetActive(false);
            endGameScore.gameObject.SetActive(true);
        }

        scoreTmPro.text = BirdControl.Instance.pipesPassedCount.ToString();
    }

    private void OnPauseButtonClicked()
    {
        soundSource.PlayOneShot(pauseButtonSound);
        GameManager.Instance.gameState = GameManager.GameState.PAUSED;
        UIObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        endGameScore.gameObject.SetActive(false);
    }

    private void OnCloseButtonClicked()
    {
        soundSource.PlayOneShot(pauseButtonSound);
        GameManager.Instance.gameState = GameManager.GameState.RUNNING;
        UIRaycaster.isPauseButtonHit = false;
        UIObject.SetActive(false);
    }

    private void OnMuteButtonClicked()
    {
        soundSource.PlayOneShot(muteButtonSound);

        if (AudioListener.volume != 0f)
        {
            AudioListener.volume = 0f;
            soundGameObject.GetComponent<Image>().sprite = noSoundSprite;
        }
        else
        {
            AudioListener.volume = 1f;
            soundGameObject.GetComponent<Image>().sprite = fullSoundSprite;
        } 
    }

    private void OnRestartButtonClicked()
    {
        soundSource.PlayOneShot(restartButtonSound);
        BirdControl.Instance.RestartGame();
        PipeControl.Instance.RestartGame();
        UIRaycaster.isPauseButtonHit = false;
        GameManager.Instance.gameState = GameManager.GameState.SET;
        backgroundSource.volume = mainMenuMusicVolume;
        UIObject.SetActive(false);
    }

    private void OnQuitButtonClicked()
    {
        soundSource.PlayOneShot(quitButtonSound);
        Application.Quit();
    }

    private void SetEndGameScore(int score)
    {
        endGameScore.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }
}