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
    public void EnableWeaponCollider()
    {
        m_hero.weaponCollider.EnableCollider();
    }

    public void DisableWeaponCollider()
    {
        m_hero.weaponCollider.DisableCollider();
    }

    //패링
    public void StartParrying()
    {
        m_hero.SetIsParrying(true);
        m_animator.SetBool("IsParrying", true);
    }

    public void EndParrying()
    {
        m_hero.SetIsParrying(false);
        m_animator.SetBool("IsParrying", false);
    }

    private void StartPerfectParrying()
    {
        m_hero.SetIsPerfectParrying(true);
        m_animator.SetBool("PerfectParrying", true);
    }

    private void EndPerfectParrying()
    {
        m_hero.SetIsPerfectParrying(false);
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
        m_hero.SetIsPerfectGuard(true);
    }

    public void EndPerfectGuard()
    {
        m_hero.SetIsPerfectGuard(false);
    }
}
