using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isClose = false;
    protected new string name;

    public virtual void OpenPanel(string name)
    {
        this.name = name;
        gameObject.SetActive(true);
    }

    public virtual void ClosePanel(string name)
    {   
        print(name+"  将被删除" );
        isClose = true;
        gameObject.SetActive(false);
        Destroy(gameObject);

        if(UIManager.Instance.panelDict.ContainsKey(name))
        {
            UIManager.Instance.panelDict.Remove(name);
        }
    }

}
