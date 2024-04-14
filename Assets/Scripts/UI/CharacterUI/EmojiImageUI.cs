using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojiImageUI : MonoBehaviour
{
    private Image image;

    private void Start()
    {
    }

    public void Refresh(string ImagePath)
    {
        Texture2D t = (Texture2D)Resources.Load("Image/Emoji/" + ImagePath);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        GetComponent<Image>().sprite = temp;
    }
}
