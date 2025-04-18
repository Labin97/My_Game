using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterAnimationEvents : MonoBehaviour
{
    private Monster m_monster;
    private Monster_Stats m_stats;
    private Sensor_Monster m_sensorMonster;

    [SerializeField] private GameObject warningIcon;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_monster = GetComponentInParent<Monster>();
        m_stats = GetComponentInParent<Monster_Stats>();
        m_sensorMonster = GetComponentInChildren<Sensor_Monster>();
    }

    // 슈퍼아머 시작 및 종료
    public void StartSuperArmor() => m_stats.SuperArmor = true;
    public void EndSuperArmor() => m_stats.SuperArmor = false;


    // 파워 어택 시작 및 종료
    public void StartPowerAttack()
    {
        m_animator.SetBool("PowerAttack", true);
        ShowWarning();
    }

    public void EndPowerAttack() => m_animator.SetBool("PowerAttack", false);


    // 공격 콜라이더 활성화 및 비활성화
    public void EnableWeaponColliderByIndex(int index)
    {
        if (index >= 0 && index < m_monster.weaponColliders.Count)
            m_monster.weaponColliders[index].EnableCollider();
    }

    public void DisableWeaponColliderByIndex(int index)
    {
        if (index >= 0 && index < m_monster.weaponColliders.Count)
            m_monster.weaponColliders[index].DisableCollider();
    }


    // 센서 콜라이더 활성화 및 비활성화
    public void EnableSensorCollider() => m_sensorMonster.EnableCollider();
    public void DisableSensorCollider() => m_sensorMonster.DisableCollider();


    // 파워어택 시작 시 경고 아이콘 표시
    private void ShowWarning()
    {
        warningIcon.SetActive(true);
        StartCoroutine(HideWarning());
    }

    private IEnumerator HideWarning()
    {
        yield return new WaitForSeconds(1f);
        warningIcon.SetActive(false);
    }
}
