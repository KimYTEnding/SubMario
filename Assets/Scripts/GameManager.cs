using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CinemachineInstance;
    public LifeDisplayer LifeDisplayerInstance;
    public PlayerController Player;
    public ObjectPool BulletPool;

    [SerializeField]
    private GameObject popupCanvas;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }


    public TMP_Text TimeLimitLabel;
    public float TimeLimit = 30;
    private int life = 3;
    private bool isCleared;
    public bool IsCleared
    {
        get { return isCleared; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Instantiate(LevelManager.Instance.SelectedPrefab);

        life = 3;
        LifeDisplayerInstance.SetLifes(life);
    }

    // Update is called once per frame
    void Update()
    {
        TimeLimit -= Time.deltaTime;
        TimeLimitLabel.text = "Time Left " + ((int) TimeLimit);

        if (TimeLimit < 0 )
        {
            GameOver();
        }
    }

    public void AddTime(float time)
    {
        TimeLimit += time;
    }

    public void Die()
    {
        CinemachineInstance.SetActive(false);
        life--;
        LifeDisplayerInstance.SetLifes(life);

        Invoke("Restart", 2);
    }

    public void Restart()
    {
        if (life > 0)
        {
            CinemachineInstance.SetActive(true);
            Player.Restart();
        } else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isCleared = false;
        popupCanvas.SetActive(true);
    }

    public void GameClear()
    {
        isCleared = true;
        popupCanvas.SetActive(true);
    }
}
