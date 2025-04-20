using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // 더 많은 위치 정의 (좌우 각 5개)
    public enum Position { 
        Left_5 = 0, Left_4 = 1, Left_3 = 2, Left_2 = 3, Left_1 = 4, 
        Right_1 = 5, Right_2 = 6, Right_3 = 7, Right_4 = 8, Right_5 = 9 
    }
    
    [Header("Position Settings")]
    public Position currentPosition;
    
    // 게임 시작 시 모든 위치 포인트 참조를 저장
    private static List<Transform> positionPoints;
    
    [Header("Components")]
    public List<WeaponCollider> weaponColliders;
    
    private Animator m_animator;
    private Monster_Stats m_stats;
    private float attackCooldown = 0f;
    private bool isAttacking = false;
    private bool isMoving = false;
    
    public bool IsAttacking => isAttacking;
    
    // 게임 시작 시 한번만 실행되는 초기화
    private static bool initialized = false;
    
    void Awake()
    {
        m_stats = GetComponent<Monster_Stats>();
        m_animator = GetComponent<Animator>();
        
        // 첫 번째로 생성된 몬스터가 위치 포인트들을 초기화
        if (!initialized)
        {
            InitializePositionPoints();
            initialized = true;
        }
    }
    
    // 모든 위치 포인트를 찾아서 초기화
    private void InitializePositionPoints()
    {
        positionPoints = new List<Transform>();
        GameObject positionsParent = GameObject.Find("MonsterPositions");
        
        if (positionsParent != null)
        {
            // "Left_5", "Left_4", ... 등의 이름을 가진 자식 오브젝트들을 찾아 순서대로 추가
            for (int i = 0; i < 10; i++)
            {
                string pointName = "";
                if (i < 5)
                    pointName = "Left_" + (5 - i);
                else
                    pointName = "Right_" + (i - 4);
                
                Transform point = positionsParent.transform.Find(pointName);
                if (point != null)
                    positionPoints.Add(point);
                else
                    Debug.LogError("Position point not found: " + pointName);
            }
        }
        else
        {
            Debug.LogError("MonsterPositions parent object not found!");
        }
    }
    
    void Start()
    {
        // 시작 위치로 이동
        transform.position = positionPoints[(int)currentPosition].position;
        UpdateFacingDirection();
        
        // 죽었을 때 이벤트 구독
        // m_stats.OnDeathEvent += OnDeath;
    }
    
    void Update()
    {
        if (isMoving || m_stats.IsDead())
            return;
            
        attackCooldown = Mathf.Max(0f, attackCooldown - Time.deltaTime);
        
        // 히어로와 가까운 포지션(Left_1, Right_1)에서만 공격
        if (currentPosition == Position.Left_1 || currentPosition == Position.Right_1)
        {
            if (!IsAttacking && attackCooldown <= 0f)
            {
                int attackIndex = Random.Range(0, weaponColliders.Count);
                StartCoroutine(PlayAttackAnimation(attackIndex + 1));
                attackCooldown = Random.Range(4.0f, 6.0f);
            }
        }
        else
        {
            // 앞쪽 위치 계산
            Position frontPosition = GetFrontPosition(currentPosition);
            
            // 앞에 몬스터가 없으면 전진
            if (!IsMonsterAtPosition(frontPosition))
            {
                StartCoroutine(MoveToPosition(frontPosition));
            }
        }
    }
    
    // 앞쪽 위치 반환
    private Position GetFrontPosition(Position current)
    {
        switch (current)
        {
            case Position.Left_5: return Position.Left_4;
            case Position.Left_4: return Position.Left_3;
            case Position.Left_3: return Position.Left_2;
            case Position.Left_2: return Position.Left_1;
            case Position.Right_5: return Position.Right_4;
            case Position.Right_4: return Position.Right_3;
            case Position.Right_3: return Position.Right_2;
            case Position.Right_2: return Position.Right_1;
            default: return current; // Left_1, Right_1은 그대로
        }
    }
    
    // 뒤쪽 위치 반환
    private Position GetBackPosition(Position current)
    {
        switch (current)
        {
            case Position.Left_4: return Position.Left_5;
            case Position.Left_3: return Position.Left_4;
            case Position.Left_2: return Position.Left_3;
            case Position.Left_1: return Position.Left_2;
            case Position.Right_4: return Position.Right_5;
            case Position.Right_3: return Position.Right_4;
            case Position.Right_2: return Position.Right_3;
            case Position.Right_1: return Position.Right_2;
            default: return current; // Left_5, Right_5은 그대로
        }
    }
    
    // 특정 위치에 몬스터가 있는지 확인
    private bool IsMonsterAtPosition(Position pos)
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        foreach (Monster monster in monsters)
        {
            if (monster != this && monster.currentPosition == pos && !monster.m_stats.IsDead())
                return true;
        }
        return false;
    }
    
    // 새 위치로 이동
    private IEnumerator MoveToPosition(Position newPos)
    {
        isMoving = true;
        
        // 이동 애니메이션 재생 (필요한 경우)
        // m_animator.SetBool("IsMoving", true);
        
        // 목표 위치
        Vector3 targetPos = positionPoints[(int)newPos].position;
        
        // 부드러운 이동
        float moveSpeed = 2.0f;
        float startTime = Time.time;
        Vector3 startPos = transform.position;
        float journeyLength = Vector3.Distance(startPos, targetPos);
        
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
            yield return null;
        }
        
        // 최종 위치 설정 및 상태 업데이트
        transform.position = targetPos;
        currentPosition = newPos;
        UpdateFacingDirection();
        
        // 이동 애니메이션 종료
        // m_animator.SetBool("IsMoving", false);
        
        isMoving = false;
    }
    
    // 몬스터가 Hero를 바라보도록 방향 설정
    private void UpdateFacingDirection()
    {
        // Left 포지션은 오른쪽을, Right 포지션은 왼쪽을 바라봄
        int pos = (int)currentPosition;
        if (pos < 5) // Left 포지션들
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // 오른쪽 바라보기
        else // Right 포지션들
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f); // 왼쪽 바라보기
    }
    
    IEnumerator PlayAttackAnimation(float attackType)
    {
        isAttacking = true;
        m_animator.speed = m_stats.attackSpeedMultiplier;
        m_animator.SetTrigger("Attack" + attackType);
        
        AnimatorClipInfo[] clipInfos = m_animator.GetCurrentAnimatorClipInfo(0);
        float clipLength = clipInfos.Length > 0 ? clipInfos[0].clip.length : 0.5f;
        yield return new WaitForSeconds(clipLength / m_stats.attackSpeedMultiplier);
        
        isAttacking = false;
        m_animator.speed = 1.0f;
    }
    
    // 몬스터가 죽었을 때 호출되는 메서드
    public void OnDeath()
    {
        // 뒤쪽에 있는 몬스터들에게 전진하라고 알림
        NotifyMonstersToAdvance();
    }
    
    // 뒤쪽 몬스터들에게 전진하라고 알림
    private void NotifyMonstersToAdvance()
    {
        // 현재 위치 기준으로 뒤쪽 위치 계산
        Position backPosition = GetBackPosition(currentPosition);
        
        // 해당 위치에 몬스터가 있는지 확인
        Monster[] monsters = FindObjectsOfType<Monster>();
        foreach (Monster monster in monsters)
        {
            if (monster.currentPosition == backPosition && !monster.m_stats.IsDead() && !monster.isMoving)
            {
                // 해당 몬스터에게 전진하라고 알림
                monster.StartCoroutine(monster.MoveToPosition(currentPosition));
                break; // 한 마리만 전진시킴
            }
        }
    }
    
    // private void OnDestroy()
    // {
    //     // 이벤트 구독 해제
    //     if (m_stats != null)
    //         m_stats.OnDeathEvent -= OnDeath;
    // }
}