using System.Collections.Generic;
using UnityEngine;

public class AI_Range : Interface_Enemy
{
    public float m_detectRange = 10.0f;
    public float m_alarmRange = 10.0f;
    public float m_chaseMoveSpeed = 10.0f;
    public float m_patrolMoveSpeed = 10.0f;
    public Transform m_muzzle = null;


    protected override void CreateBehaviorTreeAIState()
    {

        m_enemyType = AI.EnemyType.Melee;
        m_brain = new BT_Root();
        m_selectedSkill = null;
        m_isSkillSelected = false;
        m_isChaseComplete = false;

        // ���� ������
        BT_Selector l_mainSelector = new BT_Selector();

        BT_Sequence l_DeathSQ = new BT_Sequence();
        l_DeathSQ.AddChild(new Condition_IsDeath(gameObject, Stat));
        l_DeathSQ.AddChild(new Action_Death(gameObject, 2.0f));
        l_mainSelector.AddChild(l_DeathSQ);

        // Gameobject
        // SkillId
        // CoolTime
        // Range
        // Priority
        // ETC
        Condition_SkillSelector l_skillselector = new Condition_SkillSelector(gameObject);
        l_skillselector.AddSkill(new Skill_Range(gameObject, 0101, 1, 8, 1,
            10, 0.8f, 10, 5.0f,m_muzzle));

        BT_Sequence l_ReadyforSkillSQ = new BT_Sequence();
        l_ReadyforSkillSQ.AddChild(new Condition_PlayerDetect(gameObject, m_detectRange));
        l_ReadyforSkillSQ.AddChild(l_skillselector);
        l_ReadyforSkillSQ.AddChild(new Condition_IsOutofSkillRange(gameObject));
        l_ReadyforSkillSQ.AddChild(new Action_Chase(gameObject, m_chaseMoveSpeed));
        l_mainSelector.AddChild(l_ReadyforSkillSQ);

        BT_Sequence l_UseSkillSQ = new BT_Sequence();
        l_UseSkillSQ.AddChild(new Condition_IsSkillRuning(gameObject));
        l_UseSkillSQ.AddChild(new Action_SkillDelegator(gameObject));
        l_mainSelector.AddChild(l_UseSkillSQ);

        l_mainSelector.AddChild(new Action_WayPointPatrol(gameObject, m_patrolMoveSpeed, m_patrolWayPoint));

        m_brain.Child = l_mainSelector;
    }


}
