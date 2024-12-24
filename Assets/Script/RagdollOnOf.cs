using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
public class RagdollOnOf : MonoBehaviour
{
    public CapsuleCollider mainCollider;
    public GameObject CharacterRig;
    public Animator CharacterAnimator;
    public PlayableDirector deathSceneDirector;
    public Transform deathCam;
    public float invulnerabilityTime = 2f;

    private Collider[] RagdollColliders;
    private Rigidbody[] limbsRigidbodies;
    private bool isInvulnerable = false;
    private Vector3 deathPosition; 
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
            deathPosition = transform.position;
            SwitchRagDollMode(true);
            StartCoroutine(DeathSequenceCoroutine());
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
    private IEnumerator DeathSequenceCoroutine()
    {
        if (deathCam != null)
        {
            deathCam.position = deathPosition + new Vector3(0, 2, -3); 
            deathCam.rotation = Quaternion.LookRotation(deathPosition - deathCam.position);
            StartCoroutine(RotateCameraAroundDeathPosition());
        }
        else
        {
            Debug.LogWarning("DeathCam is not assigned!");
        }
        if (deathSceneDirector != null)
        {
            Debug.Log("Starting death timeline...");
            deathSceneDirector.Play();
            yield return new WaitForSeconds((float)deathSceneDirector.duration);
        }
        else
        {
            Debug.LogWarning("DeathSceneDirector is not assigned!");
        }
        SwitchRagDollMode(false);
        StartCoroutine(InvulnerabilityCoroutine());
    }
    private IEnumerator RotateCameraAroundDeathPosition()
    {
        float rotationSpeed = 45f;
        float totalRotation = 0f;
        while (totalRotation < 360f)
        {
            float step = rotationSpeed * Time.deltaTime;
            totalRotation += step;
            deathCam.RotateAround(deathPosition, Vector3.up, step);
            deathCam.LookAt(deathPosition);
            yield return null;
        }
        deathCam.LookAt(deathPosition);
    }
    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
        Debug.Log("Invulnerability ended!");
    }
}