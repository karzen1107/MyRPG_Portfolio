using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Item_SO : ScriptableObject
{
    [SerializeField] private ItemType type = ItemType.Max; //아이템의 유형
    [SerializeField] private EquipType equipType = EquipType.Max;   //장비 타입
    [SerializeField] private EffectType effectType = EffectType.Max;    //속성 타입
    [SerializeField] private ItemGrade grade = ItemGrade.Normal;   //아이템 등급
    [SerializeField] private GameObject itemPrefab;    //아이템 오브젝트 프리팹
    [Space(10)]
    [SerializeField] private Sprite itemImage; //UI에 출력할 아이템 이미지
    [SerializeField] private string itemName;   //아이템의 이름
    [SerializeField] private PlayerClass itemClass = PlayerClass.None; //아이템 직업
    [SerializeField] private int price; //아이템 가격
    [Space(10)]
    [SerializeField] private int canUseLevel; //아이템 착용 가능 레벨
    [SerializeField] private int itemAbillity; //아이템 능력치(공격력,방어력,회복량 등)

    [SerializeField, Multiline] private string itemDescription; //아이템 부가 설명

    [SerializeField] private int goldAmount; //골드 전용

    #region 속성들
    public ItemType Type { get { return type; } }
    public EquipType EquipType { get { return equipType; } }
    public EffectType EffectType { get { return effectType; } }
    public ItemGrade Grade { get { return grade; } }

    public GameObject ItemPrefab { get { return itemPrefab; } }

    public Sprite ItemImage { get { return itemImage; } }
    public string ItemName { get { return itemName; } }
    public PlayerClass ItemClass { get { return itemClass; } }
    public int Price { get { return price; } }

    public int CanUseLevel { get { return canUseLevel; } }
    public int ItemAbillity { get { return itemAbillity; } }

    public string ItemDescription { get { return itemDescription; } }
    public int GoldAmount { get { return goldAmount; } set { goldAmount = value; } }
    #endregion
}

/// <summary> Equipment : 장비 / Used : 소모품 / Gold : 골드 / etc : 기타 </summary>
public enum ItemType    //아이템 타입
{
    Equipment, Used, Gold, Max
}

/// <summary> Head : 헬멧 / Weapon : 주무기 / Sheild : 방패(보조무기) / Armor : 갑옷 / Gloves : 장갑 / Pants : 바지 / Belt : 벨트 / Boots : 신발 </summary>
public enum EquipType   //장비 타입
{
    Head, Weapon, Sheild, Armor, Gloves, Pants, Belt, Boots, Max
}

public enum EffectType   //소모품 타입
{
    Hp, Mp, Atk, Def, Max
}

/// <summary> Normal : 일반 / Advanced : 고급 / Rare : 희귀 / Hero : 영웅 / Legend : 전설 </summary>
public enum ItemGrade   //아이템 등급
{
    Normal, Advanced, Rare, Hero, Legend, Max
}