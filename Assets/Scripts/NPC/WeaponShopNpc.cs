public class WeaponShopNpc : NpcAction
{
    public bool IsDaughterCome { get; set; }

    protected override void CheckNpcQuest(MainQuestData questData)
    {
        if (IsDaughterCome)
        {
            visitShopButton.SetActive(true);
            return;
        }

        questDatas[0].questState = QuestState.None;
    }
}
