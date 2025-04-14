using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public List<WeaponCollider> weaponColliders;

    private Animator m_animator;
    private Monster_Stats m_stats;

    private float   attackCooldown = 0f; // 공격 쿨타임
    private bool isAttacking = false; 
    public bool IsAttacking => isAttacking; // 공격 중인지 여부를 외부에서 접근할 수 있도록 함
    public float rayDistance = 0.2f; // 레이캐스트 거리

    void Start()
    {
        m_stats = GetComponent<Monster_Stats>();
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        attackCooldown = Mathf.Max(0f, attackCooldown - Time.deltaTime);

        if (IsSomethingInRange() == 1)
        {
            if (!IsAttacking && attackCooldown <= 0f)
            {
                int attackIndex = Random.Range(0, weaponColliders.Count);
                StartCoroutine(PlayAttackAnimation(attackIndex + 1));
                attackCooldown = Random.Range(4.0f, 6.0f); // 다음 공격까지 대기 시간
            }
        }
        else if (IsSomethingInRange() == 2)
        {
        }
        else
        {

        }
    }

    IEnumerator PlayAttackAnimation(float attackType)
    {
        isAttacking = true; // 공격 시작
        m_animator.speed = m_stats.attackSpeedMultiplier;
        m_animator.SetTrigger("Attack" + attackType);

        //애니메이션 기다림
        AnimatorClipInfo[] clipInfos = m_animator.GetCurrentAnimatorClipInfo(0);
        //만약 재생중인 애니메이션이 있다면 그 길이를 가져옴. 없다면 0.5초 반환
        float clipLength = clipInfos.Length > 0 ? clipInfos[0].clip.length : 0.5f; // fallback 0.5초
        yield return new WaitForSeconds(clipLength / m_stats.attackSpeedMultiplier);

        isAttacking = false; // 공격 종료
        m_animator.speed = 1.0f;
    }

    // 적이 범위 내에 있는지 확인하는 메서드
    private int IsSomethingInRange()
    {
        Vector2 direction = transform.right * transform.localScale.x; // 바라보는 방향 (2D 좌우 반영)
        Vector2 origin = (Vector2)transform.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance, LayerMask.GetMask("EnemyHitsensor", "PlayerHitsensor"));

        Debug.DrawRay(origin, direction * rayDistance, Color.red);

        Debug.Log("Raycast hit: " + hit.collider?.name); // 히트된 오브젝트 이름 출력
        if (hit.collider != null)
        {

            if (hit.collider.CompareTag("PlayerHitSensor"))
            {
                return 1;
            }
            else if (hit.collider.CompareTag("EnemyHitSensor"))
            {
                return 2;
            }
        }

        return 0;
    }

}
