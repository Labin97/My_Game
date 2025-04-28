using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimationEvents : MonoBehaviour
{
    private Hero m_hero; // Hero 스크립트 참조
    private Hero_Stats m_stats; // Hero_Stats 스크립트 참조
    private Sensor_Hero m_sensorHero; // Sensor_Hero 스크립트 참조


    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_hero = GetComponent<Hero>();
        m_stats = GetComponent<Hero_Stats>();
        m_sensorHero = GetComponentInChildren<Sensor_Hero>();
    }


    //공격 애니메이션 용 이벤트
    public void EnableWeaponCollider(int index)
    {
        m_hero.weaponCollider[index].EnableCollider();
    }

    public void DisableWeaponCollider(int index)
    {
        m_hero.weaponCollider[index].DisableCollider();
    }

    // 스킬 공격 보너스
    public void skillAttackBonus(float amount)
    {
        m_stats.skillBonus = m_stats.damageMultiplier;
        m_stats.damageMultiplier += amount;
    }

    public void ResetSkillAttackBonus()
    {
        m_stats.damageMultiplier = m_stats.skillBonus;
    }

    //패링
    public void StartParrying()
    {
        m_hero.isParrying = true;
        m_animator.SetBool("IsParrying", true);
    }

    public void EndParrying()
    {
        m_hero.isParrying = false;
        m_animator.SetBool("IsParrying", false);
    }

    private void StartPerfectParrying()
    {
        m_hero.isPerfectParrying = true;
        m_animator.SetBool("PerfectParrying", true);
    }

    private void EndPerfectParrying()
    {
        m_hero.isPerfectParrying = false;
        m_animator.SetBool("PerfectParrying", false);
    }

    private void SetParryLevel(int level)
    {
        m_animator.SetInteger("ParryLevel", level);

        if (level == 1)
            m_stats.ApplyParryingBonus(0.7f);
        else if (level == 2)
            m_stats.ApplyParryingBonus(m_stats.minParryingBonus);
        else if (level == 3)
            m_stats.ApplyParryingBonus(m_stats.maxParryingBonus);
    }

    //퍼펙트 가드
    public void StartPerfectGuard()
    {
        m_hero.isPerfectGuard = true;
    }

    public void EndPerfectGuard()
    {
        m_hero.isPerfectGuard = false;
    }
}
