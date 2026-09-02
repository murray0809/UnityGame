using UnityEngine;

public class WaypointController : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints;

    public Transform GetWaypoint(int index)
    {
        if (index < 0 || index >= waypoints.Length)
        {
            return null;
        }

        return waypoints[index];
    }

    public int WaypointCount
    {
        get { return waypoints.Length; }
    }
}