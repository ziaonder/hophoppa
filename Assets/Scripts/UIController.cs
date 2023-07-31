using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private TextMeshProUGUI tmPro;
    public Button pauseButton, closeButton, soundButton, restartButton, exitButton;
    public GameObject UIObject, soundGameObject, tapToPlayObject, scoreObject, panelObject;
    public Sprite noSoundSprite, fullSoundSprite;

    private void Awake()
    {
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
        Debug.Log("clicked");
        GameManager.Instance.gameState = GameManager.GameState.PAUSED;
        UIObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
    }

    private void OnCloseButtonClicked()
    {
        GameManager.Instance.gameState = GameManager.GameState.RUNNING;
        UIRaycaster.isPauseButtonHit = false;
        UIObject.SetActive(false);
    }

    private void OnSoundButtonClicked()
    {
        if(AudioListener.volume != 0f)
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
        BirdControl.Instance.RestartGame();
        PipeControl.Instance.RestartGame();
        UIRaycaster.isPauseButtonHit = false;
        GameManager.Instance.gameState = GameManager.GameState.SET;
        UIObject.SetActive(false);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}