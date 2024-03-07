[System.Serializable]
public class Npc
{
    public int npcIndex;
    public string name;
    public NpcType npcType;
}

public enum NpcType
{
    GeneralShop, WeaponShop, JustQuests
}
