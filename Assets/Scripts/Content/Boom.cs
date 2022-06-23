using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class Boom : MonoBehaviour
{
    [SerializeField]
    private float               m_moveSpeed = 400.0f;               // 폭탄의 가해지는 힘
    [SerializeField]
    private float               m_explosionRange = 5.0f;            // 폭발 반경
    [SerializeField]
    private float               m_detectRange = 2.0f;               // 감지 반경

    // 이것만 필요할꺼같아서 어빌리티는 안씀
    private float               m_explosionDelayTime = 0.0f;        // 현재 지연 시간
    [SerializeField]
    private float               m_explosionMaxDelayTime = 5.0f;     // 최대 지연 시간
    private bool                m_isExplosion = false;

    private PlayerController    m_player = null;
    private Rigidbody           m_rigid = null;

    private int                 m_layer = 1 << (int)Layer.Monster | 1 << (int)Layer.Player;

    public void Create(Vector3 p_dir, PlayerController p_player)
	{
        m_player = p_player;

        m_rigid.AddForce(p_dir * m_moveSpeed);
    }

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ExplosionDaleyCheck();
        Explosion();
    }

    private void ExplosionDaleyCheck()
	{
        m_explosionDelayTime += Time.deltaTime;

        if(m_explosionMaxDelayTime > m_explosionDelayTime) {
            return;
		}

        m_isExplosion = true;
    }



    private void Explosion()
	{
        if(m_isExplosion == false) {
            return;
		}

        // 이 부분은 벽체크 할지 여부가 결정되면 할꺼
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_explosionRange, (int)m_layer);

        // 이부분은 스탯이 확정나면 변경될꺼
        List<TargetData> listMonster = new List<TargetData>();
        List<TargetData> listPlayer = new List<TargetData>();

        foreach(Collider collider in colliders) {    
            // 몬스터 레이어인지
            if((collider.gameObject.layer & (int)Layer.Monster) == (int)Layer.Monster) {
                MonsterController monster = collider.GetComponent<MonsterController>();
                listMonster.Add(new TargetData(monster.ID, monster.Attack));
            }
            // 플레이어 레이어인지
            else if ((collider.gameObject.layer & (int)Layer.Player) == (int)Layer.Player) {
                PlayerController player = collider.GetComponent<PlayerController>();
                listPlayer.Add(new TargetData(player.ID, 30));

            }
        }

        foreach(TargetData data in listMonster) {
            Managers.Game.Monster.Attack(data.id, data.attack);
        }

        gameObject.SetActive(false);
    }


	private void OnDrawGizmos()
	{
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_explosionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_detectRange);
    }
}
