using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class AIHuman : MonoBehaviour
{
    private float checkRadius = 6f;
    public SkinnedMeshRenderer skinnedMeshRenderers;
    public MeshRenderer meshRenderer;
    public Material zombieMat;
    public LayerMask checkLayer;
    public Vector3 moveSpot;
    public float distance;
    public float remaininDistance;
    private NavMeshAgent navMeshAgent;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    private Animator aiAnim;
    public bool notActive;
    private bool notDied;
    public Transform targetTrans;
    // Start is called before the first frame update
    void Start()
    {
        notActive = notDied = true;
        moveSpot = new Vector3(UnityEngine.Random.Range(minX, maxX), 0f, UnityEngine.Random.Range(minY, maxY));
        navMeshAgent = GetComponent<NavMeshAgent>();
        aiAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
        if(notActive)
        {
            SetRandomPositionAndFollow();
        }
        else
        {
            FollowNearestAI();
        }
    }
    private void FollowNearestAI()
    {
        if(targetTrans != null)
        {
            AIHuman ai = targetTrans.GetComponent<AIHuman>();
            if(ai.notActive)
            {
                navMeshAgent.SetDestination(targetTrans.position);
            }
            else
            {
                FindClosestAI();
            }
        }
        else
        {
            SetRandomPositionAndFollow();
            FindClosestAI();
        }
    }
    private void SetRandomPositionAndFollow()
    {
        navMeshAgent.SetDestination(moveSpot);
        remaininDistance = navMeshAgent.remainingDistance;
        if( remaininDistance < 0.5f)
        {
            moveSpot = RandomPosition();
        }
    }
    private Vector3 RandomPosition()
    {
        return new Vector3(UnityEngine.Random.Range(minX, maxX), 0, UnityEngine.Random.Range(minY, maxY));
    }
    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.tag == "Bullet" || other.transform.tag == "Zombie") && notActive)
        {
            transform.tag = "Zombie";
            ChangeMaterial();
            notActive = false;
            navMeshAgent.speed += 2.5f;
            aiAnim.SetBool("Run", true);
            this.gameObject.AddComponent<Rigidbody>();
            StartCoroutine(ActiveAI());
            FindClosestAI();
            GameManager gManager = FindObjectOfType<GameManager>();
            if(gManager != null)
                gManager.UpdateScore();
        }
        else if(other.transform == targetTrans)
        {
            FindClosestAI();
        }
    }
    private IEnumerator ActiveAI()
    {
        yield return new WaitForSeconds(3f);
        navMeshAgent.isStopped = true;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.tag = "AI";
        notDied = false;
        aiAnim.SetBool("Die", true);
        Destroy(this.gameObject.GetComponent<NavMeshAgent>());
        Destroy(this.gameObject.GetComponent<AIHuman>());
        GameManager gManager = FindObjectOfType<GameManager>();
        if(gManager != null)
            gManager.RemoveAnAI(this.transform);
        //yield return new WaitForSeconds(3f);
        //Destroy(this.gameObject);

    }
    private void OnDrawGizmos()
    {
        if(!notActive)
        {
            Gizmos.DrawWireSphere(transform.position, checkRadius);
        }
    }
    private void FindClosestAI()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius, checkLayer);
        Array.Sort(colliders, new DistanceCompare(transform));
        if(colliders.Length != 0)
        {
            targetTrans = colliders[0].transform;
            navMeshAgent.SetDestination(targetTrans.position);
        }
        else
            targetTrans = null;
    }
    private void ChangeMaterial()
    {
        skinnedMeshRenderers.material = zombieMat;
        meshRenderer.material = zombieMat;
    }
}
