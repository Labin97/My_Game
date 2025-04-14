using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimationEvents : MonoBehaviour
{
    [SerializeField] private Hero hero; // Hero 스크립트 참조
    [SerializeField] private Hero_Stats stats;
    [SerializeField] private Sensor_Hero sensorHero;


    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }


    //공격 애니메이션 용 이벤트
    public void EnableWeaponCollider()
    {
        hero.weaponCollider.EnableCollider();
    }

    public void DisableWeaponCollider()
    {
        hero.weaponCollider.DisableCollider();
    }

    //패링
    public void StartParrying()
    {
        hero.SetIsParrying(true);
        m_animator.SetBool("IsParrying", true);
    }

    public void EndParrying()
    {
        hero.SetIsParrying(false);
        m_animator.SetBool("IsParrying", false);
    }

    private void StartPerfectParrying()
    {
        hero.SetIsPerfectParrying(true);
        m_animator.SetBool("PerfectParrying", true);
    }

    private void EndPerfectParrying()
    {
        hero.SetIsPerfectParrying(false);
        m_animator.SetBool("PerfectParrying", false);
    }

    private void SetParryLevel(int level)
    {
        m_animator.SetInteger("ParryLevel", level);

        if (level == 1)
            stats.ApplyParryingBonus(0.7f);
        else if (level == 2)
            stats.ApplyParryingBonus(stats.minParryingBonus);
        else if (level == 3)
            stats.ApplyParryingBonus(stats.maxParryingBonus);
    }

    //퍼펙트 가드
    public void StartPerfectGuard()
    {
        hero.SetIsPerfectGuard(true);
    }

    public void EndPerfectGuard()
    {
        hero.SetIsPerfectGuard(false);
    }
}
