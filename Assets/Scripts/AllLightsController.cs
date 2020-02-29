using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllLightsController : MonoBehaviour
{
    [HideInInspector] public AllLightsController instance = null;
    [HideInInspector] public List<Junction> junctions = new List<Junction>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
