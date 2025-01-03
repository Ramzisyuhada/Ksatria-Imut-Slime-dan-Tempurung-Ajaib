using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{

    public enum Statet
    {
        Patrol,
        Run,
        Attack,
        Die
    }

    public Statet statet;
    private GameObject player;

    private NavMeshAgent agent;

    [SerializeField] private GameObject[] posisi;
    private Vector3 Dest;
    Animator animator;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();   
        agent.avoidancePriority = Random.Range(0, 100);
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        posisi = GameObject.FindGameObjectsWithTag("Spawn");
        FindNextPoint();
    }


    void FindNextPoint()
    {
        int randomIndex = Random.Range(0, posisi.Length);
        float Radius = 10f;
        Vector3 RndPos = Vector3.zero;
        Dest = posisi[randomIndex].transform.position + RndPos;
        if (CurrentPos(Dest))
        {
            RndPos = new Vector3(Random.Range(-Radius,Radius),0f , Random.Range(-Radius,Radius));
            Dest = posisi[randomIndex].transform.position + RndPos;
        }
    }

    bool CurrentPos(Vector3 pos)
    {
        float x =  Mathf.Abs(pos.x - transform.position.x);
        float z =  Mathf.Abs(pos.z - transform.position.z);

        if(x <= 50  && z <= 50) return true;
        return false;
    }
    private void Patroll()
    {

        if(Vector3.Distance(transform.position, Dest) < 15f)
        {

            FindNextPoint() ;
        }

        Debug.Log(Vector3.Distance(transform.position, player.transform.position) < 10);
        if (Vector3.Distance(transform.position, player.transform.position) < 10)
        {
            statet = Statet.Run;
            agent.speed = 5f;
            return;
        }

        agent.SetDestination(Dest);
        animator.SetFloat("Speed",agent.speed);
    }

    private void Run()
    {
        Debug.Log("Lari");
        Dest = player.transform.position;
        if (Vector3.Distance(transform.position, player.transform.position) > 10f)
        {
            statet = Statet.Patrol;
            agent.speed = 2F;
           // FindNextPoint();
            return;
        }
        
        if (Vector3.Distance(transform.position, player.transform.position) < 1)
        {
            statet = Statet.Attack;
            timeattack = 3.0f;
            
            return ;
        }
        agent.SetDestination(Dest);
        animator.SetFloat("Speed", agent.speed);
    }
    int currentattack;
    float timeattack = 3.0f;
    private void Attack()
    {
       //agent.SetDestination(Dest);
        if(Vector3.Distance(transform.position,player.transform.position) > 2) statet = Statet.Run;

        Vector3 dir = (player.transform.position - transform.position);
        Quaternion target = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * agent.angularSpeed);
        timeattack += Time.deltaTime;
        if (timeattack > 2)
        {

            currentattack++;
            if (timeattack > 3.0f) currentattack = 1;
            if(currentattack > 3) currentattack = 1;
            Debug.Log(currentattack);
            animator.SetTrigger("Attack" + currentattack);
            timeattack = 0f;
            
        }
    }
    void Update()
    {
        switch (statet)
        {
            case Statet.Patrol:
                Patroll(); break;
            case Statet.Run:
                Run();
                break;
            case Statet.Attack:
               Attack();
                break;
        }

        
       

    }
}
