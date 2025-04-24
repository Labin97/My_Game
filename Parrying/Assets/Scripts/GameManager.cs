using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private float totalSoulPoint = 0;
    private int stageIndex;
    public GameObject[] stages;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // MonsterManager의 이벤트 구독
        if (MonsterManager.Instance != null)
        {
            MonsterManager.Instance.OnAllMonstersDead += OnAllMonstersDead;
        }
    }
    
    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (MonsterManager.Instance != null)
        {
            MonsterManager.Instance.OnAllMonstersDead -= OnAllMonstersDead;
        }
    }
    
    // 모든 몬스터가 죽었을 때 호출되는 메서드
    private void OnAllMonstersDead()
    {
        Debug.Log("모든 몬스터가 처치되었습니다! 다음 스테이지로 진행합니다.");
        NextStage();
    }
    
    public void NextStage()
    {
        int currentStageIndex = stageIndex;
        if (currentStageIndex < stages.Length - 1)
        {
            // 스테이지 클리어 처리, 걷기 애니메이션, 배경 이동
            stages[currentStageIndex].SetActive(false);
            stages[currentStageIndex + 1].SetActive(true);
            stageIndex++; // 스테이지 인덱스 증가 추가
        }
        else
        {
            Debug.Log("모든 스테이지를 클리어했습니다!");
            // 게임 클리어 로직
        }
    }
    
    public void AddSoulPoint(float amount)
    {
        totalSoulPoint += amount;
    }
}