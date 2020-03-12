using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartControlScene : MonoBehaviour
{
    public void StartControl()
    {
        SceneManager.LoadScene("ControlScene");
    }
}
