using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class General_AI_Combat_Alarm : BT_Action
{
    private GameObject m_object;
    private bool isalarm = true;
    private float alarm_range;

    public General_AI_Combat_Alarm(GameObject p_object,float p_alarm_range)
    {
        m_object = p_object;
        alarm_range = p_alarm_range;
    }
    public override State Update()
    {
        return OnAlarm();
    }

    public State OnAlarm()
    {
        if(isalarm)
        {
            List<GameObject> List = new List<GameObject>();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Monster");
            for(int i=0;i<objects.Length;i++)
            {
                float Distance = Vector3.Distance(m_object.transform.position, objects[i].transform.position);
                if(Distance<alarm_range)
                {
                    List.Add(objects[i]);
                }
            }
          
            //몬스터 그룹을 불러와서
            //거리를 잰뒤
            //그거리에 있는 몬스터만 스테이트를 바꾼다.
          
        }
        return State.SUCCESS;
    }
}
