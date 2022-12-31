using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AkibTreeView.UIControls;


/// <summary>
/// In this Manager we can use any hierarchical data with treeview.
/// </summary>
public class TreeViewManager : MonoBehaviour
{
    [SerializeField] private VirtualizingTreeView TreeView;

    #region Singleton

    private static TreeViewManager _instance;

    public static TreeViewManager Singleton
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TreeViewManager>();
                if (_instance == null)
                {
                    GameObject TreeView = new GameObject();
                    TreeView.name = typeof(TreeViewManager).Name;
                    _instance = TreeView.AddComponent<TreeViewManager>();
                    //DontDestroyOnLoad(pointManager);
                }
            }
            return _instance;
        }
    }
    public static void SetNewSingleTon(TreeViewManager treeViewManager)
    {
        if (_instance != null) return;
        _instance = treeViewManager;
    }

    #endregion Singleton

    public void Init(List<GameObject> items)
    {
        TreeView.ItemDataBinding += OnItemDataBinding;
        TreeView.ItemsRemoved += OnItemsRemoved;
        TreeView.ItemExpanding += OnItemExpanding;
        TreeView.Items = items;
    }

    private void OnDestroy()
    {
        TreeView.ItemDataBinding -= OnItemDataBinding;
        TreeView.ItemsRemoved -= OnItemsRemoved;
        TreeView.ItemExpanding -= OnItemExpanding;
    }

    private void OnItemExpanding(object sender, VirtualizingItemExpandingArgs e)
    {
        //get parent data item (game object in our case)
        GameObject gameObject = (GameObject) e.Item;
        if (gameObject.transform.childCount > 0)
        {
            //get children
            List<GameObject> children = new List<GameObject>();

            for (int i = 0; i < gameObject.transform.childCount; ++i)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;

                children.Add(child);
            }

            //Populate children collection
            e.Children = children;
        }
    }


    private void OnItemsRemoved(object sender, ItemsRemovedArgs e)
    {
        //Destroy removed dataitems
        for (int i = 0; i < e.Items.Length; ++i)
        {
            GameObject go = (GameObject) e.Items[i];
            if (go != null)
            {
                Destroy(go);
            }
        }
    }

    /// <summary>
    /// This method called for each data item during databinding operation
    /// You have to bind data item properties to ui elements in order to display them.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnItemDataBinding(object sender, VirtualizingTreeViewItemDataBindingArgs e)
    {
        GameObject dataItem = e.Item as GameObject;
        if (dataItem != null)
        {
            //We display dataItem.name using UI.Text 
            Text text = e.ItemPresenter.GetComponentInChildren<Text>(true);
            text.text = dataItem.name;

            //Load icon from resources
            Image icon = e.ItemPresenter.GetComponentsInChildren<Image>()[6];
            icon.sprite = Resources.Load<Sprite>("cube");

            //And specify whether data item has children (to display expander arrow if needed)

            e.HasChildren = dataItem.transform.childCount > 0;
        }
    }
    public void AddItem(GameObject item)
    {
        TreeView.Add(item);
    }

    public void AddItemAsChild(Transform parent, GameObject child)
    {
        child.transform.SetParent(parent);
        TreeView.AddChild(parent.gameObject, child);
    }

    public void DeleteItem(GameObject item)
    {
        TreeView.Remove(item);
    }
}