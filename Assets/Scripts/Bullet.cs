using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CameraShake.instance.Shake(0.5f, 0.24f);
            Destroy(this.gameObject);
        }
    }
}
