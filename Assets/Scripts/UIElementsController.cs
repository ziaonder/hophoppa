using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIElements : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    [SerializeField] private GameObject TapToPlay, Panel, Score;
    
    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh = Score.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMesh.text = $"Score: {GameManager.Instance.score}";

        if(GameManager.Instance.gameState != GameManager.GameState.SET)
        {
            Panel.gameObject.SetActive(false);
            TapToPlay.SetActive(false);
        }
        else
        {
            Panel.gameObject.SetActive(true);
            TapToPlay.gameObject.SetActive(true);
        }
    }
}
