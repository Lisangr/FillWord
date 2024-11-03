using UnityEngine;
using UnityEngine.UI;

public class LetterCell : MonoBehaviour
{
    public Image letterImage;
    public GameObject gameObject;    
    public void SetLetterImage(Sprite sprite)
    {
        if (sprite != null)
        {
            letterImage.sprite = sprite;
        }
    }
    public void OnClickOffBorders() => gameObject.SetActive(false);
    public void OnCellClicked()
    {
        if (!gameObject.activeSelf) return; // »гнорируем клики по неактивным €чейкам

        FindObjectOfType<GridSpawner>().RegisterCellClick(this);
    }
}