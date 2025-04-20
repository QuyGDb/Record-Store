using System;
using UnityEngine;

public class LogoGenres : MonoBehaviour
{
    private PictureFrameManager pictureFrameManager;
    private GameObject pop;
    private GameObject rap;
    private GameObject rock;
    private void Awake()
    {
        pictureFrameManager = GetComponent<PictureFrameManager>();
    }
    void Start()
    {
        ApplicationManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
        CreateLogoForGenres();
    }

    private void OnDestroy()
    {
        ApplicationManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.ObjectParent)
        {
            pop.SetActive(true);
            rap.SetActive(true);
            rock.SetActive(true);
        }
    }

    void CreateLogoForGenres()
    {
        pop = Instantiate(GameResources.Instance.pop.logo, pictureFrameManager.popGenreOnWall);
        pop.gameObject.SetActive(false);
        rap = Instantiate(GameResources.Instance.rap.logo, pictureFrameManager.rapGenreOnWall);
        rap.gameObject.SetActive(false);
        rock = Instantiate(GameResources.Instance.rock.logo, pictureFrameManager.rockGenreOnWall);
        rock.gameObject.SetActive(false);
    }

}
