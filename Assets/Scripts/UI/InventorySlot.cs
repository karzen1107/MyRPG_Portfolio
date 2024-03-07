using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 인벤토리 슬롯 클래스. 해당 슬롯의 아이템 정보도 가지고 있음
/// </summary>
public class InventorySlot : MonoBehaviour
{
    private Item item;
    [SerializeField] private GameObject itemImage;  //보유(착용) 아이템 아이콘
    [SerializeField] private TextMeshProUGUI countText; //보유 아이템 수량 출력

    public Item Item { get { return item; } }
    public GameObject ItemImage { get { return itemImage; } }

    public void AddItemCount()  //아이템 수량 추가
    {
        if (item != null)
        {
            item.ItemCount++;
            countText.text = item.ItemCount.ToString();
        }
    }

    public void AddItemCount(int amount)  //아이템 수량 추가. 오버로딩
    {
        if (item != null)
        {
            item.ItemCount += amount;
            countText.text = item.ItemCount.ToString();
        }
    }

    public void RemoveItemCount() //아이템 수량 감소.
    {
        if (item != null)
        {
            item.ItemCount--;
            countText.text = item.ItemCount.ToString();
            if (item.ItemCount == 1)
                countText.text = "";
        }
    }

    public void RemoveItemCount(int amount) //아이템 수량 감소. 오버로딩
    {
        if (item != null)
        {
            item.ItemCount -= amount;
            countText.text = item.ItemCount.ToString();
            if (item.ItemCount == 1)
                countText.text = "";
        }
    }

    public virtual void SlotSetUI(Item item)    //슬롯 UI 세팅
    {
        SlotSetNull();
        this.item = item;
        itemImage.GetComponent<Image>().sprite = item.ItemImage;
        //itemImage.GetComponent<UIDragAndDrop>().SetItem(item);

        if (countText != null)
        {
            if (item.Type == ItemType.Used && item.ItemCount > 1)
                countText.text = item.ItemCount.ToString();
            else
                countText.text = "";
        }

        switch (item.Type)
        {
            case ItemType.Used:
                itemImage.GetComponent<UIDragAndDrop>().CanUseSlot =
                    new List<uiNumbers> { uiNumbers.QuikSlots, uiNumbers.InventoryUI };
                break;
            case ItemType.Equipment:
                itemImage.GetComponent<UIDragAndDrop>().CanUseSlot =
                    new List<uiNumbers> { uiNumbers.PlayerStatusUI, uiNumbers.InventoryUI };
                break;
            default:
                break;
        }

        itemImage.SetActive(true);
    }

    public virtual void SlotSetNull()   //슬롯 UI 초기화
    {
        item = null;
        itemImage.GetComponent<Image>().sprite = null;
        //itemImage.GetComponent<UIDragAndDrop>().SetItem(null);
        itemImage.SetActive(false);

        if (countText != null)
            countText.text = "";
    }
}
