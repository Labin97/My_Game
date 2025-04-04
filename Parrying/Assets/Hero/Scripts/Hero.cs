using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

    private Animator            m_animator;
    private Sensor_Hero         m_hitSensor;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;
    private float m_invincibleTimer = 1.0f;


    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
        m_hitSensor = transform.Find("HitSensor").GetComponent<Sensor_Hero>();
    }
	
	// Update is called once per frame
	void Update () {
        //Hurt
        if (m_hitSensor.State())
        {
            m_animator.SetTrigger("Hurt");
            m_hitSensor.Disable(m_invincibleTimer);
        }

        //Animation Lock
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag("Lock"))
            return;

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // -- Handle Animations --

        //Guard
        bool isGuarding = Input.GetMouseButton(1);
        m_animator.SetBool("Guard", isGuarding);

        //Attack
        if(Input.GetMouseButtonDown(0)) {
            m_animator.SetTrigger("Attack");
        }

        //Parrying
        else if(Input.GetKeyDown("f"))
            m_animator.SetTrigger("Parrying");

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
}
