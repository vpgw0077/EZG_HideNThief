using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePooling : MonoBehaviour
{
    public static ParticlePooling instance;

   [SerializeField] private ParticleSystem Smoke_Effect = null;
   [SerializeField] private ParticleSystem Flash_Effect = null;

    private Queue<ParticleSystem> S_queue = new Queue<ParticleSystem>();
    private Queue<ParticleSystem> F_queue = new Queue<ParticleSystem>();

    private int createCount = 10;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        for (int i = 0; i < createCount; i++)
        {

            ParticleSystem s_Effect = Instantiate(Smoke_Effect, Vector3.zero, Quaternion.identity);
            ParticleSystem f_Effect = Instantiate(Flash_Effect, Vector3.zero, Quaternion.identity);
            S_queue.Enqueue(s_Effect);
            F_queue.Enqueue(f_Effect);
            s_Effect.gameObject.SetActive(false);
            f_Effect.gameObject.SetActive(false);
        }

    }
    public void InsertQueue(ParticleSystem s_Effect)
    {
        S_queue.Enqueue(s_Effect);
        s_Effect.gameObject.SetActive(false);
    }
    public void InsertF_Queue(ParticleSystem f_Effect)
    {
        F_queue.Enqueue(f_Effect);
        f_Effect.gameObject.SetActive(false);
    }

    public ParticleSystem GetQueue()
    {
        ParticleSystem s_Effect = S_queue.Dequeue();
        s_Effect.gameObject.SetActive(true);
        return s_Effect;
    }
    public ParticleSystem GetF_Queue()
    {
        ParticleSystem f_Effect = F_queue.Dequeue();
        f_Effect.gameObject.SetActive(true);
        return f_Effect;
    }

}
