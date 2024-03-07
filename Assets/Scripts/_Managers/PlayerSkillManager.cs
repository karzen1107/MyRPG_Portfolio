using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : Singletone<PlayerSkillManager>
{
    [SerializeField] private List<BaseSkill> baseSkills = new List<BaseSkill>();

    public List<BaseSkill> BaseSkills { get { return baseSkills; } }
}
