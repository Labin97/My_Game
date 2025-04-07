using UnityEngine;
using System.Collections;

public class Sensor_Monster : MonoBehaviour
{
    public float invincibleDuration = 1.0f;
    private float m_invincibleTimer = 0f;

    private void OnEnable()
    {
        // 초기화
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 무적 시간 안이면 무시
        if (m_invincibleTimer > 0f)
            return;

        // 몬스터 애니메이터 트리거
        Animator animator = GetComponentInParent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        // 무적 타이머 시작
        m_invincibleTimer = invincibleDuration;
    }

    void Update()
    {
        if (m_invincibleTimer > 0f)
            m_invincibleTimer -= Time.deltaTime;
    }
}
