using UnityEngine;

public class DungeonDoorTrigger : MonoBehaviour
{
    [SerializeField] private string sentence = "던전에 입장하시겠습니까?";
    [SerializeField] private string sceneName = "DungeonScene";

    private UIManager uiManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && PlayerState.Instance.AttackMode == false)
            uiManager.DefaultCanvas.GetComponent<DefaultCanvas>().OpenActCheckUI(sentence, sceneName);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            uiManager.DefaultCanvas.GetComponent<DefaultCanvas>().CloseActCheckUI();
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
    }
}
