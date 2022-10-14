using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    int scoreValue;
    public List<Transform> aiHuman;
    public GameObject gameOverScreen;
    // Start is called before the first frame update
    void Start()
    {
        scoreValue = PlayerPrefs.GetInt("Score");
        scoreText.text = "$" + scoreValue.ToString();
        GameObject[] aiS = GameObject.FindGameObjectsWithTag("AI");
        aiHuman = new List<Transform>();
        for(int i = 0; i < aiS.Length; i++)
        {
            aiHuman.Add(aiS[i].transform);
        }
    }

    public void UpdateScore()
    {
        scoreValue += 12;
        scoreText.text = "$"+ scoreValue.ToString();
        PlayerPrefs.SetInt("Score", scoreValue);
    }
    public void RemoveAnAI(Transform aiTransform)
    {
        aiHuman.Remove(aiTransform);
        if(aiHuman.Count < 1)
        {
            gameOverScreen.SetActive(true);
        }
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
