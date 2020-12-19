using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IQuizSerializable
{  
    QuizData Serialize();
    void Deserialize(QuizData data);
}

[System.Serializable]
public class QuizData
{
    public List<int> mIntData = new List<int>();
    public List<string> mStringData = new List<string>();
    public List<float> mFloatData = new List<float>();
    public List<bool> mBoolData = new List<bool>();
}
