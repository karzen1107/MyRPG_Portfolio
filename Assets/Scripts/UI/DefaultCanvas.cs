using UnityEngine;

public class DefaultCanvas : MonoBehaviour
{
    public GameObject playerStatusUI;
    public GameObject playUI;
    public GameObject inventoryUI;
    public GameObject skillBookUI;
    public GameObject actCheckUI;
    public GameObject enemyBossHpUI;
    public GameObject shopUI;

    public void OpenActCheckUI(string sentence, string sceneName)   //씬 전환용
    {
        actCheckUI.SetActive(true);
        actCheckUI.GetComponent<ActCheckUI>().SettingUI(sentence, sceneName);
    }

    public void OpenActCheckUI(string sentence, int sceneNum)   //씬 전환용
    {
        actCheckUI.SetActive(true);
        actCheckUI.GetComponent<ActCheckUI>().SettingUI(sentence, sceneNum);
    }

    public void OpenActCheckUI(string sentence, Item item, int index)   //상점 이용용
    {
        actCheckUI.SetActive(true);
        actCheckUI.GetComponent<ActCheckUI>().SettingUI(sentence, item, index);
    }

    public void OpenActCheckUI(string sentence)   //상점 이용용
    {
        actCheckUI.SetActive(true);
        actCheckUI.GetComponent<ActCheckUI>().SettingUI(sentence);
    }
    public void CloseActCheckUI()
    {
        actCheckUI.SetActive(false);
    }
}
