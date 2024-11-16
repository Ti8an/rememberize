using CGL;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private Button[] Buttons;

    public void ButtonPlay_Click()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void SetComplexity(int i)
    {
        GameData.complexity = i;

        foreach(Button b in Buttons) b.interactable = true;
        Buttons[i].interactable = false;
        AudioSource.Play();
    }
}
