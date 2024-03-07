/// <summary>
/// 플레이어 애니메이션 파라미터 변수들
/// </summary>
public class PlayerAniVariable
{
    public const string run = "Run";
    public const string jump = "Jump";
    public const string isGround = "IsGrounded";
    public const string canMove = "CanMove";
    public const string isDeath = "IsDeath";
    public const string drawWeapon = "DrawWeapon";
    public const string sheathWeapon = "SheathWeapon";
    public const string useSkill = "UseSkill";
    public const string basicSkillNum = "BasicSkillNum_F";
    public const string comboSkillNum = "ComboSkillNum_I";
    public const string comboCount = "ComboCount_I";
    ///<summary> 0 : at once / 1 : combo / 2 : charge </summary>
    public const string skillType = "SkillType_I";
    ///<summary> 0 : 평화모드 / 1 : 공격모드 </summary>
    public const string attackMode = "AttackMode_F";
    /// <summary> 크로스헤어 타겟팅 모드 설정    /// </summary>
    public const string isTargeted = "IsTargeted";

    public const int BaseLayer = 0;
    public const int DrawWeaponLayer = 1;
    public const int SheathWeaponLayer = 2;
}
