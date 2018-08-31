using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character instance;   // 스크립트 인스턴스

    // 이동관련
    Transform tr;   // 캐릭터 트렌스폼
    Vector3 tmpVec3;
    float moveSpeed = 0f;   // 이동속도
    Vector3 basePosition;

    // 상태
    public Animator charAnimator;
    bool isRight = false;
    bool isLeft = false;
    bool isUp = false;
    bool isDown = false;
    Queue<string> actions;
    float actionTime = 0;
    bool isAction = false;

    // -1이면 왼쪽, 1이면 오른쪽
    int direction = 1;
    // -1이면 사다리 밑, 0이면 사다리X, 1이면 사다리 위
    int OnLadder = 0;

    // 테스트용 컨트롤러
    public bool doRight = false;
    public bool doLeft = false;
    public bool doUp = false;
    public bool doDown = false;
    public bool doAttack = false;

    void Awake()
    {
        Character.instance = this;     // 스크립트 인스턴스
        tr = this.GetComponent<Transform>();    // 캐릭터 트렌스폼 받아오기

        actions = new Queue<string>();
    }

    void Start()
    {
        basePosition = tr.position;
        tmpVec3 = tr.position;
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        DoAtion();  // 행동을 순서대로 실행

        DoTest();

        Act();
    }

    // 큐에 담긴 행동 모두 실행
    public void DoAtion()
    {
        if (isAction && actions.Count != 0)
        {
            actionTime += Time.deltaTime;
            if (actionTime > 2)
            {
                actionTime = 0;

                switch (actions.Dequeue())
                {
                    case "MoveRight":
                        // 갈 곳에 땅이 있는지 없는 확인
                        try
                        {
                            if (!Physics2D.OverlapCircle(new Vector2(tr.position.x + 1, tr.position.y - 0.5f), 0.1f).tag.Equals("Floor"))
                            {
                                return;
                            }
                        }
                        catch
                        {
                            return;
                        }
                        charAnimator.SetTrigger("walk");
                        tr.position = tmpVec3;
                        tmpVec3 = tr.position;
                        tmpVec3.x += 1;
                        isRight = true;
                        direction = 1;
                        tr.rotation = Quaternion.Euler(0, 0, 0);
                        moveSpeed = 0;
                        break;

                    case "MoveLeft":
                        // 갈 곳에 땅이 있는지 없는 확인
                        try
                        {
                            if (!Physics2D.OverlapCircle(new Vector2(tr.position.x - 1, tr.position.y - 0.5f), 0.1f).tag.Equals("Floor"))
                            {
                                return;
                            }
                        }
                        catch
                        {
                            return;
                        }
                        charAnimator.SetTrigger("walk");
                        tr.position = tmpVec3;
                        tmpVec3 = tr.position;
                        tmpVec3.x -= 1;
                        isLeft = true;
                        direction = -1;
                        tr.rotation = Quaternion.Euler(0, 180, 0);
                        moveSpeed = 0;
                        break;

                    case "GoUp":
                        if (OnLadder != -1)
                        {
                            return;
                        }
                        charAnimator.SetTrigger("rope");
                        tr.position = tmpVec3;
                        tmpVec3 = tr.position;
                        tmpVec3.y += 3;
                        isUp = true;
                        moveSpeed = 0;
                        break;

                    case "GoDown":
                        if (OnLadder != 1)
                        {
                            return;
                        }
                        charAnimator.SetTrigger("rope");
                        tr.position = tmpVec3;
                        tmpVec3 = tr.position;
                        tmpVec3.y -= 3;
                        isDown = true;
                        moveSpeed = 0;
                        break;

                    case "Attack":
                        try
                        {
                            charAnimator.SetTrigger("attack");

                            Collider2D coll;

                            if (direction == -1)
                            {
                                coll = Physics2D.OverlapCircle(new Vector2(tr.position.x - 1, tr.position.y + 0.5f), 0.1f);
                            }
                            else
                            {
                                coll = Physics2D.OverlapCircle(new Vector2(tr.position.x + 1, tr.position.y + 0.5f), 0.1f);
                            }

                            if (coll.tag.Equals("Monster"))
                            {
                                coll.GetComponent<Monster>().Hit();
                            }
                        }
                        catch
                        {

                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // 모든 명령을 입력받음 -> DoAtion 들어감
    public void Action()
    {
        isAction = true;
    }

    // 행동 테스트용 함수
    void DoTest()
    {

        if (doRight)
        {
            doRight = false;
            MoveRight();
        }
        else if (doLeft)
        {
            doLeft = false;
            MoveLeft();
        }
        else if (doUp)
        {
            doUp = false;
            GoUp();

        }
        else if (doDown)
        {
            doDown = false;
            GoDown();
        }
        else if (doAttack)
        {
            doAttack = false;
            Attack();
        }
    }

    // 모든 행동관리
    void Act()
    {
        if (isRight)
        {
            moveSpeed += Time.deltaTime;
            tr.position = Vector3.Lerp(tr.position, tmpVec3, moveSpeed * 0.08f);
            if (tr.position.x + 0.01 > tmpVec3.x)
            {
                moveSpeed = 0;
                tr.position = tmpVec3;
                isRight = false;
            }
        }
        else if (isLeft)
        {
            moveSpeed += Time.deltaTime;
            tr.position = Vector3.Lerp(tr.position, tmpVec3, moveSpeed * 0.08f);
            if (tr.position.x - 0.01 < tmpVec3.x)
            {
                moveSpeed = 0;
                tr.position = tmpVec3;
                isLeft = false;
            }
        }
        else if (isUp)
        {
            moveSpeed += Time.deltaTime;
            tr.position = Vector3.Lerp(tr.position, tmpVec3, moveSpeed * 0.03f);
            if (tr.position.y + 0.1 > tmpVec3.y)
            {
                moveSpeed = 0;
                tr.position = tmpVec3;
                isUp = false;
            }
        }
        else if (isDown)
        {
            moveSpeed += Time.deltaTime;
            tr.position = Vector3.Lerp(tr.position, tmpVec3, moveSpeed * 0.03f);
            if (tr.position.y - 0.1 < tmpVec3.y)
            {
                moveSpeed = 0;
                tr.position = tmpVec3;
                isDown = false;
            }
        }
    }

    // 재시작
    public void ReStart()
    {
        tr.position = basePosition;
        tmpVec3 = tr.position;
        this.gameObject.SetActive(false);
        while (actions.Count != 0)
        {
            actions.Dequeue();
        }
    }

    // 오른쪽으로 1칸 이동
    public void MoveRight()
    {
        actions.Enqueue("MoveRight");
    }

    // 왼쪽으로 1칸 이동
    public void MoveLeft()
    {
        actions.Enqueue("MoveLeft");
    }

    // 사다리 올라감
    public void GoUp()
    {
        actions.Enqueue("GoUp");
    }

    // 사다리 내려감
    public void GoDown()
    {
        actions.Enqueue("GoDown");
    }

    // 공격
    public void Attack()
    {
        actions.Enqueue("Attack");
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Equals("LadderTop"))
        {
            OnLadder = 1;
        }
        else if (coll.tag.Equals("LadderBottom"))
        {
            OnLadder = -1;
        }
        else if (coll.tag.Equals("Monster"))
        {
            GameManager.instance.GameOver();
            ReStart();
        }
        else if (coll.tag.Equals("Portal"))
        {
            GameManager.instance.GameClear();
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag.Equals("LadderTop") && OnLadder == 1)
        {
            OnLadder = 0;
        }
        else if (coll.tag.Equals("LadderBottom") && OnLadder == -1)
        {
            OnLadder = 0;
        }
    }

}
