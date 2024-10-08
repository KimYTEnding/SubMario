using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable] // 유니티에 알려주는 기능, 직렬화
public class LevelInfo
{
    public string LevelName;
    public Sprite LevelThumb;
    public GameObject LevelPrefab;
}

public class LevelManager : MonoBehaviour
{
    public List<LevelInfo> LevelInfos;
    public GameObject SelectedPrefab;

    private static LevelManager instance;
    public static LevelManager Instance
    { 
        get
        { 
            return instance; 
        } 
        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 다른 씬 이동 시에도 오브젝트 비삭제
        } else
        {
            Destroy(instance);
        }
    }// 싱글톤 작성

    /// TODO
    /// 레벨 데이터를 저장하고 있다가 GameManager에게 레벨 데이터를 전달
    public void StartLevel(int index)
    {
        SelectedPrefab = LevelInfos[index].LevelPrefab;
        SceneManager.LoadScene("GameScene");
    }
}
