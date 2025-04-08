using UnityEngine;
using System.Collections;

public class Sensor_Hero : MonoBehaviour
{
    public float invincibleDuration = 1.0f;
    private float m_invincibleTimer = 0f;
    private Animator            m_animator;

    private void OnEnable()
    {
        // 초기화
    }

    private void Awake()
    {
        m_animator = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 무적 시간 안이면 무시
        if (m_invincibleTimer > 0f)
            return;

        // 공격 받았을 때
        if (m_animator != null && !m_animator.GetBool("Guard"))
        {
           m_animator.SetTrigger("Hurt");
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

