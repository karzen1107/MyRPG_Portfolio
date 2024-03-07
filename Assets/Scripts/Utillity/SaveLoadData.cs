using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadData
{
    public static void SaveData()   //데이터 저장
    {
        //C:\Users\82107\AppData\LocalLow\DefaultCompany\MyRPG
        //C:/Users/PC-02/AppData/LocalLow/DefaultCompany/MyRPG
        string path = Application.persistentDataPath + "/Portfolio.dat";    //저장할 데이터 저장 경로. https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
        BinaryFormatter formatter = new BinaryFormatter();  // 저장할 데이터 2진화 준비
        FileStream fs = new FileStream(path, FileMode.Create);  // 저장할 파일 경로에 접근(path). 해당 경로 파일이 없을시 신규 생성(FileMode.Create).
        PlayerData pData = new PlayerData();    //저장할 데이터 생성
        formatter.Serialize(fs, pData); //데이터(pData)를 2진화(Serialize) 하여 지정한 파일(fs)에 저장
        fs.Close(); //파일 닫기.꼭 닫아야 함
    }

    public static PlayerData LoadData()   //데이터 불러오기
    {
        string path = Application.persistentDataPath + "/Portfolio.dat";    //불러올 파일 경로.

        if (File.Exists(path))   //해당 경로에 파일이 있는지 체크
        {
            BinaryFormatter formatter = new BinaryFormatter();  //불러올 데이터 역2진화 준비
            FileStream fs = new FileStream(path, FileMode.Open);    //불러올 데이터 경로 접근 및 열기
            PlayerData pData = formatter.Deserialize(fs) as PlayerData; //데이터를 역2진화(Deserialize) 하여 PlayerData형식으로 저장
            fs.Close(); //파일 닫기.
            return pData;
        }
        else
        {
            PlayerData data = new PlayerData();
            data.charName = "카르젠";
            data.charClass = PlayerClass.Warrior;
            data.charLevel = 1;
            data.startLife = 500;
            data.startMana = 300;
            data.exp = 0;
            data.startAtk = 10;
            data.startShield = 10;
            data.startTrans.SetStartPos(234, 15, 193);
            data.startTrans.SetStartRot(0, 0, 0, 0);
            data.playerAcceptQuestData = new List<MainQuestData>();
            data.ongoingQuests = new List<OngoingQuest>();
            data.inventoryData = new InventoryData();
            data.inventoryData.useableSlotCount = 8;
            data.inventoryData.slots = new InvenSlotData[data.inventoryData.useableSlotCount];
            data.inventoryData.gold = 0;
            return data;    //해당 경로 파일이 없다면 신규 데이터 반환
        }
    }
}
