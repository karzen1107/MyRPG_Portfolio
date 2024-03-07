using TMPro;
using UnityEngine;

public class CrossHairUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetDis;
    private Animator ani;

    public void AimmingTarget(GameObject _hit)
    {
        //Debug.Log(_hit.name);
        float distance = Vector3.Distance(PlayerState.Instance.gameObject.transform.position, _hit.transform.position);
        targetDis.text = $"{Mathf.Round(distance)}m";
        ani.SetBool(PlayerAniVariable.isTargeted, true);
    }

    public void NoAimmingTarget()
    {
        targetDis.text = $"";
        ani.SetBool(PlayerAniVariable.isTargeted, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        ani = this.GetComponent<Animator>();
        targetDis.text = "";
        ani.SetBool(PlayerAniVariable.isTargeted, false);
    }
}
