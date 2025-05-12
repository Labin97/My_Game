using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static GameManager instance;
    private float totalSoulPoint = 0;
    private int stageIndex;
    public GameObject[] stages;
    public Hero hero;
    private Animator hero_animator;
    public GameObject background;
    private float backgroundChangeDuration = 2f;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (hero != null)
            hero_animator = hero.GetComponent<Animator>();
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    
    private void Start()
    {
        AudioManager.Instance.PlayBGM("BGM_Stage1"); // 배경음악 재생
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
        StartCoroutine(NextStage());
    }
    
    IEnumerator NextStage()
    {
        yield return new WaitForSeconds(2f); // 2초 대기
        int currentStageIndex = stageIndex;
        if (currentStageIndex < stages.Length - 1)
        {
            // 스테이지 클리어 처리, 걷기 애니메이션, 배경 이동
            changeBackground(backgroundChangeDuration); // 배경 이동
            yield return new WaitForSeconds(backgroundChangeDuration + 1);
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

    private void changeBackground(float duration)
    {
        hero_animator.SetBool("Run", true); // 걷기 애니메이션 시작
        // 이동 중에는 플레이어 컨트롤 비활성화
        hero.SetControlEnabled(false);
        hero.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        
        // 배경의 현재 위치 저장
        Vector3 startPos = background.transform.position;
        // 이동할 목표 위치 (왼쪽으로)
        Vector3 targetPos = new Vector3(startPos.x - 10f, startPos.y, startPos.z);
        
        // DOTween으로 배경을 왼쪽으로 이동
        background.transform.DOMoveX(targetPos.x, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                // 이동이 완료되면 컨트롤 다시 활성화
                hero.SetControlEnabled(true);
                hero_animator.SetBool("Run", false);
                
                // 배경을 다시 원위치로 설정 (시각적으로 연속되도록)
                background.transform.position = startPos;
            });
    }
}