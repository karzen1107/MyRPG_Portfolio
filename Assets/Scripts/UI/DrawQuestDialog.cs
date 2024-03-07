using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class DrawQuestDialog : MonoBehaviour
{
    // Xml 다이얼로그 //
    [SerializeField] private string xmlFile = "QuestDialog"; //파일 경로
    [SerializeField] private string nodeString = "root/dialog"; //파일 내 노드 경로

    private XmlNodeList allNodes;   //모든 노드 리스트
    private Queue<Dialog> dialogs;

    private int nextDialog;

    // Npc 대화 UI //
    private NpcAction nowNpc;   //현재 대화중인 NPC
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI sentenceText;
    [SerializeField] GameObject nextButton;

    // 컴포넌트 //
    [SerializeField] private GameObject questUI;    //퀘스트 창 UI
    private QuestManager questManager;

    private void LoadDialogXml(string fileName, string nodeStr)     //Xml파일 불러오기
    {
        var xmlAsset = Resources.Load<TextAsset>("Dialog/" + fileName);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlAsset.text);
        allNodes = xmlDoc.SelectNodes(nodeStr);
    }

    private void InitDialog()   //다이얼로그 초기화
    {
        dialogs.Clear();
        nameText.text = string.Empty;
        sentenceText.text = string.Empty;
    }

    public void SettingDialog(int dialogNext)   //다이얼로그 세팅
    {
        if (allNodes == null)
            LoadDialogXml(xmlFile, nodeString);

        if (dialogs == null)
        {
            dialogs = new Queue<Dialog>();
            InitDialog();
        }

        foreach (XmlNode node in allNodes)
        {
            int num = int.Parse(node["number"].InnerText);  //노드의 number에 해당하는 텍스트 정보를 int형으로 변환하여 저장

            if (num == dialogNext)
            {
                Dialog dialog = new Dialog();

                dialog.number = num;
                dialog.character = int.Parse(node["character"].InnerText);
                dialog.name = node["name"].InnerText;

                if (node["sentence"].InnerText == "Empty")
                    dialog.sentence = string.Empty;
                else
                    dialog.sentence = node["sentence"].InnerText;

                dialog.next = int.Parse(node["next"].InnerText);

                dialogs.Enqueue(dialog);    //dialogs에 생성한 dialog 집어넣기
            }
        }

        DrawDialog();
    }

    public void DrawDialog()   //다이얼로그 사용
    {
        if (dialogs.Count == 0)
        {
            EndDialog();
            return;
        }

        Dialog dialog = dialogs.Dequeue();

        nameText.text = dialog.name;
        sentenceText.text = dialog.sentence;
        nextDialog = dialog.next;   //다음 대사
    }

    private void EndDialog()    // 다이얼로그 넘버 전환
    {
        InitDialog();

        if (nextDialog >= 0)    //다음 대화가 있다면
        {
            SettingDialog(nextDialog);
        }
        else  //다음 대화가 없다면
        {
            switch (questManager.currentQuest.questState)
            {
                case QuestState.Ready:
                    //퀘스트 UI 오픈
                    questUI.GetComponent<QuestUI>().SetQuestData(questManager.currentQuest);
                    questUI.SetActive(true);
                    break;
                case QuestState.Accept:
                    questManager.currentQuest.UpdateSocialQuest(nowNpc);
                    questUI.GetComponent<QuestUI>().CloseClick();
                    break;
                case QuestState.Complete:
                    questUI.GetComponent<QuestUI>().SetRewardData(questManager.currentQuest);
                    questUI.SetActive(true);
                    break;
            }
        }
    }

    public void NowTalkingNpc(NpcAction npcAction)  //현재 대화중인 Npc설정
    {
        nowNpc = null;
        nowNpc = npcAction;
    }

    public void NextButtonClick()   //다음 버튼 클릭시 다음 다이얼로그 재생
    {
        SettingDialog(nextDialog);
    }

    public void VisitShopClick()    //상점 방문 버튼 클릭
    {
        Debug.Log("상점 방문");
        nowNpc.OpenShop();
    }

    private void Start()
    {
        questManager = QuestManager.Instance;
        SystemManager.Instance.ev_InitDialog += InitDialog;
    }
}
