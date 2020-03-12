using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Experiment : MonoBehaviour
{
    public int maxCarsInExperiment;

    private int carsPassed = 0;

    private void finishCar()
    {
        if (maxCarsInExperiment > 0 && ++carsPassed >= maxCarsInExperiment)
            SceneManager.LoadScene("MenuScene");
    }
}
