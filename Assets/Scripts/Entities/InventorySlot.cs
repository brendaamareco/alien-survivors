using System;
using UnityEngine;

public class InventorySlot<T> where T : MonoBehaviour
{
    private Transform location;
    private T element;

    public InventorySlot(Transform location)
    {
        this.location = location;
    }

    public bool IsFree()
    { return element == null; }

    public T GetElement()
    { return element; }

    public void SetElement(T element)
    {
        this.element = element;
        //element.transform.parent = location;
        element.transform.localPosition = Vector3.zero;
        element.transform.localRotation = Quaternion.identity;
    }

    public Transform GetLocation()
    { return this.location; }
}
