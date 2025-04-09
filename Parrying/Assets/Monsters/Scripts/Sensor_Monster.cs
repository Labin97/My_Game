using UnityEngine;
using System.Collections;

public class Sensor_Monster : MonoBehaviour
{
    private float invincibleDuration = 0.1f;
    private float m_invincibleTimer = 0f;
    private Animator               m_animator;
    private Collider2D             m_sensorCollider;
    private Monster_Stats          m_stats;

    private void OnEnable()
    {
        // 비활성 했다가 다시 켜는 상황에서 초기화
    }

    private void Awake()
    {
        m_sensorCollider = GetComponent<Collider2D>();
        m_animator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        m_stats = GetComponentInParent<Monster_Stats>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_invincibleTimer > 0f || m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            return;

        Animator otherAnimator = other.GetComponentInParent<Animator>();
        string myCurrentClipName = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        // Hero 공격력 가져오기
        Hero_Stats HeroStats = other.GetComponentInParent<Hero_Stats>();
        float damage = 0f;

        if (HeroStats != null)
        {
            damage = HeroStats.GetCurrentAttackDamage();
        }

        // 큰 몬스터는 공격하다 패링 맞았을 때만 경직
        if (m_stats.BigMonster)
        {
            if (otherAnimator != null && otherAnimator.GetBool("PerfectParrying"))
            {
                Debug.Log("Parrying by " + other.transform.parent.name);

                if (myCurrentClipName.Contains("Attack"))
                    Hurt(damage, false); // 몬스터 체력 감소
            }
            else
                Hurt(damage, true); // 몬스터 체력 감소
        }
        else
        {
            Hurt(damage, false); // 몬스터 체력 감소
        }


        // 죽음
        if (m_stats.currentHealth <= 0f)
        {
            m_animator.SetBool("Death", true); // 죽음 애니메이션 재생
        }
    }

    private void Hurt(float damage, bool BigMonster)
    {
        if (!BigMonster)
            m_animator.SetTrigger("Hurt");
        m_animator.SetBool("PowerAttack", false); // 파워 어택 해제
        m_stats.TakeDamage(damage); // 몬스터 체력 감소
        m_invincibleTimer = invincibleDuration; // 무적 타이머 시작

        Debug.Log("Monster current health: " + m_stats.currentHealth);
    }

    void Update()
    {
        if (m_invincibleTimer > 0f)
            m_invincibleTimer -= Time.deltaTime;
    }

    // 애니메이션 이벤트에서 호출할 메서드
    public void EnableCollider()
    {
        m_sensorCollider.enabled = true;
    }

    public void DisableCollider()
    {
        m_sensorCollider.enabled = false;
    }
}
