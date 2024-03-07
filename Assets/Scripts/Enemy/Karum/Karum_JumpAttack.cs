using UnityEngine;

public class Karum_JumpAttack : EnemySkill
{
    [Header("This Skill Porperty")]
    [SerializeField] private float moveSpeed;   //이동 속도

    private Vector3 targetPos;  //목표 공격 위치
    private Vector3 targetDir;  //목표 공격 방향
    protected override void Attack(Transform target)
    {
        base.Attack(target);
        targetPos = target.position;
        targetDir = (targetPos - myPos.position).normalized;
    }

    private void Update()
    {
        if (ani.GetInteger("EnemyState") == 4)
            return;
        float targetToDis = Vector3.Distance(targetPos, myPos.position);
        if (ani.GetInteger(aniParameter_SkillType) == 1 && IsCanMove && targetToDis > 1f)
            myPos.transform.Translate(targetDir * moveSpeed * Time.deltaTime, Space.World);
    }
}
