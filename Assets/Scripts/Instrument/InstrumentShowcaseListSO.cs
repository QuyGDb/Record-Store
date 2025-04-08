using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "InstrumentShowcaseListSO_", menuName = "Scriptable Objects/Instrument Showcase List")]
public class InstrumentShowcaseListSO : ScriptableObject
{
    public List<InstrumentShowcaseSO> instrumentShowcaseList = new List<InstrumentShowcaseSO>();

}
