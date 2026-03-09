using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesItemsHandler : MonoBehaviour
{
    private Box[] _boxes;

    private void Awake()
    {
        _boxes = GetComponentsInChildren<Box>();
    }
}
