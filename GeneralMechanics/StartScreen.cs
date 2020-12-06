using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

    private string stringToEdit = "How to play Moonatees:\nIf you wan't to see menu press esc";
    public Texture2D texture;
    private float startTime;
    private float levelMaximumWaitTime = 7.5f;
    void Start()
    {
        startTime = Time.time;

    }




    void OnGUI()
    {
        GUI.backgroundColor = Color.yellow;

        GUI.Label(new Rect(Screen.width / 2 - 350, 100, 400, 400), texture);
        stringToEdit = GUI.TextArea(new Rect(Screen.width / 2 - 400, Screen.height / 2, 500,
        75), stringToEdit, 200);
        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (Time.time > startTime + levelMaximumWaitTime)
        {
            //Application.LoadLevel("Level1");
        }

        if (GUI.Button(new Rect(700, Screen.height / 2, 200, 30), "New Game"))
        {
            //Application.LoadLevel("Level1");
        }
        // Make the second button.
        if (GUI.Button(new Rect(700, Screen.height / 2 + 50, 200, 30),
        "Quit"))
        {
            Application.Quit();
        }
    }
}
