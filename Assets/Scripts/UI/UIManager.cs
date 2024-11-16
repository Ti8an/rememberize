using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text[] text_timer;
    [SerializeField] private Text[] text_motion;
    [SerializeField] private GameObject panel_win;
    [SerializeField] private Button button_BackMenu;


    private void Awake()
    {
        GameManager.OnTimer += GameManager_OnSetTimer;
        GameManager.OnCount += GameManager_OnSetMotionCount;
        GameManager.OnWinning += GameManager_OnWin;
    }

    private void OnDestroy()
    {
        GameManager.OnTimer -= GameManager_OnSetTimer;
        GameManager.OnCount -= GameManager_OnSetMotionCount;
        GameManager.OnWinning -= GameManager_OnWin;
    }

    private void GameManager_OnWin()
    {
        panel_win.SetActive(true);
        text_motion[0].gameObject.SetActive(false);
        text_timer[0].gameObject.SetActive(false);
    }

    private void GameManager_OnSetMotionCount()
    {
        foreach(Text i in text_motion) 
        i.text = GameManager.Instance.MotionCount.ToString();
    }
    private void GameManager_OnSetTimer()
    {
        TimeSpan t = TimeSpan.FromSeconds(GameManager.Instance.TimerVal);
        foreach (Text i in text_timer)
            i.text = string.Format("{0}:{1}", t.Minutes, t.Seconds < 10 ? "0" + t.Seconds.ToString() : t.Seconds);
    }

    public void ButtonInMenu_Click(){
        SceneManager.LoadSceneAsync("MenuScene");
    }

    public void ButtonRestart_Click(){
        SceneManager.LoadSceneAsync("GameScene");
    }
}
