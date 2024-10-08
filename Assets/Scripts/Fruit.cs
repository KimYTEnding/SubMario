using UnityEngine;

public class Fruit : MonoBehaviour
{
    public float TimeAdd = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.AddTime(TimeAdd);
            GetComponent<Animator>().SetTrigger("Eaten");
            GetComponent<Collider2D>().enabled = false;
            // Invoke("DestroyThis", 0.6f);
        }
    }
    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
