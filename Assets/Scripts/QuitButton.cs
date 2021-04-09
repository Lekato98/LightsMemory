using System;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {
    public Button quitBtn;
    
    private static void QuitGame() {
        Application.Quit();
    }

    public void Start() {
        quitBtn.onClick.AddListener(QuitGame);
    }
}
