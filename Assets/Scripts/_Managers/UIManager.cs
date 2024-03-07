using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : Singletone<UIManager>
{
    [Header("마우스 레이캐스트")]
    [SerializeField] private List<GameObject> UIs = new List<GameObject>();
    [SerializeField] private GameObject inventoryItemInfoUI;    //인벤토리 아이템 정보UI
    [SerializeField] private GameObject statusItemInfoUI;    //장비창 아이템 정보UI
    [SerializeField] private GameObject quickslotItemInfoUI;    //퀵슬로 아이템 정보UI
    [SerializeField] private GameObject skillbookSkillInfoUI;    //스킬북 스킬 정보UI

    private GameObject icon;
    private int index;

    [SerializeField] private Transform parentCanvas;   //부모 캔버스 객체 위치
    private GraphicRaycaster raycaster; //그래픽스 레이캐스터는 캔버스에 레이캐스트를 하는 데 사용합니다. 레이캐스터는 캔버스의 모든 그래픽스를 감시하여 그 중 하나에 충돌하였는지 여부를 결정합니다.
    private PointerEventData eventData; //터치 상태나 마우스 클릭중인 상황 (연속적으로 Input.GetKey() 상태를 유지하고 있을 때) 에서 특정 액션을 취할 수 있게 해준다.
    private List<RaycastResult> results;    //UnityEngine.EventSystems의 struct. Raycaster의 Hit 결과를 담고 있다.

    [Header("캔버스 관리")]
    [SerializeField] private GameObject defaultCanvas;  //기본 캔버스
    [SerializeField] private GameObject talkingCanvas;  //대화창 캔버스
    [SerializeField] private GameObject crossHairCanvas;  //대화창 캔버스
    [SerializeField] private GameObject fadeCanvas; //페이드 캔버스

    [Header("경고 문구"), SerializeField] private GameObject cautionText;

    // 이벤트 대리자 //
    public UnityAction<Item> ev_OnPlayerStatusUIDrop;   //PlayerStatusUI 아이콘 드롭 이벤트
    public UnityAction<QuickSlotType, int> ev_OnQuikSlotsUIDrop;    //QuikSlotsUI 아이콘 드롭 이벤트
    public UnityAction<Item> ev_OnInventoryUIDrop;  //InventoryUI 아이콘 드롭 이벤트
    public UnityAction ev_OnDeleteUI;   //슬롯에서 UI 삭제 이벤트
    public UnityAction ev_SetNpcTalkingAni; //Npc 대화 애니 파라미터 변경 이벤트

    // 속성 //
    public uiNumbers ValidNum { get; private set; } //valid : 유효한
    public List<RaycastResult> Results { get { return results; } }
    public int Index { get { return index; } }
    public GameObject DefaultCanvas { get { return defaultCanvas; } }
    public GameObject FadeCanvas { get { return fadeCanvas; } }


    private void SendResult()   //마우스 레이캐스트 결과 보내주는 메서드
    {
        results.Clear();
        raycaster.Raycast(eventData, results);
        OverrayUINumber r = null;
        if (results.Count <= 0 || results == null)
        {
            ValidNum = uiNumbers.Max;
        }
        else
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<OverrayUINumber>() == null)
                    continue;
                else
                {
                    r = result.gameObject.GetComponent<OverrayUINumber>();
                    break;
                }
            }
        }

        if (r != null)
        {
            for (int i = 0; i < UIs.Count; i++)
            {
                uiNumbers uinum = UIs[i].GetComponent<OverrayUINumber>().UINumber;
                if (uinum == r.UINumber)
                {
                    switch (r.UINumber)
                    {
                        case uiNumbers.PlayerButtons:
                            ValidNum = uiNumbers.PlayerButtons;
                            break;
                        case uiNumbers.QuikSlots:
                            ValidNum = uiNumbers.QuikSlots;
                            break;
                        case uiNumbers.PlayerStatusUI:
                            ValidNum = uiNumbers.PlayerStatusUI;
                            break;
                        case uiNumbers.InventoryUI:
                            ValidNum = uiNumbers.InventoryUI;
                            break;
                        case uiNumbers.SkillBookUI:
                            ValidNum = uiNumbers.SkillBookUI;
                            break;
                        case uiNumbers.ShopUI:
                            ValidNum = uiNumbers.ShopUI;
                            break;
                        case uiNumbers.Max:
                        case uiNumbers.ActCheckUI:
                            break;
                        default:
                            Debug.LogError("UIManager switch문에 등록이 필요합니다.");
                            break;
                    }
                }
            }
        }
    }

    public void IconOver(Item overed) //아이콘 마우스 오버(아이템)
    {
        if (overed != null)
        {
            inventoryItemInfoUI.GetComponent<ItemInfoUI>().SetItemInfo(overed);
            inventoryItemInfoUI.SetActive(true);
        }
    }

    public void IconOver(BaseSkill overed)  //아이콘 마우스 오버(스킬)
    {
        if (overed != null)
        {

        }
    }

    public void IconExit()  //아이콘 마우스 아웃
    {
        inventoryItemInfoUI.SetActive(false);
    }

    public void IconSelect(GameObject selected, int _index) //아이콘 마우스 다운(오버라이드)
    {
        if (selected != null)
        {
            icon = Instantiate(selected);
            index = _index;

            //현재 드래그중인 UI화면의 최상단에 출력되도록 하기
            icon.transform.SetParent(parentCanvas);  //부모 오브젝트를 canvas로 설정
            icon.transform.SetAsLastSibling(); //가장 앞에 보이도록 마지막 자식으로 설정
            icon.transform.position = eventData.position;  //UI위치 == 마우스 위치
            icon.GetComponent<RectTransform>().localScale = Vector3.one;

            icon.GetComponent<Image>().raycastTarget = false;  //광선충돌처리가 되지 않도록 하기
            #region 광선충돌처리(raycastTarget)가 되지 않도록 해야하는 이유
            //드래그하고 있는 오브젝트를 Canvas 바로 밑으로 올려 해당 오브젝트보다 하위에 존재하는 UI들이 반응하지 않는다.
            //UI관련된 그래픽 컴포넌트들은 GraphicRaycast라고 따로 받는 레이캐스트가 존재한다.
            //UI관련 오브젝트(예를 들면 Image)에 raycastTarget이라고 적힌 옵션이 있을 것이다.
            //이걸 에디터에서 바로 꺼주면 드래그가 안되니 코드 상에서 잠시 꺼주면 된다.
            #endregion

            ValidNum = uiNumbers.Max;
        }
    }

    private bool IconDrag() //아이콘 마우스 드래그
    {
        if (Input.GetMouseButton(0) && icon != null) //왼쪽 버튼으로만 가능하도록
        {
            icon.transform.position = eventData.position;  //UI위치 == 마우스 위치
            icon.GetComponent<RectTransform>().localScale = Vector3.one;
            return true;
        }
        return false;
    }

    public void IconDrop()  //아이콘 마우스 업
    {
        if (Input.GetMouseButtonUp(0) && icon != null)
        {
            if (ValidNum == uiNumbers.Max)
            {
                ev_OnDeleteUI?.Invoke();
            }
            else
            {
                List<uiNumbers> iconCanUseSlot = icon.GetComponent<UIDragAndDrop>().CanUseSlot;
                for (int i = 0; i < iconCanUseSlot.Count; i++)
                {
                    uiNumbers canUse = iconCanUseSlot[i];
                    if (ValidNum == canUse)
                    {
                        switch (canUse)
                        {
                            case uiNumbers.PlayerButtons:
                            case uiNumbers.SkillBookUI:
                            case uiNumbers.Max:
                                break;
                            case uiNumbers.PlayerStatusUI:
                                //ev_OnPlayerStatusUIDrop?.Invoke(icon.GetComponent<UIDragAndDrop>().Item);
                                break;
                            case uiNumbers.QuikSlots:
                                QuickSlotType temp = icon.GetComponent<UIDragAndDrop>().quickType;
                                ev_OnQuikSlotsUIDrop?.Invoke(temp, index);
                                break;
                            case uiNumbers.InventoryUI:
                                //ev_OnInventoryUIDrop?.Invoke(icon.GetComponent<UIDragAndDrop>().Item);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            Destroy(icon);
        }
    }

    public void SetUIOnLast(GameObject ui)  //특정 UI를 부모 캔버스 제일 마지막에 위치하도록 변경
    {
        ui.transform.SetAsLastSibling();
        SystemManager.Instance.OpendUIList.Add(ui.GetComponent<OverrayUINumber>());
    }

    public void SetNPCTalkingState(Transform targetNPC, Vector3 camPos, Vector3 camRot, NpcActionState actionState)    //NPC 대화상태로 전환
    {
        //카메라 세팅
        GameObject mainCam = Camera.main.gameObject;
        mainCam.transform.SetParent(targetNPC);
        mainCam.transform.localPosition = camPos;
        mainCam.transform.localRotation = Quaternion.Euler(camRot);

        CameraControls.Instance.IsNpcInteracting = true;
        FadeInOut fadeInOut = fadeCanvas.GetComponent<FadeInOut>();

        switch (actionState)
        {
            case NpcActionState.TalkQuest:
                defaultCanvas.GetComponent<DefaultCanvas>().playUI.SetActive(false);
                talkingCanvas.SetActive(true);
                crossHairCanvas.SetActive(false);
                break;
            case NpcActionState.OpenShop:
                defaultCanvas.GetComponent<DefaultCanvas>().playUI.SetActive(false);
                talkingCanvas.SetActive(false);
                crossHairCanvas.SetActive(false);
                break;
        }
        fadeInOut.InFade();
    }

    public void SetDefaultState()   //기본 상태로 전환
    {
        //카메라 세팅
        GameObject mainCam = Camera.main.gameObject;
        mainCam.transform.SetParent(null);

        CameraControls.Instance.IsNpcInteracting = false;
        defaultCanvas.GetComponent<DefaultCanvas>().playUI.SetActive(true);
        talkingCanvas.SetActive(false);
        crossHairCanvas.SetActive(true);
        ev_SetNpcTalkingAni?.Invoke();

        FadeInOut fadeInOut = fadeCanvas.GetComponent<FadeInOut>();
        fadeInOut.InFade();
    }

    public void ReceiveEnemyInfo(EnemyState enemy)  //보스 Enemy정보 보내주기
    {
        HpBarUI_Boss hpBar = defaultCanvas.GetComponent<DefaultCanvas>().enemyBossHpUI.GetComponent<HpBarUI_Boss>();

        if (hpBar != null)
            hpBar.SettingUI(enemy);
    }

    public void CloseUI(OverrayUINumber ui) //UI창 닫기
    {
        switch (ui.UINumber)
        {
            case uiNumbers.PlayerStatusUI:
                ui.gameObject.GetComponent<PlayerStatusUI>().CloseClick();
                break;
            case uiNumbers.InventoryUI:
                ui.gameObject.GetComponent<InventoryUI>().CloseClick();
                break;
            case uiNumbers.SkillBookUI:
                ui.gameObject.GetComponent<SkillBookUI>().CloseClick();
                break;
            case uiNumbers.ShopUI:
                ui.gameObject.GetComponent<ShopUI>().CloseClick();
                break;
            case uiNumbers.ActCheckUI:
                ui.GetComponent<ActCheckUI>().CloseClick();
                break;
            default:
                break;
        }
    }

    public void ShowCaution(CautionType cautionType)    //경고창 표시
    {
        TextMeshProUGUI cText = cautionText.GetComponent<TextMeshProUGUI>();
        Animation cAni = cautionText.GetComponent<Animation>();
        switch (cautionType)
        {
            case CautionType.NoMoney:
                cText.text = "골드가 부족합니다";
                cAni.Play();
                break;
            case CautionType.FullInventory:
                cText.text = "인벤토리가 가득 찼습니다";
                cAni.Play();
                break;
            case CautionType.SaveDone:
                cText.text = "저장 완료";
                cAni.Play();
                break;
            default:
                Debug.LogError("지정하지 않은 경고입니다.");
                return;
        }
        StartCoroutine(CautionOnOff());
    }

    private IEnumerator CautionOnOff()
    {
        cautionText.SetActive(true);
        yield return new WaitForSeconds(5f);
        cautionText.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
        raycaster = parentCanvas.GetComponentInParent<GraphicRaycaster>();    //레이캐스트 할 캔버스 설정
        eventData = new PointerEventData(null);
        ValidNum = uiNumbers.Max;
    }

    private void Update()
    {
        eventData.position = Input.mousePosition;
        results = new List<RaycastResult>();

        if (!IconDrag())
            SendResult();

        IconDrop();
    }
}

public enum CautionType
{
    NoMoney, FullInventory, SaveDone, Max
}