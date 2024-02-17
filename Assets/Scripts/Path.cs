using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Path")]
public class Path : ScriptableObject
{
    public Vector3[] _waypoints;
}