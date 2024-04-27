using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpressionUI : MonoBehaviour
{
    private Transform UIEmoji;
    private Transform Distance;
    public GameObject EmojiPrefab;

    public List<string> currentPath;

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
        Distance = transform.Find("Distance"); 
    }

    private void InitEmojiContent()
    {
        for (int i = 0; i < UIEmoji.childCount; i++)
        {
            Destroy(UIEmoji.GetChild(i).gameObject);
        }
    }

    public void Refresh(List<string> ImagePaths, float distance = 0)
    {
        
        if (distance > 3)
            Distance.GetComponent<Text>().text = distance.ToString() + " m";
        else
        {
            Distance.GetComponent<Text>().text = "";
        }
        
        if (ImagePaths == null || ImagePaths == currentPath) return;

        currentPath = ImagePaths;

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
