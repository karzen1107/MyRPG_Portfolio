using UnityEngine;
/// <summary> 플레이어의 퀵슬롯에 등록된 스킬,아이템을 실행해주는 클래스 </summary>
public class PlayerQuickSlotHandler : MonoBehaviour
{
    // 스킬 관련 //
    private BaseSkill actionSkill;    //실행할 스킬
    private AnimationClip lastAniclip; //진행중인 마지막 애니메이션
    private float waitAniTime; //애니메이션 타이머
    private float setAniTime;   //세팅한 애니메이션 타이머
    [HideInInspector] public bool doNextCombo;   //다음 콤보 진행을 위한 변수

    // 플레이어 접근 컴포넌트 //
    private PlayerMove playerMove;  //PlayerMove 컴포넌트
    private PlayerState playerState;    //PlayerState 컴포넌트
    private Animator ani;   //플레이어 애니메이터 컴포넌트

    public void ReceiveSkillAction(BaseSkill actionObj) //퀵슬롯 키코드 인풋 시 
    {
        if (playerMove == null)
            playerMove = this.GetComponent<PlayerMove>();

        if (playerMove.IsGrounded == false) //플레이어가 땅에서 떨어져있다면 퀵슬롯 사용 불가
            return;

        if (actionObj.GetComponent<BaseSkill>()) //스킬 컴포넌트를 갖고있다면 스킬사용
        {
            LookAtCamView();
            actionSkill = actionObj.GetComponent<BaseSkill>();
            if (lastAniclip == null)    // 진행중인 스킬애니메이션이 없다면
            {
                InitializedAction();
                DoSkill(actionObj);
            }

            if (lastAniclip != null && actionSkill.Skill.Type == Skill_SO.SkillType.combo && doNextCombo)  //스킬 진행중이고 콤보 스킬의 경우
            {
                InitializedAction();
                DoSkill(actionObj);
            }
        }
    }

    private void LookAtCamView()    //스킬 사용시 항상 전방을 보도록하는 메서드
    {
        Vector3 camDir = playerMove.PlayerCamera.forward;
        camDir.y = 0;
        this.gameObject.transform.rotation = Quaternion.LookRotation(camDir);
    }

    public bool ReceiveItemAction(Item item)    //아이템 사용시 적용
    {
        if (playerState == null)
            playerState = PlayerState.Instance;

        switch (item.EffectType)
        {
            case EffectType.Hp:
                if (playerState.Life == playerState.StartLife)
                {
                    Debug.Log("생명령이 가득 찼습니다");
                    return false;
                }
                else
                {
                    playerState.RecoverHp(item.ItemAbillity);
                    return true;
                }
            case EffectType.Mp:
                if (playerState.Mana == playerState.StartMana)
                {
                    Debug.Log("마나가 가득 찼습니다");
                    return false;
                }
                else
                {
                    playerState.RecoverMp(item.ItemAbillity);
                    return true;
                }
            case EffectType.Atk:
                playerState.NowAtk += item.ItemAbillity;
                return true;
            case EffectType.Def:
                playerState.NowShield += item.ItemAbillity;
                return true;
            default:
                return false;
        }
    }

    #region 애니메이션 이벤트 메서드
    public void DoNextCombo_Ani()   //다음 콤보 실행 메서드
    {
        doNextCombo = true;
    }

    public void OnInvincibility_Ani()  //무적모드 활성화 메서드 (Invincibility = 무적) - 레이어 충돌 무시
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Player"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("NPC"), LayerMask.NameToLayer("Player"), true);
    }

    public void OffInvincibility_Ani()  //무적모드 비활성화 메서드 (Invincibility = 무적) - 레이어 충돌 무시 취소
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Player"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("NPC"), LayerMask.NameToLayer("Player"), false);
    }

    public void OnSkillEffect_Ani() //스킬 이펙트 수행 메서드
    {

        if (actionSkill.Skill.Type == Skill_SO.SkillType.combo) //콤보 공격이라면
        {
            if (actionSkill.Skill.Dis == Skill_SO.SkillDis.shortDis)    //근거리 공격이라면
                actionSkill.InstantiateEffect(actionSkill.ComboCount);
            else  //원거리 공격이라면
            {
                if (playerState == null)
                    playerState = PlayerState.Instance;

                if (playerState.Target != null) //에이밍한 타겟이 있다면
                    actionSkill.InstantiateEffect(actionSkill.ComboCount, playerState.Target.transform);
                else if (playerState.TargetPos != null)  //에이밍한 타겟이 없다면(땅이라면)
                    actionSkill.InstantiateEffect(actionSkill.ComboCount, playerState.TargetPos);
            }
        }
        else  //단일 공격이라면
        {
            if (actionSkill.Skill.Dis == Skill_SO.SkillDis.shortDis)    //근거리 공격이라면
                actionSkill.InstantiateEffect(0);
            else  //원거리 공격이라면
            {
                if (playerState == null)
                    playerState = PlayerState.Instance;

                if (playerState.Target != null) //에이밍한 타겟이 있다면
                    actionSkill.InstantiateEffect(0, playerState.Target.transform);
                else if (playerState.TargetPos != null)  //에이밍한 타겟이 없다면(땅이라면)
                    actionSkill.InstantiateEffect(0, playerState.TargetPos);
            }
        }
    }
    #endregion

    private void DoSkill(BaseSkill skill)   //스킬 사용
    {
        skill.DoAction();
        lastAniclip = skill.Clip();   //스킬의 애니메이션 클립 저장
        setAniTime = lastAniclip.length * 0.9f;   //애니메이션 타이머 시간 저장
        waitAniTime = 0;
        ani.SetBool(PlayerAniVariable.useSkill, true);
        ani.SetLayerWeight(PlayerAniVariable.DrawWeaponLayer, 0);
        ani.SetLayerWeight(PlayerAniVariable.SheathWeaponLayer, 0);
    }

    private void InitializedAction()    //액션 초기화
    {
        ani.SetBool(PlayerAniVariable.useSkill, false);
        lastAniclip = null;
        waitAniTime = 0;
    }

    private void Awake()
    {
        playerMove = this.GetComponent<PlayerMove>();
        ani = this.GetComponent<Animator>();
    }

    private void Start()
    {
        playerState = PlayerState.Instance;
    }

    private void Update()
    {

        if (ani.GetBool(PlayerAniVariable.useSkill))   //useSkill이 true면  초기화
        {
            waitAniTime += Time.deltaTime;
            if (waitAniTime >= setAniTime)
            {
                InitializedAction();
                if (ani.GetFloat(PlayerAniVariable.attackMode) < 0.1f && !ani.GetBool(PlayerAniVariable.useSkill))
                    PlayerState.Instance.SetNormalMode();
            }

        }
    }
}
