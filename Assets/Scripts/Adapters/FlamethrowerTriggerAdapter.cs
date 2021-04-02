using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerTriggerAdapter : UnitTriggerAdapter
{
    [SerializeField] Transform anchor;
    BoxCollider boxColl;

    protected override void Awake()
    {
        boxColl = GetComponent<BoxCollider>();
    }

    public override void SetNewRange(float range)
    {
        boxColl.size = new Vector3(boxColl.size.x, boxColl.size.y, range);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, anchor.localPosition.z + range / 2f);
    }
}
