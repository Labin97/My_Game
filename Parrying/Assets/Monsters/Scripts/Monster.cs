using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Animator m_animator;
    private Monster_Stats          m_stats;
    private Sensor_Monster      m_sensorMonster;
    private bool isAttacking = false; 
    public bool IsAttacking => isAttacking; // 공격 중인지 여부를 외부에서 접근할 수 있도록 함
    private float   attackCooldown = 0f; // 공격 쿨타임
    public float minCooldown = 4f;
    public float maxCooldown = 6f;
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
        attackCooldown -= Time.deltaTime;

        if (IsEnemyInRange() && !IsAttacking && attackCooldown <= 0f)
        {
            int attackIndex = Random.Range(0, m_weaponColliders.Count);
            StartCoroutine(PlayAttackAnimation(attackIndex + 1));
            attackCooldown = Random.Range(minCooldown, maxCooldown); // 다음 공격까지 대기 시간
        }
    }

    IEnumerator PlayAttackAnimation(float attackType)
    {
        isAttacking = true; // 공격 시작
        m_animator.speed = m_stats.attackSpeedMultiplier;
        m_animator.SetTrigger("Attack" + attackType);

        //애니메이션 기다림
        AnimatorClipInfo[] clipInfos = m_animator.GetCurrentAnimatorClipInfo(0);
        //만약 재생중인 애니메이션이 있다면 그 길이를 가져옴. 없다면 0.5초 반환
        float clipLength = clipInfos.Length > 0 ? clipInfos[0].clip.length : 0.5f; // fallback 0.5초
        yield return new WaitForSeconds(clipLength / m_stats.attackSpeedMultiplier);

        isAttacking = false; // 공격 종료
        m_animator.speed = 1.0f;
    }

    // 적이 범위 내에 있는지 확인하는 메서드
    private bool IsEnemyInRange()
    {
        return true;
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
