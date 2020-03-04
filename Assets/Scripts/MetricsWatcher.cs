using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MetricsWatcher : MonoBehaviour
{
    public int queueLength;

    private Queue<float> journeyTimes = new Queue<float>();

    void Start()
    {
        
    }

    void Update()
    {
        //Debug.Log(reportJourneyTimeMean());
    }

    public void addJourneyTime(float time)
    {
        journeyTimes.Enqueue(time);

        while (journeyTimes.Count > queueLength)
            journeyTimes.Dequeue();
    }

    public float reportJourneyTimeMean()
    {
        if (journeyTimes.Count == 0) return 1000f;

        return journeyTimes.Average();
    }
}
