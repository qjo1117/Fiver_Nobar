﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private PlayerManager m_player = null;

    public PlayerManager Player { get { return m_player; } }

    private MonsterManager m_monster = null;

    public MonsterManager Monster { get { return m_monster; } }

    public void Init()
    {
        // 이건 조금 생각해야할게 객체로 들고 있을 필요가 있을지가 의문
        // 구조적으로 이건 조금 생각해보자
        {
            GameObject obj = new GameObject { name = "PlayerManager" };
            m_player = obj.AddComponent<PlayerManager>();
        }

        {
            GameObject obj = new GameObject { name = "MonsterManager" };
            m_monster = obj.AddComponent<MonsterManager>();
        }


    }

	public void Update()
    {
        
    }
}
