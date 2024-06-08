using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestionHandler : MonoBehaviour
{
    [Header("Questions")]
    public GameObject questionHolder;

    [Header("Question Panel")]
    public TMP_Text question_title;
    public GameObject content;
    public GameObject answerPrefab;

    [Header("Screens")]
    public GameObject endScreen;
    public GameObject questionScreen;

    private int question_id;
    private Question question;
    private int question_amount;
    private int[] points;
    private void Start()
    {
        // retrieve necessary data from questionholder about every question
        question_id = Convert.ToInt32(questionHolder.transform.GetChild(0).gameObject.name);
        question_amount = questionHolder.transform.childCount;
        question = questionHolder.transform.GetChild(question_id-1).gameObject.GetComponent<Question>();
        Debug.Log(question_id);
        // set first question
        points = new int[question_amount];
        refreshQuestion();
    }
    public void refreshQuestion()
    {
        question = questionHolder.transform.GetChild(question_id - 1).gameObject.GetComponent<Question>();
        question_title.text = question_id + ". " + question.question;
        DestroyAllChildren(content);
        int answerIndex = 0;
        foreach (string st in question.answers)
        {
            GameObject ans = Instantiate(answerPrefab, content.transform);
            TMP_Text title = ans.transform.Find("anstitle").GetComponent<TMP_Text>();
            Toggle toggle = ans.transform.Find("Toggle1").GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(CheckCorrectAnswer);
            title.text = st;
            ans.name = answerIndex.ToString();
            answerIndex++;
        }
    }

    public void Forward()
    {
        PrintArray(points);
        question_id++; 
        if (question_id > question_amount)
        {
            question_id = question_amount;
            EndScreen();
        } else
        {   
            refreshQuestion();
        }
    }
    
    public void Backward()
    {
        PrintArray(points);
        question_id--;
        if (question_id < 1)
        {
            question_id = 1;
            Debug.Log("cannot go negative, id: "+question_id);
        } else
        {
            refreshQuestion();
        }
    }
    public void DestroyAllChildren(GameObject o)
    {
        foreach (Transform child in o.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void CheckCorrectAnswer(bool redundant)
    {
        bool[] correctAnswerArray = new bool[content.transform.childCount];
        int index = 0;
        Toggle[] toggles = content.GetComponentsInChildren<Toggle>().Where(t => t.name == "Toggle1").ToArray();
        foreach (Toggle toggle in toggles)
        {
            correctAnswerArray[index] = toggle.isOn;
            index++;
        }
        PrintArray(correctAnswerArray);
        index = 0;
        foreach (bool bl in question.correctAnswerIndex)
        {
            if (bl != correctAnswerArray[index])
            {
                points[question_id - 1] = 0;
                return;
            } 
            index++;
        }
        points[question_id - 1] = 1;
    }
    public void EndScreen()
    {
        questionScreen.SetActive(false);
        endScreen.SetActive(true);
        int totalPoints = 0;
        foreach (int pt in points)
        {
            totalPoints += pt;
        }
        endScreen.transform.Find("Points").gameObject.GetComponent<TMP_Text>().text = totalPoints+" points!";
    }
    public void PrintArray<T>(T[] array)
    {
        string arrayString = string.Join(", ", array.Select(item => item.ToString()));
        Debug.Log("Array: [" + arrayString + "]");
    }
}
