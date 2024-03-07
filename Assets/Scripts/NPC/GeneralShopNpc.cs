using UnityEngine;

public class GeneralShopNpc : NpcAction
{
    [SerializeField] private Vector3 defaultPos;
    [SerializeField] private Vector3 defaultRot;

    [SerializeField] WeaponShopNpc weaponShopNpc;   //무기 상인

    protected override void CheckNpcQuest(MainQuestData questData)    //플레이어의 퀘스트 상태에 따른 Npc 세팅
    {
        visitShopButton.SetActive(false);
        // 100번 '카룸 처치' 퀘스트 완료시 NPC 애니메이션 상태 변경
        if (questData.number == 100 && questData.questState == QuestState.Complete)
        {
            ani.SetBool("QuestCompleted_100", true);
            return;
        }

        // 200번 '마리암의 보답' 퀘스트 수락시 NPC 위치 이동
        if (questData.number == 200 && questData.questState != QuestState.Ready)
        {
            this.transform.position = defaultPos;
            this.transform.rotation = Quaternion.Euler(defaultRot);
            visitShopButton.SetActive(true);
            weaponShopNpc.IsDaughterCome = true;
            return;
        }
    }
}
