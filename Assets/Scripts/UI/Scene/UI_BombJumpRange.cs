using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BombJumpRange : UI_BombRange
{
    #region ����
    private Camera           m_mainCam;
    private UI_BombJumpArrow m_bombJumpArrow;           // ��ź ���� ȭ��ǥ UI (��ź ���� ȭ��ǥ UI ǥ������ �ʿ���)
    #endregion

    public override void Init()
    {
        base.Init();
        m_mainCam = Camera.main;
    }


    // ���콺 ��ġ ��ź���� ��Ÿ� ������ üũ
    public bool BombJumpRangeInnerCheck(Vector3 _point)
    {
        Vector3 l_subVector = _point - transform.position;                      // �÷��̾�� ���콺 ��ġ���� �Ÿ� Vector
        Vector3 l_maxRangeVector = l_subVector.normalized * m_rangeRadius;      // ��ź ���� �ִ� ��Ÿ� Vector

        // ���� ���콺�� ��ġ�� ��ź���� ��Ÿ� ���ΰ��
        if (l_subVector.sqrMagnitude > l_maxRangeVector.sqrMagnitude)
        {
            // ȭ��ǥ �̹����� �������� �ִ°�� Close
            if (m_bombJumpArrow != null)
            {
                m_bombJumpArrow.CloseSceneUI();
                m_bombJumpArrow = null;
            }
            return false;
        }
        else
        {
            // ȭ��ǥ �̹����� ǥ�õ��� �ʴ°�� Show�ϰ�, ���콺������ ��ġ Vector + ������ Vector�� (offset) ��ġ�� Position ����
            if (m_bombJumpArrow == null)
            {
                m_bombJumpArrow = Managers.UI.ShowSceneUI<UI_BombJumpArrow>("UI_BombJumpArrow");
            }

            m_bombJumpArrow.SetPosition(m_mainCam.WorldToScreenPoint(_point + new Vector3(1.0f,1.0f,1.0f)));
            return true;
        }
    }
}