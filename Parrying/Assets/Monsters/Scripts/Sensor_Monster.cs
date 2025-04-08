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

            // 몬스터 애니메이터 트리거
            m_animator.SetTrigger("Hurt");

            // Hero 공격력 가져오기
            Hero_Stats HeroStats = other.GetComponentInParent<Hero_Stats>();
            float damage = 0f;

            if (HeroStats != null)
            {
                damage = HeroStats.GetCurrentAttackDamage();
            }

            // 체력 감소
            if (m_stats != null)
            {
                m_stats.TakeDamage(damage);

                if (m_stats.currentHealth <= 0f)
                {
                    Debug.Log("Monster is dead.");
                    m_animator.SetBool("Death", true); // 죽음 애니메이션 재생
                }
                else
                {
                    Debug.Log("Monster current health: " + m_stats.currentHealth);
                }
            }

        // 무적 타이머 시작
        m_invincibleTimer = invincibleDuration;
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
