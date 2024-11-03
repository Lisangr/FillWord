/*using UnityEngine;
using YG;

public class PlatformChanger : MonoBehaviour
{
    public GameObject mobileCanvas;
    public GameObject computerCanvas;
    public GameObject mobilePlayCanvas;
    public GameObject computerPlayCanvas;
    private void Awake()
    {
        if (YandexGame.EnvironmentData.isDesktop)
        {
            computerCanvas.SetActive(true);
            mobileCanvas.SetActive(false);
        }
        else
        {
            computerCanvas.SetActive(false);
            mobileCanvas.SetActive(true);
        }
    }
    private void Start()
    {
        if (YandexGame.EnvironmentData.isDesktop)
        {
            computerPlayCanvas.SetActive(true);
            mobilePlayCanvas.SetActive(false);
        }
        else
        {
            computerPlayCanvas.SetActive(false);
            mobilePlayCanvas.SetActive(true);
        }
    }
}
*/