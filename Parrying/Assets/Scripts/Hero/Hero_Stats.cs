using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    public float skillGage = 0f;
    private float parryingMultiplier = 1f;
    public Slider HealthSlider;
    public Slider SkillGageSlider;

    public float skillBonus;

    //체력 초기화
    private void Awake()
    {
        currentHealth = baseHealth;
        UpdateHealthSlider();
        UpdateSkillGageSlider();
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
        UpdateHealthSlider();
    }

    // 회복
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, baseHealth);
        UpdateHealthSlider();
    }

    //죽음 체크
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    // 스킬 게이지 
    public void SkillGageChange(float amount)
    {
        skillGage += amount;
        skillGage = Mathf.Clamp(skillGage, 0, 99);
        UpdateSkillGageSlider();
    }

    // 슬라이더 업데이트
    private void UpdateSkillGageSlider()
    {
        if (SkillGageSlider != null)
        {
            SkillGageSlider.value = skillGage / 99f;
        }
    }

    private void UpdateHealthSlider()
    {
        if (HealthSlider != null)
        {
            float healthPercent = currentHealth / baseHealth;
            HealthSlider.value = healthPercent;
        }
    }

}
