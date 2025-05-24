using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Instrument : MonoBehaviour
{
    [HideInInspector] public Transform currentTranforms;
    public InstrumentSO instrumentSO;
}
