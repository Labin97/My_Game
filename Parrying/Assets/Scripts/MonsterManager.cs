using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }
    private List<Monster> activeMonsters = new List<Monster>();
    
    // 모든 몬스터가 죽었을 때 발생하는 이벤트
    public event System.Action OnAllMonstersDead;
    
    // 이전에 모든 몬스터가 죽었는지 상태를 저장
    private bool wasAllDead = false;
    
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
    
    private void Update()
    {
        // 디버그 정보 추가
        if (activeMonsters.Count > 0)
        {
            Debug.Log($"활성 몬스터 수: {activeMonsters.Count}");
            foreach (Monster monster in activeMonsters)
            {
                Debug.Log($"몬스터 {monster.name} 상태: {(monster.IsDead() ? "죽음" : "살아있음")}");
            }
        }
        
        // 몬스터가 있고 모두 죽었는지 확인
        bool allDead = AreAllMonstersDead();
        
        // 디버그 출력 추가
        Debug.Log($"모든 몬스터 죽음: {allDead}, 이전 상태: {wasAllDead}, 활성 몬스터 수: {activeMonsters.Count}");
        
        // 이전 상태와 현재 상태가 다를 때만 이벤트 발생
        if (!wasAllDead && allDead && activeMonsters.Count > 0)
        {
            Debug.Log("모든 몬스터가 죽었습니다!");
            OnAllMonstersDead?.Invoke();
        }
        
        wasAllDead = allDead;
    }
    
    public void RegisterMonster(Monster monster)
    {
        if (!activeMonsters.Contains(monster))
            activeMonsters.Add(monster);
        Debug.Log("몬스터 등록됨: " + monster.name);
        
        // 몬스터가 추가되면 상태 리셋
        wasAllDead = false;
    }
    
    public void UnregisterMonster(Monster monster)
    {
        activeMonsters.Remove(monster);
    }
    
    public bool AreAllMonstersDead()
    {
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