using System.Collections.Generic;
using UnityEngine;

public class Krog_RoaringCall : EnemySkill
{
    [SerializeField] private List<SpawnEnemy> spawnEnemys = new List<SpawnEnemy>();
    protected override void Attack(Transform target)
    {
        base.Attack(target);
        for (int i = 0; i < spawnEnemys.Count; i++)
        {
            GameObject newEnemy = Instantiate(spawnEnemys[i].enemy);
            newEnemy.transform.position = spawnEnemys[i].sqawnPos;
        }
    }
}

[System.Serializable]
public class SpawnEnemy
{
    public GameObject enemy;
    public Vector3 sqawnPos;
}
