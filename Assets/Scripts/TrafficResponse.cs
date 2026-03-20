using System;
using UnityEngine;

[Serializable]
public class Status
{
    public float vehicleDensity;
    public float averageSpeed;
    public string weather;
}

[Serializable]
public class PredictedStatus
{
    public int estimated_time;
    public Status predictions;
}

[Serializable]
public class TrafficResponse
{
    public Status current_status;
    public PredictedStatus[] predicted_status;
}
