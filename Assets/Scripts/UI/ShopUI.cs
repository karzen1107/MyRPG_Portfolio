using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("NPC 판매품 세팅")]
    [SerializeField] private GameObject itemPrefab; //아이템 프리펩
    [SerializeField] private Transform prefabParent;    //프리펩 부모위치
    private List<GameObject> itemPrefabs = new List<GameObject>();  //구매품 슬롯 리스트

    [Header("플레이어 판매품 세팅")]
    [SerializeField] private GameObject reCellSlot; //플레이어 판매 슬롯
    private List<GameObject> reCellSlots = new List<GameObject>();  //판매한 슬롯 리스트

    // 스크롤 뷰 //
    [SerializeField] private RectTransform viewPort;
    [SerializeField] private RectTransform contents;
    private float contentsSize;   //.스크롤뷰 컨텐츠 가로 사이즈
    [SerializeField] private Scrollbar hScroll; //가로 스크롤바

    private UIManager uiManager;

    public void SettingShop(List<Item_SO> items)    //상점 세팅
    {
        InitShop();
        for (int i = 0; i < items.Count; i++)
        {
            itemPrefabs.Add(Instantiate(itemPrefab, prefabParent));
            itemPrefabs[i].GetComponent<ShopItem>().SettingItem(items[i]);
        }
    }

    private void InitShop() //상점 초기화
    {
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            Destroy(itemPrefabs[i]);
        }
        itemPrefabs.Clear();
    }

    public void CloseClick()    //상점 닫기
    {
        uiManager.SetDefaultState();
        uiManager.DefaultCanvas.GetComponent<DefaultCanvas>().inventoryUI.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void SetReCellViewPort() // 판매한 목록 뷰포트 세팅
    {
        contentsSize += 57.5f;
        float dWidth = contentsSize - viewPort.rect.width;    //contents 높이야 viewPort 높이 차
        if (dWidth > 0)
        {
            contents.sizeDelta = new Vector2(contentsSize, contents.rect.height);
            hScroll.value = 1;
        }
    }

    public void AddReCellItem(Item item, int amount)    //플레이어가 판매한 아이템 세팅
    {
        for (int i = 0; i < reCellSlots.Count; i++)
        {
            for (int j = 0; j < amount; j++)
            {
                InventorySlot temp = reCellSlots[i].GetComponent<InventorySlot>();
                if (temp.Item.ItemPrefab == item.ItemPrefab && temp.Item.ItemCount < 5)
                {
                    temp.AddItemCount();
                    item.ItemCount--;
                }
            }
        }

        if (item.ItemCount <= 0)
            return;

        GameObject recellSlot = Instantiate(this.reCellSlot, contents);
        recellSlot.GetComponent<InventorySlot>().SlotSetUI(item);
        reCellSlots.Add(recellSlot);
        SetReCellViewPort();
    }

    private void OnPointerEnter()   //마우스가 슬롯 위로 올라갔을때 정보 보이기
    {
        foreach (RaycastResult result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
        {
            if (result.gameObject.GetComponent<InventorySlot>() != null)
            {
                GameObject selected = result.gameObject;

                for (int i = 0; i < reCellSlots.Count; i++)
                {
                    if (reCellSlots[i] == selected)
                    {
                        uiManager.IconOver(reCellSlots[i].GetComponent<InventorySlot>().Item);
                        break;
                    }
                }
            }
            else if (result.gameObject.GetComponent<ShopItem>() != null)
            {
                GameObject selected = result.gameObject;

                for (int i = 0; i < itemPrefabs.Count; i++)
                {
                    if (itemPrefabs[i] == selected)
                    {
                        uiManager.IconOver(itemPrefabs[i].GetComponent<ShopItem>().Item);
                        break;
                    }
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

    private void Start()
    {
        uiManager = UIManager.Instance;
        contentsSize = 10f;
    }

    private void Update()
    {
        if (uiManager.ValidNum == uiNumbers.ShopUI)
        {
            OnPointerEnter();
            //OnPointerDown();
            //InteractItem();
        }

        OnPointerExit();
    }
}
