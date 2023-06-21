using System.Collections; 
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    
    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();    
    }

    void Update()
    {
        //Debug.Log(GameManager.Instance.score);
        textMesh.text = $"Score: {GameManager.Instance.score}";
        //Debug.Log(textMesh.text);
    }
}
