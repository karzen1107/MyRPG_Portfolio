using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [Header("체력")]
    [SerializeField] private Image Hplmage; // 체력바 이미지
    [SerializeField] private TextMeshProUGUI HpText;    // 체력바 텍스트

    [Header("마나")]
    [SerializeField] private Image MpImage; // 마나바 이미지
    [SerializeField] private TextMeshProUGUI MpText; // 마나바 텍스트

    [Header("레벨&이름")]
    [SerializeField] private TextMeshProUGUI levelText; //레벨 텍스트
    [SerializeField] private TextMeshProUGUI userNameText;  //캐릭터 이름 텍스트

    // 컴포넌트 //
    SystemManager systemManager;
    PlayerState playerState;

    private void ShowHp() //체력바 UI 보이는 메서드
    {
        if (playerState == null)
            playerState = PlayerState.Instance;

        if (playerState.Life <= 0)
        {
            Hplmage.fillAmount = 0;
            HpText.text = (playerState.Life + "/" + playerState.StartLife).ToString();
        }
        else
        {
            Hplmage.fillAmount = playerState.Life / playerState.StartLife;
            HpText.text = (playerState.Life + "/" + playerState.StartLife).ToString();
        }

    }

    private void ShowMp() //마나바 UI 보이는 메서드
    {
        if (playerState == null)
            playerState = PlayerState.Instance;

        if (playerState.Mana <= 0)
        {
            MpImage.fillAmount = 0;
            MpText.text = (playerState.Mana + "/" + playerState.StartMana).ToString();
        }
        else
        {
            MpImage.fillAmount = playerState.Mana / playerState.StartMana;
            MpText.text = (playerState.Mana + "/" + playerState.StartMana).ToString();
        }
    }

    private void SetUser()
    {
        if (playerState == null)
            playerState = PlayerState.Instance;

        levelText.text = playerState.CharLevel.ToString();
        userNameText.text = playerState.CharName;
    }

    private void Awake()
    {
        playerState = PlayerState.Instance;
        systemManager = SystemManager.Instance;
    }

    private void Start()
    {
        SetUser();
    }

    // Update is called once per frame
    void Update()
    {
        ShowHp();
        ShowMp();
    }
}
