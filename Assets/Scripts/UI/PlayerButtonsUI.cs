using TMPro;
using UnityEngine;

public class PlayerButtonsUI : MonoBehaviour
{
    [SerializeField] private GameObject[] playerButtons;    //버튼들 자식 오브젝트
    private GameObject selectButton;    //선택한 버튼
    [SerializeField] private GameObject[] buttonOpenUI; //버튼 클릭시 활성화/비활성화 할 UI들

    [Space(5), Header("buttonName 관련")]
    [SerializeField] private GameObject buttonNameUI; //버튼 이름 UI
    private TextMeshProUGUI buttonNameUIText;   //버튼 이름 UI 텍스트

    // 컴포넌트 //
    UIManager uiManager;

    public void PlayerButtonClick() //버튼 클릭 메서드
    {
        if (selectButton != null)
        {
            for (int i = 0; i < playerButtons.Length; i++)
            {
                if (selectButton == playerButtons[i])
                {
                    if (buttonOpenUI[i] != null)
                        buttonOpenUI[i].SetActive(!buttonOpenUI[i].activeSelf);
                }
            }
        }

    }

    private void PlayerButtonEnter()
    {
        foreach (var result in uiManager.Results)
        {
            for (int i = 0; i < playerButtons.Length; i++)
            {
                if (result.gameObject == playerButtons[i])
                {
                    selectButton = result.gameObject;
                    break;
                }
            }
        }

        if (selectButton != null)
            SetButtonName(selectButton.name);
    }

    private void PlayerButtonExit()
    {
        selectButton = null;
        buttonNameUI.SetActive(false);
    }

    private void SetButtonName(string buttonName)   //버튼 이름 세팅 메서드
    {
        switch (buttonName)   //버튼에 따라 표시할 내용
        {
            case "PlayerStatus":
                buttonNameUIText.text = "캐릭터 정보";
                break;
            case "Inventory":
                buttonNameUIText.text = "인벤토리";
                break;
            case "SkillBook":
                buttonNameUIText.text = "스킬북";
                break;
            case "Quest":
                buttonNameUIText.text = "퀘스트";
                break;
            case "Option":
                buttonNameUIText.text = "옵션";
                break;
            default:
                break;
        }

        buttonNameUI.GetComponent<RectTransform>().position = new Vector2(selectButton.transform.position.x, buttonNameUI.transform.position.y);
        buttonNameUI.SetActive(true);
    }

    private void Awake()
    {
        buttonNameUIText = buttonNameUI.GetComponentInChildren<TextMeshProUGUI>();
        uiManager = UIManager.Instance;
    }

    private void Start()
    {
        if (uiManager == null)
            uiManager = UIManager.instance;
    }

    private void Update()
    {
        if (uiManager.ValidNum == uiNumbers.PlayerButtons)
            PlayerButtonEnter();
        else
            PlayerButtonExit();
    }
}
