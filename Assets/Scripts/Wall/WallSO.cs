using UnityEngine;

[CreateAssetMenu(fileName = "WallSO_", menuName = "Scriptable Objects/WallSO", order = 1)]
public class WallSO : ScriptableObject
{
    public string wallName;
    public GameObject wallPrefab;
}
