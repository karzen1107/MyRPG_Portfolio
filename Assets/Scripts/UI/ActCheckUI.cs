using TMPro;
using UnityEngine;

public class ActCheckUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sentenceText;
    [SerializeField] private FadeInOut fade;

    [SerializeField] private GameObject yesButton;
    [SerializeField] private GameObject noButton;
    [SerializeField] private TMP_InputField itemCountInputField;
    [SerializeField] private GameObject UpCountButton;
    [SerializeField] private GameObject DownCountButton;
    [SerializeField] private GameObject okButton;

    private UIManager uiManager;

    private int sceneNum;
    private string sceneName;
    private Item item;
    private int inventoryIndex;
    private ActType actType;

    public void CloseClick()    //닫기 버튼 클릭 메서드
    {
        this.gameObject.SetActive(false);
    }

    // =========== 씬 전환 =============//
    public void YesClick()  //수락 버튼 클릭 메서드
    {
        switch (actType)
        {
            case ActType.LoadScene:
                if (sceneNum > 0)
                {
                    fade.OutFade(sceneNum);
                    SystemManager.Instance.BeforeSceneNum = sceneNum;
                }

                else if (sceneName != string.Empty)
                {
                    fade.OutFade(sceneName);
                    SystemManager.Instance.BeforSceneName = sceneName;
                }

                AudioManager.Instance.SoundFadeOut((AudioManager.Instance.bgmSound));
                sceneName = string.Empty;
                sceneNum = -1;
                actType = ActType.Max;
                break;
            case ActType.ExitGame:
                Application.Quit();
                break;
            default:
                Debug.LogError("등록되지 않은 타입입니다.");
                actType = ActType.Max;
                break;
        }
        this.gameObject.SetActive(false);
    }

    public void NoClick()   //거절 버튼 클릭 메서드
    {
        this.gameObject.SetActive(false);
    }

    public void SettingUI(string text, int _sceneNum)    //UI 세팅(씬 전환시 사용)
    {
        sentenceText.text = text;
        sceneNum = _sceneNum;
        yesButton.SetActive(true);
        noButton.SetActive(true);
        itemCountInputField.gameObject.SetActive(false);
        UpCountButton.SetActive(false);
        DownCountButton.SetActive(false);
        okButton.SetActive(false);
        actType = ActType.LoadScene;
    }

    public void SettingUI(string text, string _sceneName)    //UI 세팅(씬 전환시 사용)
    {
        sentenceText.text = text;
        sceneName = _sceneName;
        yesButton.SetActive(true);
        noButton.SetActive(true);
        itemCountInputField.gameObject.SetActive(false);
        UpCountButton.SetActive(false);
        DownCountButton.SetActive(false);
        okButton.SetActive(false);
        actType = ActType.LoadScene;
    }

    // =========== 상점 관련 =============//
    public void UpCountClick()  //아이템 수량 업 클릭 메서드
    {
        int temp = int.Parse(itemCountInputField.text);
        if (temp == item.ItemCount)
            return;
        temp += 1;
        itemCountInputField.text = temp.ToString();
    }

    public void DownCountClick()    //아이템 수량 다운 클릭 메서드
    {
        int temp = int.Parse(itemCountInputField.text);
        if (temp == 1)
            return;
        temp -= 1;
        itemCountInputField.text = temp.ToString();
    }

    public void OKClick()   //아이템 수략 확정 클릭 메서드
    {
        int temp = int.Parse(itemCountInputField.text);
        int totalPrice = item.CellPrice * temp;

        if (temp == item.ItemCount)
            InventoryManager.Instance.RemoveOldItem(inventoryIndex);
        else
            InventoryManager.Instance.RemoveItemCount(inventoryIndex, temp);

        if (uiManager == null)
            uiManager = UIManager.Instance;

        ShopUI shop = uiManager.DefaultCanvas.GetComponent<DefaultCanvas>().shopUI.GetComponent<ShopUI>();
        //판매한 아이템 새로 생성 및 초기화
        Item selledItem = new Item();
        selledItem.SetItem(item, temp);
        shop.AddReCellItem(selledItem, temp);   //상점에 판매한 아이템 세팅
        InventoryManager.Instance.GetGold(totalPrice);
        this.gameObject.SetActive(false);
    }

    public void OnValueChanged(string text) //인풋 필드의 내용 변화 시 호출
    {
        if (itemCountInputField.text == string.Empty)
            return;

        int temp = int.Parse(itemCountInputField.text);
        if (temp > item.ItemCount)
            itemCountInputField.text = item.ItemCount.ToString();
        if (temp < 1)
            itemCountInputField.text = "1";
    }

    public void SettingUI(string text, Item item, int index)   // 다수의 아이템 구매/판매시 사용
    {
        sentenceText.text = text;
        yesButton.SetActive(false);
        noButton.SetActive(false);
        itemCountInputField.gameObject.SetActive(true);
        UpCountButton.SetActive(true);
        DownCountButton.SetActive(true);
        okButton.SetActive(true);

        this.item = item;
        inventoryIndex = index;
        itemCountInputField.text = "1";
    }

    // =========== 게임 종료 관련 =============//
    public void SettingUI(string text)    //UI 세팅(게임 종료 확인 시 사용)
    {
        sentenceText.text = text;
        yesButton.SetActive(true);
        noButton.SetActive(true);
        itemCountInputField.gameObject.SetActive(false);
        UpCountButton.SetActive(false);
        DownCountButton.SetActive(false);
        okButton.SetActive(false);
        actType = ActType.ExitGame;
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
    }
}

public enum ActType
{
    LoadScene, ExitGame, Max
}