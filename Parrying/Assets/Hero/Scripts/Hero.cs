using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

    private Animator            m_animator;
    private Hero_Stats          m_stats;
    private WeaponCollider      m_weaponCollider;
    private Sensor_Hero         m_sensorHero;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;
    public bool isGuarding { get; private set; } = false;
    public bool isParrying { get; private set; } = false;
    public bool isPerfectParrying { get; private set; } = false;



    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
        m_stats = GetComponent<Hero_Stats>();
        m_weaponCollider = GetComponentInChildren<WeaponCollider>();
        m_sensorHero = GetComponentInChildren<Sensor_Hero>();
    }
	
	// Update is called once per frame
	void Update () {
        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        bool canMove = CanMove();

        // Swap direction
        if (canMove)
        {
            if (inputX > 0)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            else if (inputX < 0)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        // -- Handle Animations --


        //Guard
        if (canMove)
        {
            isGuarding = Input.GetMouseButton(1);
            m_animator.SetBool("Guard", isGuarding);
        }
        else
        {
            isGuarding = false;
            m_animator.SetBool("Guard", false);
        }

        if (!isGuarding)
        {
            //Attack
            if(Input.GetMouseButtonDown(0)) {
                //공격 속도 적용
                StartCoroutine(PlayAttackAnimation());
            }

            //Parrying
            if(Input.GetKeyDown("q")) 
            {
                StartParrying();
            }
            if (Input.GetKeyUp("q")) 
            {
                EndParrying();
            }
        }

        //Death
        else if (Input.GetKeyDown("e")) {
            if(!m_isDead)
                m_animator.SetTrigger("Death");
            else
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }

        //Change between idle and combat idle
        else if (Input.GetKeyDown("p"))
            m_combatIdle = !m_combatIdle;

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);

    }

    //공격 코루틴
    IEnumerator PlayAttackAnimation()
    {
        m_animator.speed = m_stats.attackSpeedMultiplier;
        m_animator.SetTrigger("Attack");


        //애니메이션 기다림
        AnimatorClipInfo[] clipInfos = m_animator.GetCurrentAnimatorClipInfo(0);
        //만약 재생중인 애니메이션이 있다면 그 길이를 가져옴. 없다면 0.5초 반환
        float clipLength = clipInfos.Length > 0 ? clipInfos[0].clip.length : 0.5f; // fallback 0.5초
        yield return new WaitForSeconds(clipLength / m_stats.attackSpeedMultiplier);

        m_animator.speed = 1.0f;
    }

    private bool CanMove()
    {
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        return !stateInfo.IsName("Attack") && !stateInfo.IsName("Hurt") &&
            !stateInfo.IsName("Death") && !stateInfo.IsName("Recover") &&
            !stateInfo.IsName("Parrying") && !stateInfo.IsName("Parrying1") &&
            !stateInfo.IsName("Parrying2") && !stateInfo.IsName("Parrying3");
    }

    public bool IsFacingAttacker(Transform attacker)
    {
        float heroFacing = -transform.localScale.x;
        float directionToAttacker = Mathf.Sign(attacker.position.x - transform.position.x);
        return (heroFacing * directionToAttacker) > 0f;
    }


    //공격 애니메이션 용 이벤트
    public void EnableWeaponCollider()
    {
        m_weaponCollider.EnableCollider();
    }

    public void DisableWeaponCollider()
    {
        m_weaponCollider.DisableCollider();
    }

    //패링
    private void StartParrying()
    {
        isParrying = true;
        m_animator.SetBool("IsParrying", true);
    }

    private void EndParrying()
    {
        isParrying = false;
        m_animator.SetBool("IsParrying", false);
    }

    private void StartPerfectParrying()
    {
        isPerfectParrying = true;
        m_animator.SetBool("PerfectParrying", true);
    }

    private void EndPerfectParrying()
    {
        isPerfectParrying = false;
        m_animator.SetBool("PerfectParrying", false);
    }

    private void SetParryLevel(int level)
    {
        m_animator.SetInteger("ParryLevel", level);

        if (level == 1)
            m_stats.ApplyParryingBonus(0.7f);
        else if (level == 2)
            m_stats.ApplyParryingBonus(m_stats.minParryingBonus);
        else if (level == 3)
            m_stats.ApplyParryingBonus(m_stats.maxParryingBonus);
    }

    //퍼펙트 가드
    public void StartPerfectGuard()
    {
        m_sensorHero.StartPerfectGuard();
    }

    public void EndPerfectGuard()
    {
        m_sensorHero.EndPerfectGuard();
    }

}
