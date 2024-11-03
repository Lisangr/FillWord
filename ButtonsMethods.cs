using UnityEngine;

public class ButtonsMethods : MonoBehaviour
{
    public GameObject objectToDisable;
    public GameObject objectToEnable;

    public void OnMenuButtonClick()
    {
        objectToDisable.SetActive(false);
        objectToEnable.SetActive(true);
    }
}
