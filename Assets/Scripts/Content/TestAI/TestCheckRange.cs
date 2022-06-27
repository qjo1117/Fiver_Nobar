using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCheckRange : Exacution
{
	private float m_range = 0.0f;

    public TestCheckRange(BehaviorTree p_tree) : base(p_tree)
	{

		m_range = m_tree.GetData<float>("CheckRange");
	}

	public override BehaviorStatus Update()
	{
		PlayerController target = m_tree.GetData<PlayerController>("Target");

		// Ÿ���� ������ ������ ����� �ִ��� üũ�Ѵ�.
		if (target == null) {
			// ����Ʈ�� ��ȸ�ؼ� �Ÿ��� ���Ѵ�.
			foreach(PlayerController player in Managers.Game.Player.List) {
				if(player.IsDead == true){
					continue;
				}

				Vector3 dist = player.transform.position - m_transform.position;
				// Sphere�ѱ�
				if(dist.sqrMagnitude <= m_range * m_range) {
					m_tree.SetData("Target", player);
					m_status = BehaviorStatus.Success;
					return m_status;
				}
			}

			// Ÿ���� ������ ������ ����
			m_status = BehaviorStatus.Failure;
		}
		// Ÿ���� ���� ���
		else {
			m_status = BehaviorStatus.Success;
		}

		return m_status;
	}
}