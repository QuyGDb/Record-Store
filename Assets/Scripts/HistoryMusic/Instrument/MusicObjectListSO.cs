using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ListSO_", menuName = "Scriptable Objects/Music Object List")]
public class MusicObjectListSO : ScriptableObject
{
    [ShowInInspector]
    public List<InstrumentDetails> musicObjects = new List<InstrumentDetails>();

}
