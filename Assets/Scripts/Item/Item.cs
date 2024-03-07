using UnityEngine;

public class Item
{
    private ItemType type; //아이템의 유형
    private EquipType equipType;    //장비 타입
    private EffectType effectType;    //장비 타입
    private ItemGrade grade;   //아이템 등급
    private GameObject itemPrefab;    //아이템 오브젝트 프리팹
    private Sprite itemImage; //UI에 출력할 아이템 이미지
    private string itemName;   //아이템의 이름
    private PlayerClass itemClass; //아이템 직업
    private int buyPrice;  //아이템 구매 가격
    private int canUseLevel; //아이템 착용 가능 레벨
    private int itemAbillity; //아이템 능력치(공격력,방어력,회복량 등)
    private int itemHP; //아이템 추가 생명력
    private int itemMP; //아이템 추가 마나
    [Multiline] private string itemDescription; //아이템 부가 설명

    #region 속성들
    public ItemType Type { get { return type; } }
    public EquipType EquipType { get { return equipType; } }
    public EffectType EffectType { get { return effectType; } }
    public ItemGrade Grade { get { return grade; } }
    public GameObject ItemPrefab { get { return itemPrefab; } }

    public Sprite ItemImage { get { return itemImage; } }
    public string ItemName { get { return itemName; } }
    public PlayerClass ItemClass { get { return itemClass; } }
    public int BuyPrice { get { return buyPrice; } }
    public int CellPrice { get { return (int)(buyPrice * 0.8f); } }
    public int CanUseLevel { get { return canUseLevel; } }
    public int ItemAbillity { get { return itemAbillity; } }
    public int ItemHP { get { return itemHP; } }
    public int ItemMP { get { return itemMP; } }

    public string ItemDescription { get { return itemDescription; } }

    public int ItemCount { get; set; }
    #endregion

    public void SetItem(Item_SO item, int itemCount)
    {
        type = item.Type;
        equipType = item.EquipType;
        effectType = item.EffectType;
        grade = item.Grade;
        itemPrefab = item.ItemPrefab;
        itemImage = item.ItemImage;
        itemName = item.ItemName;
        itemClass = item.ItemClass;
        buyPrice = item.Price;
        canUseLevel = item.CanUseLevel;
        itemAbillity = item.ItemAbillity;
        itemDescription = item.ItemDescription;
        ItemCount = itemCount;
    }

    public void SetItem(Item item, int itemCount)
    {
        type = item.Type;
        equipType = item.EquipType;
        effectType = item.EffectType;
        grade = item.Grade;
        itemPrefab = item.ItemPrefab;
        itemImage = item.ItemImage;
        itemName = item.ItemName;
        itemClass = item.ItemClass;
        buyPrice = item.BuyPrice;
        canUseLevel = item.CanUseLevel;
        itemAbillity = item.ItemAbillity;
        itemDescription = item.ItemDescription;
        ItemCount = itemCount;
    }
}
