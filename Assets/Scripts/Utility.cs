using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static List<Transform> GetFirstLevelChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            children.Add(parent.GetChild(i));
        }

        return children;
    }
}
