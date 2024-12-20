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
        RagdollModeOff();
    }
    private void GetRagdollBits()
    {
        RagdollColliders = CharacterRig.GetComponentsInChildren<Collider>();
        limbsRigidbodies = CharacterRig.GetComponentsInChildren<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isInvulnerable) return;
        if (collision.gameObject.name == "DangerCube")
        {
            RagdollModeOn();
            StartCoroutine(RespawnCoroutine());
        }
    }
    private void RagdollModeOn()
    {
        Debug.Log("RagdollModeOn activated!");
        CharacterAnimator.enabled = false;
        foreach (Collider col in RagdollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rb in limbsRigidbodies)
        {
            rb.isKinematic = false;
        }
        mainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void RagdollModeOff()
    {
        Debug.Log("RagdollModeOff activated!");
        foreach (Collider col in RagdollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rb in limbsRigidbodies)
        {
            rb.isKinematic = true;
        }
        CharacterAnimator.enabled = true;
        mainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private IEnumerator RespawnCoroutine()
    {
        Debug.Log("Respawn started...");
        yield return new WaitForSeconds(respawnDelay);
        RagdollModeOff();
        StartCoroutine(InvulnerabilityCoroutine());
        Debug.Log("Respawn completed!");
    }
    private IEnumerator InvulnerabilityCoroutine()
    {
        Debug.Log("Invulnerability started!");
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);

        isInvulnerable = false; 
        Debug.Log("Invulnerability ended!");
    }
}
