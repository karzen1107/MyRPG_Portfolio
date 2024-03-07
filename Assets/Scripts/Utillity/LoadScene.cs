using UnityEngine;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 startRot;

    //BGM
    [SerializeField] private string bgmName;

    // 컴포넌트 //
    protected PlayerState playerState;
    protected SystemManager systemManager;

    public void SetPosToBeforeScene()   //이전 씬에 따른 다음 씬 전환 시 플레이어 위치 세팅
    {
        if (systemManager == null)
            systemManager = SystemManager.Instance;
        if (playerState == null)
            playerState = PlayerState.Instance;

        if (systemManager.BeforeSceneNum < 0 || systemManager.BeforSceneName == null || systemManager.BeforSceneName == string.Empty)
            return;

        playerState.gameObject.GetComponent<CharacterController>().enabled = false;
        playerState.gameObject.transform.position = startPos;
        playerState.gameObject.transform.rotation = Quaternion.Euler(startRot);
        playerState.gameObject.GetComponent<CharacterController>().enabled = true;
    }

    protected virtual void Start()
    {
        SetPosToBeforeScene();
        UIManager.Instance.FadeCanvas.GetComponent<FadeInOut>().InFade();
        AudioManager.Instance.BgmPlay(bgmName);
    }
}
