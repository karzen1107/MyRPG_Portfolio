using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 인벤토리UI에 관한 클래스
/// </summary>

public class InventoryUI : MonoBehaviour
{
    // 골드 //
    [SerializeField] private TextMeshProUGUI goldText;  //보유한 골드 텍스트

    private int beginSlotIndex; //마우스 다운 한 슬롯의 인덱스 정보
    private Item endSlotItem;
    private int endSlotIndex;   //마우스 업 한 슬롯의 인덱스 정보

    // 컴포넌트 //
    private InventoryManager inventoryManager;
    private PlayerState playerState;
    private UIManager uiManager;
    [SerializeField] private PlayerStatusUI playerStatusUI; //캐릭터 정보 창

    //테스트용//
    public int BeginSlotIndex { get { return beginSlotIndex; } }
    public int EndSlotIndex { get { return endSlotIndex; } }

    #region 버튼 클릭 메서드
    public void CloseClick()
    {
        if (CameraControls.Instance.IsNpcInteracting == true)
        {
            uiManager.SetDefaultState();
            uiManager.DefaultCanvas.GetComponent<DefaultCanvas>().shopUI.SetActive(false);
        }
        this.gameObject.SetActive(false);
    }

    public void SortClick()
    {
        Debug.Log("자동 정렬");
    }
    #endregion

    private void ResetSlotIndex()   //슬롯 인덱스 초기화 메서드
    {
        beginSlotIndex = -1;
        endSlotIndex = -1;
    }

