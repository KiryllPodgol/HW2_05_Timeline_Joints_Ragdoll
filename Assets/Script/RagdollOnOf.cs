using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class RagdollOnOf : MonoBehaviour
{
    public CapsuleCollider mainCollider;
    public GameObject CharacterRig;
    public Animator CharacterAnimator;
    public float respawnDelay = 3f;
    public float invulnerabilityTime = 2f; 
    private Collider[] RagdollColliders;
    private Rigidbody[] limbsRigidbodies;
    private bool isInvulnerable = false; 
    private void Start()
    {
        GetRagdollBits();
        SwitchRagDollMode(false);
    }
    private void GetRagdollBits()
    {
        RagdollColliders = CharacterRig.GetComponentsInChildren<Collider>();
        limbsRigidbodies = CharacterRig.GetComponentsInChildren<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isInvulnerable) return;

        if (collision.gameObject.TryGetComponent<DangerousObject>(out DangerousObject dangerous))
        {
            SwitchRagDollMode(true);
            StartCoroutine(RespawnCoroutine());
        }
    }
   private void SwitchRagDollMode(bool On)
   {
       CharacterAnimator.enabled = !On;
       foreach (Collider col in RagdollColliders)
       {
           col.enabled = On;
       }
       foreach (Rigidbody rb in limbsRigidbodies)
       {
           rb.isKinematic = !On;
       }
       mainCollider.enabled = !On;
       GetComponent<Rigidbody>().isKinematic = On;
   }
   
    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        SwitchRagDollMode(false);
        StartCoroutine(InvulnerabilityCoroutine());
        Debug.Log("Respawn completed!");
    }
    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);

        isInvulnerable = false; 
        Debug.Log("Invulnerability ended!");
    }
}
