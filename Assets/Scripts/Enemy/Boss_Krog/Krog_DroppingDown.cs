using UnityEngine;

public class Krog_DroppingDown : EnemySkill
{
    [SerializeField] private Transform bodyMesh;

    protected override void Attack(Transform target) //타겟을 바라보면서 공격하는 메서드
    {
        myPos.LookAt(target);
        bodyMesh.localRotation = Quaternion.Euler(bodyMesh.position.x, bodyMesh.position.y - 27f, bodyMesh.position.z);
        ani.SetInteger(aniParameter_SkillType, ani_skillNum);
    }
}
