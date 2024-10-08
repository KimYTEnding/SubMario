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


    private void OnEnable() // active가 아니었을 땐 실행안되다 active가 되면 실행 - 반대는 OnDisable
    {
        Time.timeScale = 0; // 게임 시간 진행 정지

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
    /// high score를 확인하고, 이를 출력하기
    void SaveHighScore()
    {
        float score = GameManager.Instance.TimeLimit;
        float highScore = PlayerPrefs.GetFloat("highscore", 0);
        if (GameManager.Instance.TimeLimit > highScore)
        {
            highScoreObject.SetActive(true);
            PlayerPrefs.SetFloat("highscore", GameManager.Instance.TimeLimit); // TimeLimit 저장
            // PlayerPrefs.Save(); 게임 저장
        } else
        {
            highScoreObject.SetActive(false);
        }

        /// TODO
        /// score 10개를 저장하기
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

            string result = string.Join(",", scoreList); // ,로 List를 하나의 string으로 병합
            Debug.Log(result);
            PlayerPrefs.SetString("HighScores", result);
        }

        PlayerPrefs.Save();
    }

    public void TryAgainPressed()
    {
        Time.timeScale = 1; // 게임 시간 진행
        SceneManager.LoadScene("GameScene");
    }

    public void QuitPressed()
    {
        Time.timeScale = 1; // 게임 시간 진행
        SceneManager.LoadScene("LevelSelectScene");
    }

    public void ShowHighScoresPanel()
    {
        highScorePopup.SetActive(true);
    }
}
