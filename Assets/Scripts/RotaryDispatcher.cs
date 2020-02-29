using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotaryDispatcher : MonoBehaviour
{
    public AllLightsController lightsController;

    private float ticker = 0f;
    private int counter = 0;
    private int counterMax;

    void Start()
    {
        counterMax = lightsController.junctions.Count;
    }

    void Update()
    {
        ticker += Time.deltaTime;

        if (ticker > 3f) {
            ticker = 0f;

            var nextJunction = lightsController.junctions[counter++];
            bool previousAlignment = nextJunction.Controller.northSouthAlign;
            nextJunction.Controller.goGreenNorthSouth(!previousAlignment);

            if (counter >= counterMax) counterMax = 0;
        }
    }
}
