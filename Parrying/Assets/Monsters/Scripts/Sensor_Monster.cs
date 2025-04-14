using UnityEngine;
using System.Collections;

public class Sensor_Monster : MonoBehaviour
{
    private float invincibleDuration = 0.1f;
    private float m_invincibleTimer = 0f;
    private Animator               m_animator;
    private Collider2D             m_sensorCollider;
    private Monster_Stats          m_stats;
    private Monster                m_monster;
    [SerializeField]
    private GameObject             bloodEffectPrefab; // 피 이펙트 프리팹

    private void Awake()
    {
        m_sensorCollider = GetComponent<Collider2D>();
        m_animator = GetComponentInParent<Animator>();
        m_stats = GetComponentInParent<Monster_Stats>();
        m_monster = GetComponentInParent<Monster>();
    }

    private void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 무적 시간 안일 때 무시
        if (m_invincibleTimer > 0f)
            return;

        // 히어로 애니메이터 가져오기
        Animator otherAnimator = other.GetComponentInParent<Animator>();
        // Hero 공격력 가져오기
        Hero_Stats HeroStats = other.GetComponentInParent<Hero_Stats>();
        float damage = HeroStats.GetCurrentAttackDamage();

        bool isParried = otherAnimator != null && otherAnimator.GetBool("PerfectParrying");
        bool isAttacking = m_monster.IsAttacking; // 몬스터가 공격 중인지 여부

        // 경직 여부 판단
        bool applyStun = true;

        if (m_stats.SuperArmor)
        {
            // 슈퍼 아머가 있는 경우 패링과 공격중에만 경직
            applyStun = isParried && isAttacking;
        }

        Hurt(damage, applyStun, other);

        if (m_stats.currentHealth <= 0f)
        {
            m_animator.SetBool("Death", true);
            m_sensorCollider.enabled = false; // 죽으면 센서 비활성화

            Destroy(transform.root.gameObject, m_stats.DeathTime);
        }   
    }  

    private void Hurt(float damage, bool applyStun, Collider2D other)
    {
        if (applyStun)
        {
            m_animator.SetTrigger("Hurt");
            CameraShake.Instance.Shake(0.15f, 0.05f); // 카메라 흔들림 효과
        }

        m_stats.TakeDamage(damage);
        m_animator.SetBool("PowerAttack", false);  //파워 어택 초기화
        m_invincibleTimer = invincibleDuration;

        if (bloodEffectPrefab != null)
        {
            // 피 이펙트 생성
            Vector3 hitPosition = other.ClosestPoint(transform.position);
            GameObject bloodEffect = Instantiate(bloodEffectPrefab, hitPosition, Quaternion.identity);
            Destroy(bloodEffect, 1f); // 1초 후에 피 이펙트 삭제
        }

        Debug.Log("Monster current health: " + m_stats.currentHealth);
    }

    void Update()
    {
        if (m_invincibleTimer > 0f)
           m_invincibleTimer = Mathf.Max(0f, m_invincibleTimer - Time.deltaTime);
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
