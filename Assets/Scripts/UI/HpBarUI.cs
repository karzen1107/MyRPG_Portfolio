using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Slider slider;
    // 컴포넌트 //
    private EnemyState enemyState;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        slider = GetComponentInChildren<Slider>();
        enemyState = GetComponentInParent<EnemyState>();
    }

    private void Update()
    {
        this.transform.LookAt(cam.transform.position);
        slider.value = enemyState.Life / enemyState.StartLife;

    }
}
