using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListSO_", menuName = "Scriptable Objects/Music Object List")]
public class MusicObjectListSO : ScriptableObject
{
    public List<GameObject> instrumentList = new List<GameObject>();
}
