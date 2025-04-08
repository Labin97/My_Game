using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    private Collider2D      m_weaponCollider;

    private void Awake()
    {
        m_weaponCollider = GetComponent<Collider2D>();
        m_weaponCollider.enabled = false; // 초기에는 비활성화
    }

    // 애니메이션 이벤트에서 호출할 메서드
    public void EnableCollider()
    {
        m_weaponCollider.enabled = true;
    }

    public void DisableCollider()
    {
        m_weaponCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DisableCollider();
    }
}
