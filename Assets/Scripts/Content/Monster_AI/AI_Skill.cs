using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Skill : BT_Action
{
    protected int m_id;            //스킬 ID
    protected float m_coolTime;    //전체 쿨타임
    protected float m_coolDown;    //현재 쿨다운
    protected float m_range;       //스킬 사거리 - 사거리 없는거면 -1넣으셈
    protected int m_priority;      //스킬 우선도

    public int ID => m_id;
    public float CoolTime => m_coolTime;
    public float CoolDown { get => m_coolDown; set => m_coolDown = value; }

    public float Range => m_range;
    public int Priority => m_priority;
}