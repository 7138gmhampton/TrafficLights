using UnityEngine;

public class RotaryDispatcher : MonoBehaviour
{
    public AllLightsController lightsController;

    private float ticker = 0f;
    private int counter = 0;

    void Update()
    {
        ticker += Time.deltaTime;

        if (ticker > 3f) {
            ticker = 0f;

            var nextJunction = lightsController.junctions[counter++];
            int xCursor = nextJunction.XLocus;
            int yCursor = nextJunction.YLocus;

            if (lightsController.checkAlignment(xCursor, yCursor))
                lightsController.switchEW(xCursor, yCursor);
            else lightsController.switchNS(xCursor, yCursor);

            if (counter >= lightsController.junctions.Count) counter = 0;
        }
    }
}
