using UnityEngine;

[CreateAssetMenu(fileName = "InstrumentShowcaseSO_", menuName = "Scriptable Objects/Instrument Showcase")]
public class InstrumentShowcaseSO : ScriptableObject
{
    public string instrumentName;
    public GameObject instrumentPrefab;
    public Sprite instrumentSprite;
}
