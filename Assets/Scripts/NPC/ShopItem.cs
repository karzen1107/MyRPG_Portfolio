using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;   //아이템 이미지
    [SerializeField] private TextMeshProUGUI itemName;  //아이템 이름
    private Item item = new Item();   //아이템

    private InventoryManager inventoryManager;

    public Item Item { get { return item; } }

    public void SettingItem(Item_SO _item)  //아이템 세팅
    {
        item.SetItem(_item, 1);
        itemImage.sprite = item.ItemImage;
        itemName.text = item.ItemName;
    }

    public void BuyClick()
    {
        if (inventoryManager.UseGold(item.BuyPrice))
            inventoryManager.GetItem(item);
    }

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
    }
}
