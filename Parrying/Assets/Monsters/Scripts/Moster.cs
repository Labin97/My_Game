using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moster : MonoBehaviour
{
    private Animator m_animator;
    private Monster_Stats          m_stats;
    private Sensor_Monster      m_sensorMonster;
    [SerializeField]
    private List<WeaponCollider> m_weaponColliders;
    [SerializeField]
    private GameObject warningIcon; // 경고 아이콘 오브젝트

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_stats = GetComponent<Monster_Stats>();
        m_sensorMonster = GetComponentInChildren<Sensor_Monster>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(PlayAttackAnimation(1));
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(PlayAttackAnimation(2));
        }
    }

    IEnumerator PlayAttackAnimation(float attackType)
    {
        m_animator.speed = m_stats.attackSpeedMultiplier;
        m_animator.SetTrigger("Attack" + attackType);

        //애니메이션 기다림
        AnimatorClipInfo[] clipInfos = m_animator.GetCurrentAnimatorClipInfo(0);
        //만약 재생중인 애니메이션이 있다면 그 길이를 가져옴. 없다면 0.5초 반환
        float clipLength = clipInfos.Length > 0 ? clipInfos[0].clip.length : 0.5f; // fallback 0.5초
        yield return new WaitForSeconds(clipLength / m_stats.attackSpeedMultiplier);

        m_animator.speed = 1.0f;
    }


    // 경고 아이콘 표시
    public void ShowWarning()
    {
        warningIcon.SetActive(true);
        StartCoroutine(DisableWarningIcon());
    }

    private IEnumerator DisableWarningIcon()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        warningIcon.SetActive(false);
    }

    //파워 어택 애니메이션 용 이벤트
    public void StartPowerAttack()
    {
        m_animator.SetBool("PowerAttack", true);
        ShowWarning(); // 경고 아이콘 표시
    }

    public void EndPowerAttack()
    {
        m_animator.SetBool("PowerAttack", false);
    }

    //공격 애니메이션 용 이벤트
    public void EnableWeaponColliderByIndex(int index)
    {
        if (index >= 0 && index < m_weaponColliders.Count)
            m_weaponColliders[index].EnableCollider();
    }

    public void DisableWeaponColliderByIndex(int index)
    {
        if (index >= 0 && index < m_weaponColliders.Count)
            m_weaponColliders[index].DisableCollider();
    }

    //공격 무적 이벤트
    public void EnableSensorCollider()
    {
        m_sensorMonster.EnableCollider();
    }

    public void DisableSensorCollider()
    {
        m_sensorMonster.DisableCollider();
    }
}
