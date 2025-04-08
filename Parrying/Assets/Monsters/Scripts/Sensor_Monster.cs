using UnityEngine;
using System.Collections;

public class Sensor_Monster : MonoBehaviour
{
    public float invincibleDuration = 1.0f;
    private float m_invincibleTimer = 0f;
    private Animator            m_animator;
    private Collider2D          m_sensorCollider;

    private void OnEnable()
    {
        // 비활성 했다가 다시 켜는 상황에서 초기화
    }

    private void Awake()
    {
        m_sensorCollider = GetComponent<Collider2D>();
        m_animator = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 무적 시간 안이면 무시
        if (m_invincibleTimer > 0f)
            return;

        // 몬스터 애니메이터 트리거
        m_animator.SetTrigger("Hurt");

        // 무적 타이머 시작
        m_invincibleTimer = invincibleDuration;
    }

    void Update()
    {
        if (m_invincibleTimer > 0f)
            m_invincibleTimer -= Time.deltaTime;
    }

    // 애니메이션 이벤트에서 호출할 메서드
    public void EnableCollider()
    {
        m_sensorCollider.enabled = true;
    }

    public void DisableCollider()
    {
        m_sensorCollider.enabled = false;
    }
}
