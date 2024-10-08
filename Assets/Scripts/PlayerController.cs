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
        
        if (BottomCollider.IsTouching(TerrainCollider))// 지금 바닥에 붙어있을 때,
        {
            if (!grounded) // 이전에 붙어있지 않았다면(착지)
            {
                if (vx == 0) // 이동이 없을 때
                {
                    GetComponent<Animator>().SetTrigger("Idle");
                }
                else // 이동이 있을 때
                {
                    GetComponent<Animator>().SetTrigger("Run");
                }
            } else // 땅에 있을 때
            {
                if (vx!=prevVx) // 움직임이 있었다면
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
        }else // 현재 땅에 붙어 있지 않음
        {
            if (grounded) // 이전에는 땅에 있었을 때
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

        if (Input.GetButtonDown("Fire1")) // Fire1 눌릴 시
        {
            Vector2 bulletV = new Vector2(10, 0); // 총알의 속도
            if (GetComponent<SpriteRenderer>().flipX) // 캐릭터가 축 반전상태일 때
            {
                bulletV.x = -bulletV.x; // 총알의 이동방향 반전
            }

            //GameObject bullet = Instantiate(BulletPrefab); // 프리팹 실체화
            GameObject bullet = GameManager.Instance.BulletPool.GetObject();
            bullet.transform.position = transform.position; // 총알의 출발 위치 설정
            bullet.GetComponent<Bullet>().Velocity = bulletV; // 총알의 속도 설정
        }
    }

    public void Restart()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation; // 회전 고정
        GetComponent<Rigidbody2D>().angularVelocity = 0; // 회전속도 삭제
        GetComponent<Collider2D>().enabled = true; // collider 활성화

        transform.eulerAngles = Vector3.zero; // 오브젝트의 rotation을 기존 값으로 초기화
        transform.position = originalPosition; // 초기 위치로 복귀
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // 이동 속도 초기화
        state = State.Playing; // playing 상태로 전환
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
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None; // z좌표 고정 해제
        GetComponent<Rigidbody2D>().angularVelocity = 720; // 초당 2회 반시계 방향 회전
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10), ForceMode2D.Impulse); // y에 10만큼의 힘을 작용
        GetComponent<Collider2D>().enabled = false; // collider 비활성화

        GameManager.Instance.Die();
    }


    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Clear in Player");
    }*/
}
