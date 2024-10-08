using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public PlayerController Player;
    public GameObject Cinemachine;

    private void Awake()
    {
        GameManager.Instance.Player = Player;
        GameManager.Instance.CinemachineInstance = Cinemachine;
    }
}
