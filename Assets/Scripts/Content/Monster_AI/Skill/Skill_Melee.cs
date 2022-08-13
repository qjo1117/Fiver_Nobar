using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class Skill_Melee : Interface_Skill
{
    private int m_damage = 10;
    private float m_skillPlayTime = 0.0f;

    private float m_attackAngle = 135f;
    private ParticleSystem m_particle = null;
    private List<GameObject> m_targets = new List<GameObject>();

    private bool m_isSkillEnd = false;
    private float m_timeCheck = 0.0f;

    private string m_animationFileName;
    private string m_effectFileName;

    public Skill_Melee(GameObject _object, int _id, float _coolTime, float _range, int _priority,
        int _damage, float _skillPlayTime, 
        string _animationFileName = "Dagger-Attack-R3", string _effectFileName = Path.Slash_Particle)
    {
        m_object = _object;
        m_animator = m_object.GetComponent<Animator>();
        m_id = _id;
        m_coolTime = _coolTime;
        m_coolDown = 0;
        m_rangeForCastSkill = _range;
        m_priority = _priority;

        m_damage = _damage;
        m_skillPlayTime = _skillPlayTime;

        m_isSkillEnd = false;
        m_timeCheck = 0;

        m_animationFileName = _animationFileName;
        m_effectFileName = _effectFileName;
    }

    public override void Initialize() { }
    public override void Terminate() { }

    public override AI.State Update()
    {
        return OnMeleeAttack();
    }

    private AI.State OnMeleeAttack()
    {


        if (!m_isSkillEnd) {
            if (m_particle == null) {
                m_particle = Managers.Resource.Instantiate(m_effectFileName, m_object.transform).GetComponent<ParticleSystem>();

                //위치 조정
                Vector3 vec = m_object.transform.position + m_object.transform.forward;
                //vec = new Vector3(vec.x, vec.y + 2, vec.z);
                vec.y += 2.0f;
                m_particle.transform.position = vec;

                //회전 회오리
                m_particle.transform.Rotate(Quaternion.FromToRotation(m_particle.transform.forward,
                    m_object.transform.forward).eulerAngles);

                m_particle.Stop();

                var l_particle = m_particle.GetComponent<ParticleSystem>().main;
                l_particle.duration = m_skillPlayTime;
                l_particle.startLifetime = m_skillPlayTime;

            }
            m_particle.Play();
            SetAnimation(m_animationFileName, 0.15f, m_skillPlayTime);
            m_isSkillEnd = true;
        }

        m_timeCheck += Time.deltaTime;

        if((int)m_skillPlayTime / 2.0f == m_timeCheck) 
        {
            FindTarget();
        }

        if (m_timeCheck >= m_skillPlayTime) 
        {
            m_timeCheck = 0;
            m_isSkillEnd = false;

            foreach (var player in m_targets) 
            {
                player.GetComponent<PlayerController>().Damage(m_damage);
            }
            return AI.State.SUCCESS;
        }

        return AI.State.RUNNING;
    }

    //범위 내의 타켓 탐색
    void FindTarget()
    {
        //특정 거리 내의 player 탐색
        Collider[] l_colliders = Physics.OverlapSphere(m_object.transform.position, m_rangeForCastSkill, 1 << (int)Define.Layer.Player);

        m_targets.Clear();//초기화

        float l_radianRange = Mathf.Cos(Mathf.Deg2Rad * (m_attackAngle / 2));//라디안 범위

        //범위 내의 player 타켓에 추가
        //foreach (var item in l_colliders)
        //{
        //    float targetRadian = Vector3.Dot(m_object.transform.forward,
        //        (item.transform.position - m_object.transform.forward).normalized);

        //    if (targetRadian < l_radianRange)
        //    {
        //        m_targets.Add(item.gameObject);
        //    }
        //}

        Vector3 lookDir = AngleToDir(m_object.transform.eulerAngles.y);
        foreach (var item in l_colliders)
        {
            Vector3 l_targetVector = item.transform.position - m_object.transform.position;

            float dot = Vector3.Dot(l_targetVector.normalized, m_object.transform.forward);
            float degree = Mathf.Rad2Deg * Mathf.Acos(dot);

            if (degree <= m_attackAngle * 0.5f)
            {
                m_targets.Add(item.gameObject);
            }
        }
    }

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }
}
