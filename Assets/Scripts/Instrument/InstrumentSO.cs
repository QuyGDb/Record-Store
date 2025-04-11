using UnityEngine;
using UnityEngine.Video;
[CreateAssetMenu(fileName = "InstrumentSO", menuName = "Scriptable Objects/InstrumentSO")]
public class InstrumentSO : ScriptableObject
{
    public string instrumentName;
    [TextArea]
    public string description;
    public VideoClip videoClip;
    public string videoClipTitle;
}
