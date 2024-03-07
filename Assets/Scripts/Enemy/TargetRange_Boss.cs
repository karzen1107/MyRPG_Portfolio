using UnityEngine;
public class TargetRange_Boss : TargetRange
{
    private UIManager uiManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (uiManager == null)
                uiManager = UIManager.Instance;

            uiManager.ReceiveEnemyInfo(this.gameObject.GetComponentInParent<EnemyState>());
        }
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
    }
}
