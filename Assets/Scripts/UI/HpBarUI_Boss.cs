using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUI_Boss : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyName; //보스 이름
    [SerializeField] private Image hpBar;   //보스 체력
    [SerializeField] private TextMeshProUGUI hpBarText;   //보스 체력 텍스트

    // 컴포넌트 //
    private EnemyState enemy;

    public void SettingUI(EnemyState _enemy)
    {
        this.gameObject.SetActive(true);
        enemy = _enemy;
        enemyName.text = enemy.EnemyName;
        hpBar.fillAmount = enemy.Life / enemy.StartLife;
        hpBarText.text = $"{enemy.Life} / {enemy.StartLife}";
    }

    private void LateUpdate()
    {
        if (enemy == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        hpBar.fillAmount = enemy.Life / enemy.StartLife;
        hpBarText.text = $"{enemy.Life} / {enemy.StartLife}";
    }
}
