using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private GameObject parent;
    void Start()
    {
        List<GameObject> items = new List<GameObject>();
        TreeViewManager.Singleton.Init(items);
    }
    
    private int m_counter = 1;
    public void AddItem()
    {
        GameObject go = new GameObject();
        go.name = "GameObject " + m_counter;
        parent = go;
        m_counter++;
        TreeViewManager.Singleton.AddItem(go);
    }
    
    public void AddItemInChild()
    {
        if (parent)
        {
            GameObject go = new GameObject();
            go.name = "GameObject " + m_counter;
            m_counter++;
            TreeViewManager.Singleton.AddItemAsChild(parent.transform, go);
        }
        else
        {
            Debug.Log("No parent Found");
        }
    }

    public void DeleteItem()
    {
        if (parent)
        {
            TreeViewManager.Singleton.DeleteItem(parent);
        }
        else
        {
            Debug.Log("No Object Found");
        }
    }
}
