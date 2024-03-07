using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    [System.Serializable]
    public class EffectInfo //스킬 이펙트정보
    {
        public GameObject Effect;
        public Transform StartPosRot;
        public float DestroyAfter = 10;
        public bool UseLocalPosition = true;
    }

    [SerializeField] private Skill_SO skill;
    public EffectInfo[] effects;  //스킬 이펙트정보

    // 콤보 스킬 전용 필드//
    protected int comboCount = -1; //콤보 카운트
    private float startComboTime; //콤보 시작 시간
    private float nextComboTime = 1f;  //다음 콤보 입력까지 기다릴 시간
    private float inputComboTime;   //콤보 입력 대기 시간

    // 플레이어 컴포넌트 //
    private PlayerState playerState;
    private Animator ani; // 플레이어 Animator 컴포넌트

    // 속성 //
    public Skill_SO Skill { get { return skill; } }
    public int ComboCount { get { return comboCount; } }

    public void InitializeComboCount()
    {
        comboCount = -1;
    }
    public AnimationClip Clip()  //사용할 애니메이션 클립 전송할 메서드
    {
        if (skill.Clips.Length == 1)   //단일 스킬이라면
            return skill.Clips[0];
        else
            return skill.Clips[comboCount];
    }

    public void DoAction()  //스킬 수행 메서드
    {
        if (playerState == null)
        {
            playerState = PlayerState.Instance;
            ani = playerState.GetComponent<Animator>();
        }

        switch (skill.Type)
        {
            case Skill_SO.SkillType.atOnce:
                AtOnceSkill();
                break;
            case Skill_SO.SkillType.combo:
                ComboSkill();
                break;
            case Skill_SO.SkillType.charge:
                ChargeSkill();
                break;
        }
    }

    protected void AtOnceSkill()    // 단일 스킬 메서드
    {
        ani.SetInteger(PlayerAniVariable.skillType, 0);
        ani.SetFloat(PlayerAniVariable.basicSkillNum, skill.SkillNum);
        playerState.UseMana(skill.NeedMana);
    }
    protected void ComboSkill()     // 콤보 스킬 메서드
    {
        playerState.GetComponent<PlayerQuickSlotHandler>().doNextCombo = false;
        if (comboCount >= skill.Clips.Length - 1 || Time.time > inputComboTime)  //콤보 카운트가 애니메이션 수보다 많거나 다음콤보입력시간을 지나면
            comboCount = -1;

        comboCount++;
        ani.SetInteger(PlayerAniVariable.skillType, 1);
        ani.SetInteger(PlayerAniVariable.comboSkillNum, (int)skill.SkillNum);
        ani.SetInteger(PlayerAniVariable.comboCount, comboCount);
        startComboTime = Time.time;
        inputComboTime = startComboTime + skill.Clips[comboCount].length + nextComboTime;

        playerState.UseMana(skill.NeedMana);
    }
    protected void ChargeSkill()    // 차지 스킬 메서드
    {
        ani.SetInteger(PlayerAniVariable.skillType, 2);
    }

    public void InstantiateEffect(int EffectNumber) //스킬 이펙트 생성 메서드
    {
        if (effects == null || effects.Length <= EffectNumber)
        {
            Debug.LogError("Incorrect effect number or effect is null");
            return;
        }

        var instance = Instantiate(effects[EffectNumber].Effect, effects[EffectNumber].StartPosRot.position, effects[EffectNumber].StartPosRot.rotation);

        if (effects[EffectNumber].UseLocalPosition)
        {
            instance.transform.parent = effects[EffectNumber].StartPosRot.transform;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = new Quaternion();
        }
        Destroy(instance, effects[EffectNumber].DestroyAfter);
    }

    public void InstantiateEffect(int EffectNumber, Transform start) //스킬 이펙트 생성 메서드 - 오버로딩
    {
        if (effects == null || effects.Length <= EffectNumber)
        {
            Debug.LogError("Incorrect effect number or effect is null");
            return;
        }
        effects[EffectNumber].StartPosRot = start;
        var instance = Instantiate(effects[EffectNumber].Effect, effects[EffectNumber].StartPosRot.position, effects[EffectNumber].StartPosRot.rotation);

        if (effects[EffectNumber].UseLocalPosition)
        {
            instance.transform.parent = effects[EffectNumber].StartPosRot.transform;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = new Quaternion();
        }
        Destroy(instance, effects[EffectNumber].DestroyAfter);
    }

    public void InstantiateEffect(int EffectNumber, Vector3 pos) //스킬 이펙트 생성 메서드 - 오버로딩
    {
        if (effects == null || effects.Length <= EffectNumber)
        {
            Debug.LogError("Incorrect effect number or effect is null");
            return;
        }
        effects[EffectNumber].StartPosRot.position = pos;
        var instance = Instantiate(effects[EffectNumber].Effect, effects[EffectNumber].StartPosRot.position, effects[EffectNumber].StartPosRot.rotation);

        if (effects[EffectNumber].UseLocalPosition)
        {
            instance.transform.parent = effects[EffectNumber].StartPosRot.transform;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = new Quaternion();
        }
        Destroy(instance, effects[EffectNumber].DestroyAfter);
    }
}
