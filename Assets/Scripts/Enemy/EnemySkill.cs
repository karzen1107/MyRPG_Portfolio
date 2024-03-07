using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : MonoBehaviour
{
    protected Transform myPos; //몬스터 본인 트랜스폼
    private List<Collider> colliders = new List<Collider>();   //트리거에 들어온 콜라이더들

    [Header("Common Skill Porperty"), Space(5)]
    [SerializeField] protected float attackDamage; //공격력 
    [SerializeField] protected float attackCoolTime; //공격 쿨타임
    [SerializeField] protected float attackDistance; //공격 최대거리
    [SerializeField] protected int ani_skillNum;   //애니메이션 스킬 번호
    [SerializeField] private GameObject attackEffect; //어택 이펙트

    // 애니메이션 //
    protected Animator ani;
    protected const string aniParameter_SkillType = "SkillType";  //이너미 스킬번호

    // 속성 //
    public float AttackCoolTime { get { return attackCoolTime; } }
    public float AttackDistance { get { return attackDistance; } }
    public bool RealAttack { get; set; }
    public bool IsCanMove { get; set; }
    public GameObject AttackEffect { get { return attackEffect; } }

    public void DoAttack(Transform target)  //공격수행 외부 공개 메서드
    {
        Attack(target);
    }

    protected virtual void Attack(Transform target) //타겟을 바라보면서 공격하는 메서드
    {
        myPos.LookAt(target);
        ani.SetInteger(aniParameter_SkillType, ani_skillNum);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            colliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            colliders.Remove(other);
    }

    private void Awake()
    {
        ani = GetComponentInParent<Animator>();
        myPos = GetComponentInParent<EnemyState>().gameObject.transform;
    }

    private void FixedUpdate()
    {
        if (RealAttack)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                if (colliders[i].GetComponent<PlayerState>() != null)
                    colliders[i].GetComponent<PlayerState>().TakeDamage(attackDamage);
            }
            RealAttack = false;
        }
    }
}
