using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject targetCir2D;
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private Transform shootPoint;
    //public static GameObject[] aiHuman;
    private GameObject ring;
    Vector3 targetPos;
    Vector3 Vnot;
    void Start()
    {
        Vnot = Vector3.zero;
        //aiHuman = GameObject.FindGameObjectsWithTag("AI");
    }

    public void TestMethod(BaseEventData eventData)
    {
        if(((PointerEventData)eventData).pointerCurrentRaycast.isValid)
        {
            targetPos = ((PointerEventData)eventData).pointerCurrentRaycast.worldPosition;
            if(ring == null)
                ring = Instantiate(targetCir2D, targetPos, Quaternion.identity);
            ring.transform.position = targetPos;
            Vnot = CalculateVelocity(targetPos, shootPoint.position, 1);
            //transform.rotation = Quaternion.LookRotation(targetPos);
            transform.rotation = Quaternion.LookRotation(Vnot);
            //transform.rotation = Quaternion.LookRotation(new Vector3(transform.rotation.x, targetPos.y, transform.rotation.z));
        }
    }
    public void Destroy()
    {
        Destroy(ring);
        LaunchBullet();
    }
    private void LaunchBullet()
    {
        Rigidbody obj = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        obj.velocity = Vnot;
    }
    private Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        //Define the distance x and y first;
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        //create a float that represent our distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        return result;
    }
}
