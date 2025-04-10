using UnityEngine;
using System.Collections;

public class Hero_Stats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseAttackPower = 10f;
    public float baseHealth = 100f;
    public float damageMultiplier = 1f;
    public float attackSpeedMultiplier = 1f;
    public float minParryingBonus = 1.5f;
    public float maxParryingBonus = 2.0f;

    [Header("Runtime Stats")]
    public float currentHealth;
    
    private float parryingMultiplier = 1f;

    //체력 초기화
    private void Awake()
    {
        currentHealth = baseHealth;
    }

    // 공격력 계산
    public float GetCurrentAttackDamage()
    {
        float totalDamage = baseAttackPower * damageMultiplier * parryingMultiplier;
        parryingMultiplier = 1f;
        return totalDamage;
    }

    // 패링 공격력 계산
    public void ApplyParryingBonus(float bonus)
    {
        parryingMultiplier = bonus;
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
