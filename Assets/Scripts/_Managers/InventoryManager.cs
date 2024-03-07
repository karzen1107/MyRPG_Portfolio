using UnityEngine;

public class InventoryManager : Singletone<InventoryManager>
{
    // 슬롯 관리 //
    [SerializeField] private RectTransform slotParent;  //슬롯 생성할 부모 오브젝트
    [SerializeField] private GameObject slotPrefab; //생성할 슬롯 프리팹
    public int maxSlotCount = 50;    //최대 생성가능한 슬롯 수량
    private GameObject[] slots; //생성한 슬롯들
    private InventorySlot[] invenSlots; //생성한 아이템 슬롯들 컴포넌트
    private int itemMaxCount = 5;   //슬롯당 최대 누적 수량

    private SystemManager systemManager;
    private UIManager uiManager;

    // 속성 //
    public int Gold { get; private set; }
    public int UseableSlotCount { get; private set; }
    public GameObject[] Slots { get { return slots; } }
    public InventorySlot[] InvenSlots { get { return invenSlots; } }

    public bool GetItem(Item_SO _item)  //획득한 아이템의 정보 가져오고 저장하는 메서드
    {
        if (_item.Type == ItemType.Equipment)    //아이템 타입이 장비라면 슬롯에 등록
        {
            for (int i = 0; i < UseableSlotCount; i++)
            {
                if (invenSlots[i].Item == null)
                {
                    SettingNewItem(_item, i);
                    return true;
                }
            }
        }
        else    //아이템 타입이 장비가 아니라면 같은 제품이 있는지 보고 수량확인
        {
            for (int i = 0; i < UseableSlotCount; i++) //슬롯들에 같은 제품이 있는지 확인
            {
                if (invenSlots[i].Item != null && invenSlots[i].Item.ItemName == _item.ItemName)    //같은 아이템이라면
                {
                    if (invenSlots[i].Item.ItemCount < itemMaxCount)    //수량이 Max가 아니라면 수량 추가
                    {
                        invenSlots[i].AddItemCount();
                        return true;
                    }
                }
            }
            //위에서 같은 제품을 하나도 찾지 못했다면 가장 가까운 슬롯에 아이템 등록
            for (int i = 0; i < UseableSlotCount; i++)
            {
                if (invenSlots[i].Item == null)
                {
                    SettingNewItem(_item, i);
                    return true;
                }
            }
        }
        if (uiManager == null)
            uiManager = UIManager.Instance;

        uiManager.ShowCaution(CautionType.FullInventory);
        return false;
    }

    public bool GetItem(Item _item)  //획득한 아이템의 정보 가져오고 저장하는 메서드
    {
        if (_item.Type == ItemType.Equipment)    //아이템 타입이 장비라면 슬롯에 등록
        {
            for (int i = 0; i < UseableSlotCount; i++)
            {
                if (invenSlots[i].Item == null)
                {
                    SettingNewItem(_item, i);
                    return true;
                }
            }
        }
        else    //아이템 타입이 장비가 아니라면 같은 제품이 있는지 보고 수량확인
        {
            for (int i = 0; i < UseableSlotCount; i++) //슬롯들에 같은 제품이 있는지 확인
            {
                if (invenSlots[i].Item != null && invenSlots[i].Item.ItemName == _item.ItemName)    //같은 아이템이라면
                {
                    if (invenSlots[i].Item.ItemCount < itemMaxCount)    //수량이 Max가 아니라면 수량 추가
                    {
                        invenSlots[i].AddItemCount();
                        return true;
                    }
                }
            }
            //위에서 같은 제품을 하나도 찾지 못했다면 가장 가까운 슬롯에 아이템 등록
            for (int i = 0; i < UseableSlotCount; i++)
            {
                if (invenSlots[i].Item == null)
                {
                    SettingNewItem(_item, i);
                    return true;
                }
            }
        }
        if (uiManager == null)
            uiManager = UIManager.Instance;

        uiManager.ShowCaution(CautionType.FullInventory);
        return false;
    }

    public void SettingNewItem(Item_SO _item, int index, int itemCount = 1)   //새로운 아이템 등록 메서드
    {
        Item newItem = new Item();
        newItem.SetItem(_item, itemCount);
        invenSlots[index].SlotSetUI(newItem);
    }

    public void SettingNewItem(Item _item, int index, int itemCount = 1)   //새로운 아이템 등록 메서드 : 오버로딩
    {
        Item newItem = new Item();
        newItem.SetItem(_item, itemCount);
        invenSlots[index].SlotSetUI(newItem);
    }

    public void RemoveOldItem(int index) //기존 아이템 제거 메서드
    {
        invenSlots[index].SlotSetNull();
    }

    public void RemoveItemCount(int index, int amount)
    {
        invenSlots[index].RemoveItemCount(amount);
    }

    public void GetGold(int amount)  //획득한 골드 저장하는 메서드
    {
        Gold += amount;
    }

    public bool UseGold(int amount) //골드 사용 메서드
    {
        if (Gold < amount)
        {
            if (uiManager == null)
                uiManager = UIManager.Instance;

            uiManager.ShowCaution(CautionType.NoMoney);
            return false;
        }
        Gold -= amount;
        return true;
    }

    private void LoadInventoryData(PlayerData data) //인벤토리 데이터 로드
    {
        if (data == null)
            return;

        UseableSlotCount = data.inventoryData.useableSlotCount;
        slots = new GameObject[UseableSlotCount];
        invenSlots = new InventorySlot[UseableSlotCount];

        for (int i = 0; i < slots.Length; i++)  //빈 슬롯 생성, 위치 설정, 인벤토리 데이터 로드
        {
            slots[i] = Instantiate(slotPrefab, slotParent.localPosition, Quaternion.identity);  //빈 슬롯 생성

            // 슬롯 위치 설정
            RectTransform rectSlot = slots[i].GetComponent<RectTransform>();
            rectSlot.SetParent(slotParent);
            rectSlot.localScale = Vector3.one;
            invenSlots[i] = slots[i].GetComponent<InventorySlot>(); //인벤토리 슬롯 컴포넌트 할당

            //인벤토리 데이터 로드
            Gold = data.inventoryData.gold;
            if (data.inventoryData.slots[i] == null || data.inventoryData.slots[i].itemName == null)
                continue;
            Item_SO tempItem = Resources.Load($"Items/{data.inventoryData.slots[i].itemName}") as Item_SO;
            SettingNewItem(tempItem, i, data.inventoryData.slots[i].itemCount);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        systemManager = SystemManager.Instance;
        uiManager = UIManager.Instance;
        systemManager.ev_LoadData += LoadInventoryData;
    }
}