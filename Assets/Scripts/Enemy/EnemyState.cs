using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyState : MonoBehaviour
{
    /// <summary> Idle : 대기, Patrol : 이동대기, Follow : 추격, Return : 원위치, Attack : 공격, Die : 죽음 </summary>
    public enum State
    {
        Idle, Patrol, Follow, Return, Attack, Die
    }

    // 컴포넌트 //
    [SerializeField] protected Transform bodyMesh;
    protected Animator ani;   //애니메이션 컨트롤 
    protected NavMeshAgent nav;   //네비게이션 컴포넌트. 얘를 사용하려면 이너미에게 NavMesh 컴포넌트가 부착되어있어야하며, NavMesh를 Bake할 맵들은 static형태의 오브젝트여야한다.
    protected Rigidbody rb;   //리지드바디 컴포넌트
    protected TargetRange targetRange;   //타겟 인식 콜라이더 스크립트
    protected QuestManager questManager;

    // 이벤트 //
    public UnityAction ev_OnDropItem;

    [SerializeField] protected string enemyName;  //이너미 이름

    [Header("퀘스트 관련")]
    public List<int> questNumber = new List<int>();    //퀘스트 번호
    [SerializeField] protected List<MiniQuestData> quests = new List<MiniQuestData>();

    [Header("이동 속도 및 거리 관련")]
    [SerializeField] protected float normalSpeed; //기본 이동 속도
    [SerializeField] protected float turnSpeed;   //회전할때 속도
    public Vector3 startPos;  //처음 스폰될 위치
    protected float distanceToMyPos;    //원래 위치와의 거리
    [SerializeField] protected float fallowDistance;  //플레이어를 따라갈 수 있는 최대 거리
    [SerializeField] protected float returnSpeed;  //제자리로 돌아갈 때 이동 속도

    [Header("공격, 스킬 관련")]
    [SerializeField] protected EnemySkill[] skills;   //스킬들
    [SerializeField] protected int[] skillSelectWeight;  //해당 스킬 선택 가중치
    protected int[] skillRandemWeight; //가중치들 랜덤함수에 맞게 변환
    protected int sumSkillWeight;  //스킬 가중치 합계
    protected EnemySkill selectedSkill;   //랜덤 돌려서 선택한 스킬
    protected bool isAttacking;   //공격다했는지 확인. 공격메서드 한번만 실행하게 하기 위함.

    [Header("체력 관련")]
    [SerializeField] protected float startLife; //시작 체력
    [SerializeField] protected float life; //현재 체력
    [SerializeField] GameObject HpBar;  //체력 바

    [Header("상태 관련")]
    [SerializeField] protected bool isPatrol = false; //idle상태를 순찰로 할지 대기로 할지 결정
    protected State nowState;    //현재 상태
    protected State pastState;    //과거 상태
    protected bool isDeath;   //죽었는지 확인
    protected Collider enemyBody; //죽는 순간 몸체를 트리거로 만듬

    // 애니메이션 파라미터 변수 //
    protected const string aniParameter_EnemyState = "EnemyState";    //이너미 상태

    // 속성 //
    public string EnemyName { get { return enemyName; } }
    public bool IsDeath { get { return isDeath; } } //isDeath 속성
    public float StartLife { get { return startLife; } }
    public float Life { get { return life; } }

    #region 시야범위 확인 메서드(지금은 안쓰지만 미래를 위해)
    /*
     *     [SerializeField] private LayerMask targetMask;  // 타겟 마스크(플레이어)
    private bool View() //공격을 위해 플레이어가 시야범위 안에 있는지 확인하는 메서드
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);  // z 축 기준으로 시야 각도의 절반 각도만큼 왼쪽으로 회전한 방향 (시야각의 왼쪽 경계선)
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);  // z 축 기준으로 시야 각도의 절반 각도만큼 오른쪽으로 회전한 방향 (시야각의 오른쪽 경계선)
        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        Collider[] _target = Physics.OverlapSphere(transform.position, attackDistance, targetMask);
        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;

            if (_targetTf.tag == "Player")
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);
                if (_angle < viewAngle * 0.5f)
                {
                    Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                    return true;
                }
            }
        }
        return false;
        // 참고자료 : https://ansohxxn.github.io/unity%20lesson%203/ch7-3/   //
    }

    private Vector3 BoundaryAngle(float _angle) //View()메서드의 DrawRay를 위한 함수
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
        // 참고자료 : https://ansohxxn.github.io/unity%20lesson%203/ch7-3/   //
    }
        */
    #endregion

    #region 애니메이션 이벤트 메서드
    public void OnAttackCollider_Ani()   //애니메이션 이벤트로 설정하는 메서드. 공격 콜라이더 제어
    {
        selectedSkill.RealAttack = true;
    }

    public void OnEffect_Ani()
    {
        if (selectedSkill.AttackEffect != null)
        {
            GameObject effect = Instantiate(selectedSkill.AttackEffect, this.transform);
            Destroy(effect, 3f);
        }
    }

    public void OnOffIsCanMove_Ani()    //애니메이션 이벤트로 설정하는 메서드. 움직임 제어
    {
        selectedSkill.IsCanMove = !selectedSkill.IsCanMove;
    }

    public void AttackComplete_Ani()    //애니메이션 이벤트로 설정하는 메서드. 공격 완료 처리
    {
        selectedSkill = null;
        isAttacking = false;
    }

    public void ResetBodyMesh() //바디 메쉬 원래 방향으로 돌리는 메서드
    {
        if (bodyMesh != null)
            bodyMesh.localRotation = Quaternion.identity;
    }
    #endregion

    private void FreezeVelocity()   //리지드바디의 물리력이 네비게이션 이동을 방해하지 않도록 하는 메서드
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    protected void MoveToTarget() //타겟을 향해 이동하는 메서드
    {
        ani.SetInteger(aniParameter_EnemyState, 1);
        nav.speed = normalSpeed;
        nav.stoppingDistance = selectedSkill.AttackDistance;
    }

    protected void ReturnMyPos()  //제자리로 돌아가는 메서드
    {
        nav.speed = returnSpeed;
        nav.stoppingDistance = 0;
        nav.SetDestination(startPos);
        ani.SetInteger(aniParameter_EnemyState, 1);
        if (nav.remainingDistance < 0.5f)
        {
            nowState = State.Idle;
            targetRange.IsInitalized = true;

            if (pastState != nowState && !isDeath)
            {
                pastState = State.Idle;
                life = startLife;
                nav.ResetPath();
                if (HpBar != null)
                    HpBar.SetActive(false);
            }
        }
    }

    protected void Attack()   //공격 모드 메서드
    {
        if (isAttacking == false)
        {
            isAttacking = true;
            ani.SetInteger(aniParameter_EnemyState, 2);
            nav.speed = 0f;
            selectedSkill.DoAttack(targetRange.Target);
        }
    }

    public virtual void TakeDamage(float damage)    //데미지를 입는 메서드
    {
        if (isDeath || nowState == State.Return)
            return;

        life -= damage;
        if (pastState != nowState && nowState == State.Attack)
        {
            pastState = nowState;
            if (HpBar != null)
                HpBar.SetActive(true);
        }

        if (life <= 0)
        {
            life = 0;
            nowState = State.Die;
            Die();
        }
    }

    protected virtual void Die()  //죽는 메서드
    {
        for (int i = 0; i < quests.Count; i++)
        {
            questManager.UpdateCurrentQuest(quests[i]);
        }
        ani.SetInteger(aniParameter_EnemyState, 4);
        isDeath = true;
        nav.speed = 0f;
        enemyBody.isTrigger = true;
        if (HpBar != null)
            HpBar.SetActive(false);
        ev_OnDropItem?.Invoke();
    }

    private EnemySkill RandomSkill()  //스킬 랜덤돌리기
    {
        int randomResult = Random.Range(0, sumSkillWeight);

        for (int i = 0; i < skills.Length; i++)
        {
            if (randomResult <= skillRandemWeight[i])
            {
                return skills[i];
            }
        }
        return null;
    }

    protected void CheckTargetAndSkill()    //타겟 확인 및 스킬 선택
    {
        if (targetRange.Target != null) //타겟이 있다면
        {
            nav.SetDestination(targetRange.Target.position);    //SetDestination(Vector3) : 도착할 목표 위치 지정 함수
            distanceToMyPos = Vector3.Distance(startPos, this.transform.position);  //원래위치에서 현재위치까지의 거리 계산

            if (distanceToMyPos > fallowDistance || targetRange.Target.GetComponent<PlayerState>().IsDeath == true && isAttacking == false) //원래자리로 되돌아가기
                nowState = State.Return;
            else
            {
                if (selectedSkill == null)  //선택된 스킬이 없다면 스킬 고르기 
                {
                    selectedSkill = RandomSkill();
                    nowState = State.Idle;
                }
                else
                {
                    if (nav.remainingDistance <= selectedSkill.AttackDistance) //선택한 스킬이 있고 타겟이 일정 거리 이내라면 공격
                        nowState = State.Attack;
                    else
                    {
                        if (isAttacking == false)   //타겟 따라가며 거리 좁히기
                            nowState = State.Follow;
                    }
                }
            }
        }
    }

    protected virtual void SetState() //상태 세팅하는 메서드
    {
        CheckTargetAndSkill();

        switch (nowState)
        {
            case State.Idle:
                ani.SetInteger(aniParameter_EnemyState, 0);
                break;
            case State.Patrol:
                break;
            case State.Follow:
                MoveToTarget();
                break;
            case State.Return:
                targetRange.Target = null;
                ReturnMyPos();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Die:
                Die();
                break;
        }
    }

    protected virtual void SetSkillWeight() //스킬 가중치 설정
    {
        int temp = 0;
        sumSkillWeight = 0;

        for (int i = 0; i < skills.Length; i++)
        {
            sumSkillWeight += skillSelectWeight[i];
            temp += skillSelectWeight[i];
            skillRandemWeight[i] = temp;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        questManager = QuestManager.Instance;

        ani = this.GetComponent<Animator>();
        nav = this.GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        targetRange = this.GetComponentInChildren<TargetRange>();
        enemyBody = this.GetComponent<Collider>();

        startPos = this.transform.position;
        nav.speed = normalSpeed;
        life = startLife;
        selectedSkill = null;
        isAttacking = false;

        //스킬 가중치 설정
        skillRandemWeight = new int[skills.Length];
        SetSkillWeight();

        // 타겟 퀘스트 세팅
        for (int i = 0; i < questNumber.Count; i++)
        {
            quests.Add(questManager.GetTargetQuests(questNumber[i]));
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDeath)
            return;

        FreezeVelocity();
    }

    private void LateUpdate()
    {
        SetState();
    }
}

[System.Serializable]
public class Phase
{
    public int pahsePercent;
    public EnemySkill skill;
}