using System.Collections;
using System;
using UnityEngine;

public class DistanceCompare : IComparer
{
    public Transform compareTransform;

    public DistanceCompare(Transform compareTransform)
    {
        this.compareTransform = compareTransform;
    }
    public int Compare(object x, object y)
    {
        Collider xCollider = x as Collider;
        Collider yCollider = y as Collider;

        Vector3 xOffSet = xCollider.transform.position - compareTransform.position;
        float xDistance = xOffSet.sqrMagnitude;

        Vector3 yOffSet = yCollider.transform.position - compareTransform.position;
        float yDistance = yOffSet.sqrMagnitude;

        return xDistance.CompareTo(yDistance);
    }
}
