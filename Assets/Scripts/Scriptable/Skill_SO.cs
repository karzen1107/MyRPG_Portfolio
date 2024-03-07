using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/Skill")]
public class Skill_SO : ScriptableObject
{
    public enum SkillType  //스킬 시전 타입
    {
        atOnce, combo, charge
    }

    public enum SkillDis    //스킬 근거리, 원거리
    {
        shortDis, longDis
    }
    public enum DamageType   //단일 데미지, 초당 데미지
    {
        singleDamage, secDamage
    }

    // 기본 스킬 속성 //
    [SerializeField] private string skillName;    // 스킬 이름
    [SerializeField] private Sprite skillImage;    // 스킬UI 이미지
    [SerializeField] private GameObject skillPrefab;    //스킬 프리팹
    [SerializeField] private float skillNum;  // 스킬 블랜드트리 번호
    [SerializeField] private int skillLevel;  // 스킬 사용 가능 레벨
    [SerializeField] private float needMana; //필요한 마나
    [SerializeField] private float attackDamage;    //공격력
    [SerializeField] private SkillType skillType; //스킬 타입
    [SerializeField] private SkillDis skillDis; //원거리, 근거리
    [SerializeField] private DamageType damageType; //타겟형, 범위형
    [SerializeField] private AnimationClip[] clips;   // 스킬 애니메이션 클립
    [SerializeField] private float maxDistance; //스킬 최대 거리

    public string SkillName { get { return skillName; } }
    public Sprite SkillImage { get { return skillImage; } }
    public GameObject SkillPrefab { get { return skillPrefab; } }
    public float SkillNum { get { return skillNum; } }
    public int SkillLevel { get { return skillLevel; } }
    public float NeedMana { get { return needMana; } }
    public float AttackDamage { get { return attackDamage; } }
    public SkillType Type { get { return skillType; } }
    public SkillDis Dis { get { return skillDis; } }
    public DamageType DamType { get { return damageType; } }
    public AnimationClip[] Clips { get { return clips; } }
    public float MaxDistance { get { return maxDistance; } }
}
