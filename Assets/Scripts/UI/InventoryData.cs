[System.Serializable]
public class InventoryData
{
    public int useableSlotCount; // 사용가능한 슬롯 수량
    public InvenSlotData[] slots;   // 슬롯 데이터
    public int gold;    // 골드
}

[System.Serializable]
public class InvenSlotData
{
    public int slotNumber;  //슬롯 번호
    public string itemName; //아이템 이름
    public int itemCount;   //아이템 수량

    public void SaveSlotData(int num)
    {
        slotNumber = num;

        InventoryManager inventoryManager = InventoryManager.Instance;
        if(inventoryManager.InvenSlots != null && inventoryManager.InvenSlots[num].Item != null)
        {
            itemName = inventoryManager.InvenSlots[num].Item.ItemPrefab.gameObject.name;
            itemCount = inventoryManager.InvenSlots[num].Item.ItemCount;
        }
    }
}
