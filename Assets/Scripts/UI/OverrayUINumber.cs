using UnityEngine;

public class OverrayUINumber : MonoBehaviour
{
    public uiNumbers uiNumber;

    public uiNumbers UINumber { get { return uiNumber; } }
}

public enum uiNumbers
{
    PlayerButtons, QuikSlots, PlayerStatusUI, InventoryUI, SkillBookUI, ShopUI, ActCheckUI, Max
}
