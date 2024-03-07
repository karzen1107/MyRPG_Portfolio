using System.Collections.Generic;
using UnityEngine;

public class NpcAction : MonoBehaviour, IInteractive
{
    [SerializeField] private Outline outline;   //아웃라인 스크립트
    [SerializeField] private DrawQuestDialog dialog;    //퀘스트 다이얼로그 스크립트
    [SerializeField] private Npc npc;

    [Header("대화모드메인카메라 위치")]
    [SerializeField] private Vector3 camPos;  //카메라 포지션
    [SerializeField] private Vector3 camRot;    //카메라 회전값

    // 상점 관련 //
    [SerializeField] private List<Item_SO> sellItems = new List<Item_SO>();
    private GameObject shopUI; //상점 UI
    [SerializeField] protected GameObject visitShopButton;

    // 컴포넌트 //
    protected Animator ani;
    protected QuestManager questManager;
    private UIManager uiManager;

    [Space(10)]
    [SerializeField] protected List<MainQuestData> questDatas = new List<MainQuestData>();  //메인 퀘스트 데이터

    public List<Item_SO> SellItems { get { return sellItems; } }
    public Npc NPC { get { return npc; } }

    public void DoAction()
    {
        SetOutLine(false);
        ani.SetBool("IsTalking", true);
        ani.SetTrigger("IsTalking_TR");

        if (questDatas.Count < 1)  //퀘스트가 없다면
        {
            DoOwnAction();
        }
        else //퀘스트가 있다면
        {
            UIManager.Instance.SetNPCTalkingState(this.transform, camPos, camRot, NpcActionState.TalkQuest);
            questDatas[0].questState = questManager.GetQuestState(questDatas[0]);
            CheckNpcQuest(questDatas[0]);
            switch (questDatas[0].questState)   //퀘스트 상태에 따른 다이얼로그 호출
            {
                case QuestState.Ready:
                    dialog.SettingDialog(questDatas[0].dialogIndex);
                    dialog.NowTalkingNpc(this);
                    break;
                case QuestState.Accept:
                    dialog.SettingDialog(questDatas[0].dialogIndex + 1);
                    dialog.NowTalkingNpc(this);
                    break;
                case QuestState.Complete:
                    dialog.SettingDialog(questDatas[0].dialogIndex + 2);
                    dialog.NowTalkingNpc(this);
                    break;
                case QuestState.None:
                    DoOwnAction();
                    break;
            }
        }
    }

    private void DoOwnAction()  //각 NPC 역할의 고유 행동 수행
    {
        switch (npc.npcType)
        {
            case NpcType.JustQuests:
                break;
            case NpcType.GeneralShop:
            case NpcType.WeaponShop:
                OpenShop();
                break;
        }
    }

    public void RemoveQuest(MainQuestData questData)    //퀘스트 삭제
    {
        if (questData == null)
            return;

        questDatas.Remove(questData);
    }

    public void RemoveQuest(int questNum)    //퀘스트 삭제
    {
        if (questNum < 0)
            return;

        for (int i = 0; i < questDatas.Count; i++)
        {
            if (questDatas[i].number == questNum)
                questDatas.RemoveAt(i);
        }
    }

    public void OpenShop()  //상점 오픈
    {
        uiManager.SetNPCTalkingState(this.transform, camPos, camRot, NpcActionState.OpenShop);
        shopUI.SetActive(true);
        shopUI.GetComponent<ShopUI>().SettingShop(sellItems);
        uiManager.DefaultCanvas.GetComponent<DefaultCanvas>().inventoryUI.SetActive(true);
    }

    protected virtual void CheckNpcQuest(MainQuestData questData) { }

    public void CheckPlayerQuestState() //게임 시작시 플레이어와 NPC사이의 퀘스트 체크하여 알맞은 상태 설정
    {
        for (int i = 0; i < questManager.PlayerCompleteQuestData.Count; i++)    //플레이어가 완료한 퀘스트 검토
        {
            CheckNpcQuest(questManager.PlayerCompleteQuestData[i]);
            for (int j = 0; j < questDatas.Count; j++)
            {
                if (questManager.PlayerCompleteQuestData[i].number == questDatas[j].number)
                    questDatas.RemoveAt(j);
            }

        }

        for (int i = 0; i < questManager.PlayerAcceptQuestData.Count; i++)  //플레이어가 진행중인 퀘스트 검토
        {
            CheckNpcQuest(questManager.PlayerAcceptQuestData[i]);
            for (int j = 0; j < questDatas.Count; j++)
            {
                while (questDatas[0].miniQuests.Count != questManager.PlayerAcceptQuestData[i].miniQuests.Count)
                    questDatas[0].miniQuests.RemoveAt(0);
            }
        }
    }

    public void SetOutLine(bool isShow) //아웃라인 설정
    {
        outline.enabled = isShow;
    }

    public void SetTalkingAni() //IsTalking 애니메이션 파라미터 설정
    {
        this.gameObject.GetComponent<Animator>().SetBool("IsTalking", false);
    }

    protected virtual void Start()
    {
        questManager = QuestManager.Instance;
        uiManager = UIManager.Instance;
        ani = this.gameObject.GetComponent<Animator>();
        shopUI = uiManager.DefaultCanvas.GetComponent<DefaultCanvas>().shopUI;
        questDatas = questManager.GetNpcQuests(npc.npcIndex);

        //오브젝트 찾기
        GameObject talkingCanvase = GameObject.Find("OverrayCanvases").transform.Find("TalkingCanvas").gameObject;
        visitShopButton = talkingCanvase.transform.Find("ShopButton").gameObject;
        dialog = talkingCanvase.GetComponent<DrawQuestDialog>();

        // 유니티 이벤트 등록
        questManager.ev_RemoveOngoingQuest += RemoveQuest;
        uiManager.ev_SetNpcTalkingAni += SetTalkingAni;
        questManager.ev_CheckNpcQuest += CheckNpcQuest;

        CheckPlayerQuestState();
    }
}

public enum NpcActionState
{
    TalkQuest, OpenShop
}
