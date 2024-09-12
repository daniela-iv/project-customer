using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue Asset")]
public class Dialogue : ScriptableObject
{
    public DialogueNode RootNode;

   /* public TMP_Text text;
    private string currentText;

    CanvasGroup group

    void Start()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;

        Show("hey im a dialogue fuck you");
    }

    public void Show(string text)
    {
        group.alpha = 1f;
        currentText = text;
        print("test 1");
        StartCoroutine(DisplayText());
    }

    public void Close()
    {
        StopAllCoroutines();
        group.alpha = 0f;
    }

    private IEnumerator DisplayText()
    { 
        text.text = "";

        print("test 2");

        string originalText = currentText;
        string displayText = "";
        int alphaIndex = 0;

        foreach(char c in currentText.ToCharArray())
        {
            alphaIndex++;
            text.text = originalText;
            displayText = text.text.Insert(alphaIndex, "<color=#00000000>");
            text.text = displayText;

            print("test 3");

            yield return new WaitForSecondsRealtime(0.01f);
        }


        yield return null;
    }*/
}
