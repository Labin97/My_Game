using UnityEngine;
using System.Collections;

public class Monster_Stats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseAttackPower = 10f;
    public float baseHealth = 100f;
    public float damageMultiplier = 1f;
    public float attackSpeedMultiplier = 1f;
    public int soulPoint = 10; // 소울
    public float DeathTime = 2f; // 죽는 시간
    public bool  SuperArmor = false;

    [Header("Runtime Stats")]
    public float currentHealth;
    

    //체력 초기화
    private void Awake()
    {
        currentHealth = baseHealth;
    }

    // 공격력 계산
    public float GetCurrentAttackDamage()
    {
        float totalDamage = baseAttackPower * damageMultiplier;
        return totalDamage;
    }

    // 공격 속도 보너스 적용
    public void ApplyAttackSpeedBonus(float speedMultiplier)
    {
        attackSpeedMultiplier += speedMultiplier;
    }

    // 공격력 보너스 적용
    public void ApplyAttackBonus(float attackMultiplier)
    {
        damageMultiplier += attackMultiplier;
    }

    // 체력 보너스 적용
    public void ApplyHealthBonus(float bonusHealth)
    {
        baseHealth += bonusHealth;
        currentHealth += bonusHealth;
    }

    // 데미지 받기
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, baseHealth);
    }

    // 회복
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, baseHealth);
    }

    //죽음 체크
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
