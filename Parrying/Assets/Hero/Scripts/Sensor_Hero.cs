using UnityEngine;
using System.Collections;

public class Sensor_Hero : MonoBehaviour
{
    private float invincibleDuration = 0.1f;
    private float m_invincibleTimer = 0f;
    private Animator            m_animator;
    private Hero_Stats          m_stats;
    private Hero                m_hero; 
    [SerializeField]
    private GameObject          bloodEffectPrefab; // 피 이펙트 프리팹
    [SerializeField]
    private GameObject          guardEffectPrefab; // 가드 이펙트 프리팹
    [SerializeField]
    private Transform           guardEffectPoint; // 가드 이펙트 위치
    [SerializeField]
    private GameObject          parryingEffectPrefab; // 패링 이펙트 프리팹
    private GameObject          guardEffectInstance; // 가드 이펙트 인스턴스
    private GameObject          parryingEffectInstance; // 패링 이펙트 인스턴스


    private void Awake()
    {
        m_animator = GetComponentInParent<Animator>();
        m_hero = GetComponentInParent<Hero>();
        m_stats = GetComponentInParent<Hero_Stats>();
    }

    private void Start()
    {
        guardEffectInstance = Instantiate(guardEffectPrefab, guardEffectPoint.position, guardEffectPoint.rotation, guardEffectPoint); //가드 이펙트 인스턴스 생성
        parryingEffectInstance = Instantiate(parryingEffectPrefab, guardEffectPoint.position, guardEffectPoint.rotation, guardEffectPoint); //패링 이펙트 인스턴스 생성
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
            damage = monsterStats.GetCurrentAttackDamage();

        // 몬스터의 공격 방향 계산
        bool isFacing = m_hero.IsFacingAttacker(other.transform);

        // 히트 포지션 계산
        Vector3 hitPosition = other.ClosestPoint(transform.position);

        //패링 방향이나 가드 방향의 실패
        if ((m_hero.isGuarding && !isFacing) || (m_hero.isParrying && !isFacing) || (m_hero.isPerfectParrying && !isFacing))
        {
            Hurt(damage, hitPosition); // 히어로 체력 감소
        }

        // 패링 성공
        else if (m_hero.isPerfectParrying && isFacing)
        {
            Debug.Log("Parrying " + other.transform.parent.name);
            PlayParryingEffect(); // 패링 이펙트 재생
            m_stats.SkillGageChange(33f);
        }

        // 가드 성공
        else if (m_hero.isGuarding)
        {
            if (isFacing)
            {
                if (m_hero.isPerfectGuard)
                {
                    //퍼펙트 가드 성공
                    m_stats.TakeDamage(0f); // 완벽 방어 시 데미지 100% 감소
                    CameraShake.Instance.Shake(0.15f, 0.05f); // 카메라 흔들림 효과
                    PlayGuardEffect(); // 가드 이펙트 재생
                    m_stats.SkillGageChange(11f);
                }
                else 
                {
                    // 일반 가드 상태에서의 파워 어택 체크
                    Animator otherAnimator = other.GetComponentInParent<Animator>();
                    if (otherAnimator != null && otherAnimator.GetBool("PowerAttack"))
                        Hurt(damage, hitPosition);
                    // 일반 가드
                    else
                    {
                        m_stats.TakeDamage(damage * 0.2f); // 방어 시 데미지 80% 감소
                        CameraShake.Instance.Shake(0.15f, 0.05f); // 카메라 흔들림 효과
                        PlayGuardEffect(); // 가드 이펙트 재생
                    }
                }
            }
        }

        // 맞음
        else
            Hurt(damage, hitPosition); // 히어로 체력 감소
    }

    // 히어로가 맞았을 때
    private void Hurt(float damage, Vector3 hitPosition)
    {

        m_animator.SetTrigger("Hurt");
        CameraShake.Instance.Shake(0.15f, 0.2f); // 카메라 흔들림 효과

        m_stats.TakeDamage(damage); // 히어로 체력 감소
        m_invincibleTimer = invincibleDuration; // 무적 타이머 시작
        m_animator.SetInteger("ParryLevel", 0); // 패링 레벨 초기화
        m_stats.ApplyParryingBonus(1.0f); // 패링 보너스 초기화

        PlayBloodEffect(hitPosition); // 피 이펙트 재생

        // 죽음
        if (m_stats.currentHealth <= 0f)
        {
            Debug.Log("Hero is dead.");
            m_animator.SetBool("Death", true); // 죽음 애니메이션 재생
            GetComponent<Collider2D>().enabled = false; // 히어로의 콜라이더 비활성화
            return ;
        }
    }

    private void PlayBloodEffect(Vector3 hitPosition)
    {
        if (bloodEffectPrefab != null)
        {
            // 피 이펙트 생성
            GameObject bloodEffect = Instantiate(bloodEffectPrefab, hitPosition, Quaternion.identity);
            Destroy(bloodEffect, 1f); // 1초 후에 피 이펙트 삭제
        }
        AudioManager.instance.PlaySFX("Hurt"); // 피 사운드 재생
    }
    
    private void PlayGuardEffect()
    {
        if (guardEffectInstance != null && guardEffectPoint != null)
        {
            ParticleSystem effect = guardEffectInstance.GetComponent<ParticleSystem>();
            if (effect != null)
                effect.Play();
        }
        AudioManager.instance.PlaySFX("Guard"); // 가드 사운드 재생
    }

    private void PlayParryingEffect()
    {
        if (parryingEffectInstance != null && guardEffectPoint != null)
        {
            ParticleSystem effect = parryingEffectInstance.GetComponent<ParticleSystem>();
            if (effect != null)
                effect.Play();
        }
        AudioManager.instance.PlaySFX("Parrying"); // 패링 사운드 재생
    }

    void Update()
    {
        if (m_invincibleTimer > 0f)
            m_invincibleTimer = Mathf.Max(0f, m_invincibleTimer - Time.deltaTime);
    }

}

