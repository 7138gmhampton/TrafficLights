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
        Debug.Log(counterMax);
    }

    void Update()
    {
        ticker += Time.deltaTime;
        //Debug.Log(lightsController.junctions.Count);

        if (ticker > 3f) {
            ticker = 0f;

            var nextJunction = lightsController.junctions[counter++];
            //bool previousAlignment = nextJunction.Controller.northSouthAlign;
            //nextJunction.Controller.goGreenNorthSouth(!previousAlignment);
            int xCursor = nextJunction.XLocus;
            int yCursor = nextJunction.YLocus;

            if (lightsController.checkAlignment(xCursor, yCursor))
                lightsController.switchEW(xCursor, yCursor);
            else lightsController.switchNS(xCursor, yCursor);

            if (counter >= lightsController.junctions.Count) counter = 0;
        }
    }
}
