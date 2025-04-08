using UnityEngine;
using System.Collections;

public class Sensor_Hero : MonoBehaviour
{
    private float invincibleDuration = 0.1f;
    private float m_invincibleTimer = 0f;
    private Animator            m_animator;
    private Hero_Stats          m_stats;
    private bool                isPerfectGuard = false;
    private bool                isParrying = false;

    private void OnEnable()
    {
        // 초기화
    }

    private void Awake()
    {
        m_animator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        m_stats = GetComponentInParent<Hero_Stats>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 무적 시간 안이거나 애니메이션이 Hurt 상태일 때 무시
        if (m_invincibleTimer > 0f || m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
           return;

        // 몬스터의 공격력 가져오기
        Monster_Stats monsterStats = other.GetComponentInParent<Monster_Stats>();
        float damage = 0f;

        if (monsterStats != null)
        {
            damage = monsterStats.GetCurrentAttackDamage();
        }

        // 공격 받았을 때
        if (!m_animator.GetBool("Guard") && !isParrying)
        {
            // 공격 받음
            Debug.Log("Hit by " + other.transform.parent.name);

            // 맞는 사운드 재생!!!!!!!!!!!!!!!!!!!!!!!!!!!

            // 애니메이션 재생
            m_animator.SetTrigger("Hurt");

            // 체력 감소
            if (m_stats != null)
            {
                m_stats.TakeDamage(damage);
            }
        }

        else if (m_animator.GetBool("Guard") && !isParrying)
        {
            // 방어 중일 때
            float MonsterX = other.transform.position.x;
            float HeroFacing = -m_animator.transform.localScale.x;
            float attackDirection = Mathf.Sign(MonsterX - transform.position.x);
            bool isGuarding = (HeroFacing * attackDirection) > 0f;

            if (m_stats != null)
            {
                if (isGuarding)
                {
                    if (isPerfectGuard)
                    {
                        //퍼펙트 가드 사운드 재생!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        m_stats.TakeDamage(0f); // 완벽 방어 시 데미지 100% 감소
                    }
                    else 
                    {
                        //가드 사운드 재생!!!!!!!!!!!!!!!!!!!!!!!!!!
                        m_stats.TakeDamage(damage * 0.2f); // 방어 시 데미지 80% 감소
                    }
                }
                else
                {
                    // 맞는 사운드 재생!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    m_animator.SetTrigger("Hurt");
                    m_stats.TakeDamage(damage); // 방어 실패 시 데미지 그대로
                }
            }
        }

        else if (isParrying)
        {
            Debug.Log("Parrying " + other.transform.parent.name);

            //패링 성공
            if (m_stats != null)
            {
                // 패링 사운드 재생!!!!!!!!!!!!!!!!!!!!!!!!!!!
                m_stats.ApplyParryingBonus(1.5f); // 패링 시 공격력 50% 증가
                m_stats.TakeDamage(0f); // 패링 시 데미지 100% 감소
            }
        }

        if (m_stats.currentHealth <= 0f)
        {
            // 죽은 사운드 재생!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Debug.Log("Hero is dead.");
            m_animator.SetBool("Death", true); // 죽음 애니메이션 재생
            GetComponent<Collider2D>().enabled = false; // 히어로의 콜라이더 비활성화
            return ;
        }
        else
        {
            Debug.Log("Hero current health: " + m_stats.currentHealth);
        }

        // 무적 타이머 시작
        m_invincibleTimer = invincibleDuration;

    }

    void Update()
    {
        if (m_invincibleTimer > 0f)
            m_invincibleTimer = Mathf.Max(0f, m_invincibleTimer - Time.deltaTime);
    }

    public void StartPerfectGuard()
    {
        isPerfectGuard = true;
    }

    public void EndPerfectGuard()
    {
        isPerfectGuard = false;
    }

    public void StartParrying()
    {
        isParrying = true;
    }

    public void EndParrying()
    {
        isParrying = false;
    }

}

