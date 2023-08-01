using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public AudioSource backgroundSource, soundSource;
    public AudioClip pauseButtonSound, restartButtonSound, muteButtonSound, quitButtonSound;
    private TextMeshProUGUI tmPro;
    public Button pauseButton, closeButton, soundButton, restartButton, exitButton;
    public GameObject UIObject, soundGameObject, tapToPlayObject, scoreObject, panelObject;
    public Sprite noSoundSprite, fullSoundSprite;

    private void Awake()
    {
        backgroundSource.volume = 0.5f;
        tmPro = scoreObject.gameObject.GetComponent<TextMeshProUGUI>();

        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);
        soundButton.onClick.AddListener(OnSoundButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        exitButton.onClick.AddListener(OnQuitButtonClicked);
    }
    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.SET)
        {
            scoreObject.gameObject.SetActive(false);
            tapToPlayObject.gameObject.SetActive(true);
            panelObject.gameObject.SetActive(true);
        }
        else
        {
            scoreObject.gameObject.SetActive(true);
            tapToPlayObject.gameObject.SetActive(false);
            panelObject.gameObject.SetActive(false);
        }

        if (GameManager.Instance.gameState == GameManager.GameState.RUNNING)
            pauseButton.gameObject.SetActive(true);
        else
            pauseButton.gameObject.SetActive(false);

        if(GameManager.Instance.gameState == GameManager.GameState.ENDED)
        {
            UIObject.SetActive(true);
            closeButton.gameObject.SetActive(false);
        }

        tmPro.text = BirdControl.Instance.pipesPassedCount.ToString();
    }

    private void OnPauseButtonClicked()
    {
        soundSource.PlayOneShot(pauseButtonSound);
        GameManager.Instance.gameState = GameManager.GameState.PAUSED;
        UIObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
    }

    private void OnCloseButtonClicked()
    {
        soundSource.PlayOneShot(pauseButtonSound);
        GameManager.Instance.gameState = GameManager.GameState.RUNNING;
        UIRaycaster.isPauseButtonHit = false;
        UIObject.SetActive(false);
    }

    private void OnSoundButtonClicked()
    {
        soundSource.PlayOneShot(muteButtonSound);

        if (backgroundSource.volume != 0f)
        {
            backgroundSource.volume = 0f;
            soundGameObject.GetComponent<Image>().sprite = noSoundSprite;
        }
        else
        {
            backgroundSource.volume = 0.5f;
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
        UIObject.SetActive(false);
    }

    private void OnQuitButtonClicked()
    {
        soundSource.PlayOneShot(quitButtonSound);
        Application.Quit();
    }
}