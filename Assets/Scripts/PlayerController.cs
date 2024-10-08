using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State {
        Playing,
        Dead
    }

    public float Speed = 5;
    public float JumpSpeed = 10;
    public Collider2D BottomCollider;
    public CompositeCollider2D TerrainCollider;

    public GameObject BulletPrefab;

    float vx = 0;
    float vy = 0;
    bool grounded;
    float prevVx = 0;
    float prevVy = 0;

    Vector2 originalPosition;
    State state;

    void Start()
    {
        originalPosition = transform.position;
        state = State.Playing;
    }

    void Update()
    {
        if (state == State.Dead) return;

        vx = Input.GetAxisRaw("Horizontal") * Speed;
        vy = GetComponent<Rigidbody2D>().linearVelocityY;

        if(vx < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (vx > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false ;
        }
        
        if (BottomCollider.IsTouching(TerrainCollider))// ���� �ٴڿ� �پ����� ��,
        {
            if (!grounded) // ������ �پ����� �ʾҴٸ�(����)
            {
                if (vx == 0) // �̵��� ���� ��
                {
                    GetComponent<Animator>().SetTrigger("Idle");
                }
                else // �̵��� ���� ��
                {
                    GetComponent<Animator>().SetTrigger("Run");
                }
            } else // ���� ���� ��
            {
                if (vx!=prevVx) // �������� �־��ٸ�
                {
                    if (vx == 0)
                    {
                        GetComponent<Animator>().SetTrigger("Idle");
                    } else
                    {
                        GetComponent<Animator>().SetTrigger("Run");
                    }
                }
            }
        }else // ���� ���� �پ� ���� ����
        {
            if (grounded) // �������� ���� �־��� ��
            {
                GetComponent<Animator>().SetTrigger("Jump");
            }
            if(vy * prevVy < 0)
            {
                GetComponent<Animator>().SetTrigger("Fall");
            }
        }

        grounded = BottomCollider.IsTouching(TerrainCollider);
        if (Input.GetButtonDown("Jump") && grounded)
        {
            vy = JumpSpeed;
        }
        prevVx = vx;
        prevVy = vy;
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(vx, vy);

        if (Input.GetButtonDown("Fire1")) // Fire1 ���� ��
        {
            Vector2 bulletV = new Vector2(10, 0); // �Ѿ��� �ӵ�
            if (GetComponent<SpriteRenderer>().flipX) // ĳ���Ͱ� �� ���������� ��
            {
                bulletV.x = -bulletV.x; // �Ѿ��� �̵����� ����
            }

            //GameObject bullet = Instantiate(BulletPrefab); // ������ ��üȭ
            GameObject bullet = GameManager.Instance.BulletPool.GetObject();
            bullet.transform.position = transform.position; // �Ѿ��� ��� ��ġ ����
            bullet.GetComponent<Bullet>().Velocity = bulletV; // �Ѿ��� �ӵ� ����
        }
    }

    public void Restart()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation; // ȸ�� ����
        GetComponent<Rigidbody2D>().angularVelocity = 0; // ȸ���ӵ� ����
        GetComponent<Collider2D>().enabled = true; // collider Ȱ��ȭ

        transform.eulerAngles = Vector3.zero; // ������Ʈ�� rotation�� ���� ������ �ʱ�ȭ
        transform.position = originalPosition; // �ʱ� ��ġ�� ����
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // �̵� �ӵ� �ʱ�ȭ
        state = State.Playing; // playing ���·� ��ȯ
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Die();
        }
    }

    void Die()
    {
        state = State.Dead;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None; // z��ǥ ���� ����
        GetComponent<Rigidbody2D>().angularVelocity = 720; // �ʴ� 2ȸ �ݽð� ���� ȸ��
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10), ForceMode2D.Impulse); // y�� 10��ŭ�� ���� �ۿ�
        GetComponent<Collider2D>().enabled = false; // collider ��Ȱ��ȭ

        GameManager.Instance.Die();
    }


    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Clear in Player");
    }*/
}
