using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPopup : MonoBehaviour
{
    
    [SerializeField]
    private TMP_Text resultTitle;
    [SerializeField]
    private TMP_Text scoreLabel;
    [SerializeField]
    GameObject highScoreObject;
    [SerializeField]
    GameObject highScorePopup;


    private void OnEnable() // active�� �ƴϾ��� �� ����ȵǴ� active�� �Ǹ� ���� - �ݴ�� OnDisable
    {
        Time.timeScale = 0; // ���� �ð� ���� ����

        if (GameManager.Instance.IsCleared)
        {
            resultTitle.text = "Cleared!!!";
            scoreLabel.text = GameManager.Instance.TimeLimit.ToString("#.##");
            SaveHighScore();
        } else
        {
            resultTitle.text = "Game Over...";
            scoreLabel.text = "";
            highScoreObject.SetActive(false);
        }
    }

    /// TODO
    /// high score�� Ȯ���ϰ�, �̸� ����ϱ�
    void SaveHighScore()
    {
        float score = GameManager.Instance.TimeLimit;
        float highScore = PlayerPrefs.GetFloat("highscore", 0);
        if (GameManager.Instance.TimeLimit > highScore)
        {
            highScoreObject.SetActive(true);
            PlayerPrefs.SetFloat("highscore", GameManager.Instance.TimeLimit); // TimeLimit ����
            // PlayerPrefs.Save(); ���� ����
        } else
        {
            highScoreObject.SetActive(false);
        }

        /// TODO
        /// score 10���� �����ϱ�
        string currentScoreString = score.ToString("#.###");
        string savedScoredString = PlayerPrefs.GetString("HighScores", "");

        if (savedScoredString == "")
        {
            PlayerPrefs.SetString("HighScores", currentScoreString);
        } else
        {
            string[] scoreArray = savedScoredString.Split(',');
            List<string> scoreList = new List<string>(scoreArray);

            for (int i = 0; i < scoreList.Count; i++)
            {
                float savedScore = float.Parse(scoreList[i]);
                if (savedScore < score)
                {
                    scoreList.Insert(i, currentScoreString);
                    break;
                }
            }
            if (scoreArray.Length == scoreList.Count)
            {
                scoreList.Add(currentScoreString);
            }

            if(scoreList.Count > 10)
            {
                scoreList.RemoveAt(10);
            }

            string result = string.Join(",", scoreList); // ,�� List�� �ϳ��� string���� ����
            Debug.Log(result);
            PlayerPrefs.SetString("HighScores", result);
        }

        PlayerPrefs.Save();
    }

    public void TryAgainPressed()
    {
        Time.timeScale = 1; // ���� �ð� ����
        SceneManager.LoadScene("GameScene");
    }

    public void QuitPressed()
    {
        Time.timeScale = 1; // ���� �ð� ����
        SceneManager.LoadScene("LevelSelectScene");
    }

    public void ShowHighScoresPanel()
    {
        highScorePopup.SetActive(true);
    }
}
