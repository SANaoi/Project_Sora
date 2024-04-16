using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionUI : MonoBehaviour
{
    private Transform UIEmoji;
    public GameObject EmojiPrefab;

    private void Awake()
    {
        InitUI();
        InitEmojiContent();
    }

    private void InitUI()
    {
        InitUIName();
    }

    public void InitUIName()
    {
        UIEmoji = transform.Find("Emoji");
    }

    private void InitEmojiContent()
    {
        for (int i = 0; i < UIEmoji.childCount; i++)
        {
            Destroy(UIEmoji.GetChild(i).gameObject);
        }
    }

    public void Refresh(List<string> ImagePaths)
    {
        if (ImagePaths == null) return;
        if (UIEmoji.childCount != 0)
        {
            for (int i = 0; i < UIEmoji.childCount; i++)
            {
                Destroy(UIEmoji.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < ImagePaths.Count; i++)
        {
            Transform EmojiImage = Instantiate(EmojiPrefab, UIEmoji.transform).transform;
            EmojiImage.GetComponent<EmojiImageUI>().Refresh(ImagePaths[i]);
        }
    }   
}
