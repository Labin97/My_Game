using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moster : MonoBehaviour
{
    public WeaponCollider weapon;
    public Vector2 attackColliderSize = new Vector2(2.0f, 1.0f);
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))  // 예시: K를 누르면 공격
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }

    // 애니메이션 이벤트에서 호출할 함수
    public void StartAttack()
    {
        weapon.SetColliderSize(attackColliderSize);
    }

    public void EndAttack()
    {
        weapon.ResetColliderSize();
    }
}
