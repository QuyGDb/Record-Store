using System;
using TMPro;
using UnityEngine;

public class AttachObjectDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    [SerializeField] private MusicObjectListSO instrumentShowCaseVN;
    [SerializeField] private MusicObjectListSO instrumentShowCaseOversea;
    [SerializeField] private MusicObjectListSO musicGenres;
    [SerializeField] private MusicObjectListSO posters;
    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
    private void OnDropdownValueChanged(int objects)
    {
        switch (dropdown.value)
        {
            case 0:
                StaticEventHandler.InvokeObjectMusicListChanged(instrumentShowCaseVN);
                break;
            case 1:
                StaticEventHandler.InvokeObjectMusicListChanged(instrumentShowCaseOversea);
                break;
            case 2:
                StaticEventHandler.InvokeObjectMusicListChanged(musicGenres);
                break;
            case 3:
                StaticEventHandler.InvokeObjectMusicListChanged(posters);
                break;
        }
    }
}
