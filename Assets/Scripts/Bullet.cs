using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Velocity = new Vector2(10, 0);

    // Update is called once per frame
    private void Update()
    {
        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        transform.Translate(Velocity * Time.fixedDeltaTime);
        // GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + Velocity * Time.fixedDeltaTime); // ∫Æ¿ª ∂’¿ª ∂ß
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
        } else if (collision.gameObject.tag =="Enemy")
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
            collision.GetComponent<EnemyController>().Hit(1);
        }
    }
}
