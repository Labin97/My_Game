using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static MonsterManager instance;
    private List<Monster> activeMonsters = new List<Monster>();
    
    // 모든 몬스터가 죽었을 때 발생하는 이벤트
    public event System.Action OnAllMonstersDead;

    private bool startMonsterCheck = false;
    
    // 이전에 모든 몬스터가 죽었는지 상태를 저장
    private bool wasAllDead = false;

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
    }

    public static MonsterManager Instance
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

    private void Update()
    {
        // 몬스터가 있고 모두 죽었는지 확인
        bool allDead = AreAllMonstersDead();
        
        // 이전 상태와 현재 상태가 다를 때만 이벤트 발생
        if (!wasAllDead && allDead)
        {
            OnAllMonstersDead?.Invoke();
        }

        wasAllDead = allDead;
    }

    public void RegisterMonster(Monster monster)
    {
        startMonsterCheck = true;
        if (!activeMonsters.Contains(monster))
            activeMonsters.Add(monster);
        
        // 몬스터가 추가되면 상태 리셋
        wasAllDead = false;
    }
    
    public void UnregisterMonster(Monster monster)
    {
        activeMonsters.Remove(monster);
    }
    
    public bool AreAllMonstersDead()
    {
        if (!startMonsterCheck)
            return false; 

        if (activeMonsters.Count == 0)
            return true;
        
        foreach (Monster monster in activeMonsters)
        {
            if (!monster.IsDead())
                return false;
        }
        return true;
    }
}