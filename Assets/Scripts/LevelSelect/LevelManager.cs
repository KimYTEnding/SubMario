using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable] // ����Ƽ�� �˷��ִ� ���, ����ȭ
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
            DontDestroyOnLoad(gameObject); // �ٸ� �� �̵� �ÿ��� ������Ʈ �����
        } else
        {
            Destroy(instance);
        }
    }// �̱��� �ۼ�

    /// TODO
    /// ���� �����͸� �����ϰ� �ִٰ� GameManager���� ���� �����͸� ����
    public void StartLevel(int index)
    {
        SelectedPrefab = LevelInfos[index].LevelPrefab;
        SceneManager.LoadScene("GameScene");
    }
}
