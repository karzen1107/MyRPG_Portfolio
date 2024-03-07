using UnityEngine;

public class SkillEffectAttack : MonoBehaviour
{
    [SerializeField] private Skill_SO skill;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            PlayerState.Instance.GiveDamage(other.gameObject.GetComponent<EnemyState>(), skill.AttackDamage);
            PlayerState.Instance.GetComponent<PlayerState>().SetAttackMode(false);
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            Debug.Log($"{other.gameObject.layer} 나감");
    }
}
