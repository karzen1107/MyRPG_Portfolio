using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Boss : EnemyState
{
    [SerializeField] private string AttackBgm;
    [SerializeField] private string DefaultBgm;

    [Header("페이즈 스킬 허용")]
    [SerializeField] private List<Phase> phases = new List<Phase>();

    protected override void SetState() //상태 세팅하는 메서드
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
                AudioManager.Instance.BgmPlay(AttackBgm);
                break;
            case State.Return:
                targetRange.Target = null;
                ReturnMyPos();
                AudioManager.Instance.BgmPlay(DefaultBgm);
                break;
            case State.Attack:
                Attack();
                break;
            case State.Die:
                Die();
                break;
        }
    }

    protected override void Die()
    {
        base.Die();
        AudioManager.Instance.BgmPlay(DefaultBgm);
    }

    protected override void SetSkillWeight() //스킬 가중치 설정
    {
        int temp = 0;
        sumSkillWeight = 0;

        for (int i = 0; i < skills.Length; i++)
        {
            bool canUse = true;

            for (int j = 0; j < phases.Count; j++)
            {
                if (skills[i] == phases[j].skill)
                    canUse = false;
            }

            if (canUse)
            {
                sumSkillWeight += skillSelectWeight[i];
                temp += skillSelectWeight[i];
                skillRandemWeight[i] = temp;
            }
        }
    }

    public override void TakeDamage(float damage)    //데미지를 입는 메서드
    {
        base.TakeDamage(damage);
        CheckPhase();
    }

    private void CheckPhase()
    {
        if (phases.Count <= 0)
            return;

        float lifePercent = life / startLife * 100;

        for (int i = 0; i < phases.Count; i++)
        {
            if (lifePercent <= phases[i].pahsePercent)
            {
                selectedSkill = phases[i].skill;
                phases.RemoveAt(i);
            }

        }

        SetSkillWeight();
    }
}

[System.Serializable]
public class BossPhase
{
    public int pahsePercent;
    public EnemySkill skill;
}
