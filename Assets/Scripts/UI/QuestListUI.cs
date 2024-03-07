using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] private GameObject ongoingQuestUI; //진행중인 퀘스트 목록 프리팹
    [SerializeField] private Transform ongoingParent;  //퀘스트들 부모
    private List<OngoingQuestUI> ongoingQuests = new List<OngoingQuestUI>();

    // 컴포넌트 //
    private QuestManager questManager;

    public void MinMaxClick()   //퀘스트 리스트 최소&최대화 버튼
    {

    }

    public void SetNewQuest(MainQuestData mainQuest)    //새로운 퀘스트 세팅
    {
        GameObject newOngoingQuest = Instantiate(ongoingQuestUI, ongoingParent);
        newOngoingQuest.GetComponent<OngoingQuestUI>().SetQuestData(mainQuest);
        newOngoingQuest.SetActive(false);
        newOngoingQuest.SetActive(true);

        ongoingQuests.Add(newOngoingQuest.GetComponent<OngoingQuestUI>());
    }

    public void RemoveCompleteQuest(MainQuestData mainQuest)    //완료한 퀘스트 삭제
    {
        for (int i = 0; i < ongoingQuests.Count; i++)
        {
            if (ongoingQuests[i].GetComponent<OngoingQuestUI>().OnQuest.mainQuest.number == mainQuest.number)
            {
                Destroy(ongoingQuests[i].gameObject);
                ongoingQuests.RemoveAt(i);
                break;
            }
        }
    }

    public List<OngoingQuestUI> ReturnOngoinQuests()    //퀘스트 리스트 반환
    {
        return ongoingQuests;
    }


    private void Awake()
    {
        questManager = QuestManager.Instance;

        questManager.ev_SetOngoingQuest += SetNewQuest;
        questManager.ev_RemoveOngoingQuest += RemoveCompleteQuest;
        questManager.func_ReturnOngoingQuests += ReturnOngoinQuests;
    }
}
