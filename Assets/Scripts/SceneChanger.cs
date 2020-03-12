using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void startControl()
    {
        SceneManager.LoadScene("ControlScene");
    }

    public void startExpert()
    {
        SceneManager.LoadScene("ExpertScene");
    }

    public void startReinforcement()
    {
        SceneManager.LoadScene("InferenceScene");
    }
}