    private void OnPointerEnter()   //마우스가 슬롯 위로 올라갔을때 정보 보이기
    {
        foreach (RaycastResult result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
        {
            if (result.gameObject.GetComponent<InventorySlot>() == null)
                continue;

            GameObject selected = result.gameObject;
            for (int i = 0; i < inventoryManager.Slots.Length; i++)
            {
                if (inventoryManager.Slots[i] == selected && inventoryManager.InvenSlots[i] != null)
                {
                    uiManager.IconOver(inventoryManager.InvenSlots[i].Item);
                    break;
                }
            }
        }
    }

    private void OnPointerExit()   //마우스가 슬롯 밖으로 나갔을때 정보 지우기
    {
        bool isOut = true;
        foreach (RaycastResult result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
        {
            InventorySlot temp = result.gameObject.GetComponent<InventorySlot>();
            if (temp != null && temp.Item != null)
            {
                isOut = false;
                break;
            }
            isOut = true;
        }

        if (isOut)
            uiManager.IconExit();
    }

    private void OnPointerDown()    //마우스 클릭다운시 메서드
    {
        if (Input.GetMouseButtonDown(0))
        {
            uiManager.IconExit();

            foreach (var result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
            {
                InventorySlot temp = result.gameObject.GetComponent<InventorySlot>();
                if (temp != null && temp.Item != null)
                {
                    for (int i = 0; i < inventoryManager.Slots.Length; i++)
                    {
                        if (temp.gameObject == inventoryManager.Slots[i])
                        {
                            beginSlotIndex = i;
                            uiManager.IconSelect(temp.ItemImage, i);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void OnPointerUp(Item item)  //마우스 클릭업시 메서드. 퀵슬롯 삭제, 퀵슬롯 스위치의 역할
    {
        if (Input.GetMouseButtonUp(0))
        {
            //if (results.Count <= 0) //레이캐스트한것이 없다면 = 해당 아이템 필드에 버리기
            //{
            //    Vector3 pos = PlayerState.SendMyPos().position + new Vector3(0.5f, 0, 0);   //아이템 생성 위치
            //    Instantiate(beginSlotItem.ItemPrefab, pos, Quaternion.identity);    //필드에 아이템 생성
            //    Destroy(beginSlotItemImage);    //드래그중이던 아이콘 삭제
            //    SetInventoryItem();
            //    results.Clear();
            //    return;
            //}
            foreach (var result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
            {
                InventorySlot temp = result.gameObject.GetComponent<InventorySlot>();
                if (temp != null)
                {
                    for (int i = 0; i < inventoryManager.Slots.Length; i++)
                    {
                        if (inventoryManager.Slots[i] == temp.gameObject)
                        {
                            endSlotItem = inventoryManager.InvenSlots[i].Item;
                            endSlotIndex = i;
                            break;
                        }
                    }
                    SwitchItem(item, endSlotItem); //아이템 위치 교체
                }
            }
        }
    }

    private void SwitchItem(Item start, Item end)   //아이템 위치 교환 메서드
    {
        if (start == null)
            return;
        //경우의 수 : end == 빈칸 / end != 빈칸
        if (end == null)
        {
            inventoryManager.SettingNewItem(start, endSlotIndex, start.ItemCount);

            if (beginSlotIndex > -1)    //같은 인벤토리 슬롯끼리 위치 변환하는 경우
                inventoryManager.RemoveOldItem(beginSlotIndex);
            else //외부에서 온것일 경우(장비창)
            {
                playerState.RemovePlayerStatus(start);
                for (int i = 0; i < playerStatusUI.Slots.Count; i++)
                {
                    if (playerStatusUI.Slots[i].EquipType == start.EquipType)
                        playerStatusUI.Slots[i].SlotSetNull();
                }
            }
        }
        else
        {
            if (beginSlotIndex > -1) //같은 인벤토리 슬롯끼리 위치 변환 하는 경우
            {
                inventoryManager.SettingNewItem(end, beginSlotIndex, end.ItemCount);
                inventoryManager.SettingNewItem(start, endSlotIndex, start.ItemCount);
            }
            else  //외부에서 온것일 경우(장비창)
            {
                playerState.RemovePlayerStatus(start);
                inventoryManager.RemoveOldItem(endSlotIndex);
                inventoryManager.SettingNewItem(start, endSlotIndex);
                playerState.AddPlayerStatus(end);
                playerStatusUI.SetUserItem(end);
            }
        }
        ResetSlotIndex();
    }

    private void InteractItem() //마우스 오른쪽 클릭시 장착/상점은 판매 (Interact : 상호작용하다)
    {
        if (Input.GetMouseButtonDown(1))
        {
            foreach (var result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
            {
                if (result.gameObject.GetComponent<InventorySlot>() == null)
                    continue;

                GameObject selected = result.gameObject;
                for (int i = 0; i < inventoryManager.Slots.Length; i++)
                {
                    if (inventoryManager.Slots[i] == selected && inventoryManager.InvenSlots[i].Item != null)
                    {
                        Item item = inventoryManager.InvenSlots[i].Item;
                        if (uiManager.DefaultCanvas.GetComponent<DefaultCanvas>().shopUI.activeSelf == true) //상점 이용중이라면 판매
                        {
                            if (item.ItemCount > 1)
                            {
                                uiManager.DefaultCanvas.GetComponent<DefaultCanvas>().OpenActCheckUI("수량을 입력하세요", item, i);
                            }
                            else
                            {
                                inventoryManager.RemoveOldItem(i);
                                inventoryManager.GetGold(item.CellPrice);
                                ShopUI shop = uiManager.DefaultCanvas.GetComponent<DefaultCanvas>().shopUI.GetComponent<ShopUI>();
                                shop.AddReCellItem(item, 1);
                            }
                        }
                        else //상점 이용중이 아니라면 아이템 장착/사용
                        {
                            switch (item.Type)
                            {
                                case ItemType.Equipment:
                                    if (playerState.UsingItems[(int)item.EquipType] != null)    //이미 장착중인 아이템이 있다면
                                    {
                                        for (int j = 0; j < playerState.UsingItems.Length; j++)
                                        {
                                            if (playerState.UsingItems[j] != null && item.EquipType == playerState.UsingItems[j].EquipType)
                                            {
                                                Item temp = playerState.UsingItems[j];
                                                playerState.RemovePlayerStatus(playerState.UsingItems[j]);
                                                inventoryManager.RemoveOldItem(i);
                                                inventoryManager.SettingNewItem(temp, i);
                                                playerState.AddPlayerStatus(item);
                                                playerStatusUI.SetUserItem(item);
                                            }
                                        }
                                    }
                                    else  //그게 아니라면
                                    {
                                        playerState.AddPlayerStatus(item);
                                        inventoryManager.InvenSlots[i].SlotSetNull();
                                        playerStatusUI.SetUserItem(item);
                                        inventoryManager.RemoveOldItem(i);
                                    }
                                    break;
                                case ItemType.Used:
                                    bool isUsed = false;
                                    switch (item.EffectType)
                                    {
                                        case EffectType.Hp:
                                            if (playerState.Life == playerState.StartLife)
                                                Debug.Log("생명령이 가득 찼습니다");
                                            else
                                            {
                                                playerState.RecoverHp(item.ItemAbillity);
                                                isUsed = true;
                                            }
                                            break;
                                        case EffectType.Mp:
                                            if (playerState.Mana == playerState.StartMana)
                                                Debug.Log("마나가 가득 찼습니다");
                                            else
                                            {
                                                playerState.RecoverMp(item.ItemAbillity);
                                                isUsed = true;
                                            }
                                            break;
                                        case EffectType.Atk:
                                            playerState.NowAtk += item.ItemAbillity;
                                            isUsed = true;
                                            break;
                                        case EffectType.Def:
                                            playerState.NowShield += item.ItemAbillity;
                                            isUsed = true;
                                            break;
                                        default:
                                            break;
                                    }

                                    if (isUsed)
                                    {
                                        if (item.ItemCount > 1)
                                            inventoryManager.SettingNewItem(item, i);
                                        else
                                            inventoryManager.RemoveOldItem(i);
                                    }
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    private void Awake()
    {
        inventoryManager = InventoryManager.Instance;
        playerState = PlayerState.Instance;
        uiManager = UIManager.Instance;
    }

    private void Start()
    {
        uiManager.ev_OnInventoryUIDrop += OnPointerUp;
        playerStatusUI.ev_OnResetIndex += ResetSlotIndex;
    }

    private void Update()
    {
        goldText.text = inventoryManager.Gold.ToString();

        if (uiManager.ValidNum == uiNumbers.InventoryUI)
        {
            OnPointerEnter();
            OnPointerDown();
            InteractItem();
        }

        OnPointerExit();
    }
}
