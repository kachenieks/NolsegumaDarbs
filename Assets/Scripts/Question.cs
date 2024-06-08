using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    [TextArea]
    [Header("Answer and Correct Answer Index array length should be the same!")]
    public int questionNum;
    public string question;
    public string[] answers;
    public bool[] correctAnswerIndex;
}
