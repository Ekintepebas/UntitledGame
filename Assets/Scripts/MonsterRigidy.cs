using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterRigidy : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    public Vector3 movingSpeed;

    [SerializeField]
    private AudioClip bopClip;

    [SerializeField]
    private AudioClip borderBopClip;

    // Update is called once per frame
    void Start()
    {
        m_Rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    public void ReceiveForce(Vector2 forceVector)
    {         
        movingSpeed = forceVector;
        SoundFXManager.instance.PlaySoundFXClip(bopClip, transform, 1f);
        m_Rigidbody.AddForce(forceVector * 4,ForceMode2D.Force);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Something hit");

        Debug.Log(collision.ToString());
        SoundFXManager.instance.PlaySoundFXClip(borderBopClip, transform, 1f);
    }
}
