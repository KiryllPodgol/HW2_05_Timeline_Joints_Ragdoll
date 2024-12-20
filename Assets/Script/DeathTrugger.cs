using UnityEngine;
using UnityEngine.Playables;

public class DeathTrigger : MonoBehaviour
{
    public GameObject player;
    public PlayableDirector deathCutscene;

    private Animator animator; 
    private Rigidbody mainRigidbody; 
    private Rigidbody[] ragdollBodies; 
    private Collider[] ragdollColliders; 
    private Collider playerCollider; 
    private bool isDead = false; // Флаг для отслеживания состояния смерти

    private void Start()
    {
        animator = player.GetComponent<Animator>();
        mainRigidbody = player.GetComponent<Rigidbody>();
        playerCollider = player.GetComponent<Collider>();

        ragdollBodies = player.GetComponentsInChildren<Rigidbody>();
        ragdollColliders = player.GetComponentsInChildren<Collider>();

        DisableRagdoll(); // Отключаем Ragdoll на старте
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, столкнулся ли объект с коллайдером персонажа
        if (!isDead && collision.collider == playerCollider)
        {
            ActivateDeathSequence(); // Активируем процесс смерти
        }
    }

    private void DisableRagdoll()
    {
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = true; // Отключаем физику
        }

        foreach (var col in ragdollColliders)
        {
            col.enabled = false; // Отключаем коллайдеры
        }

        if (animator != null)
        {
            animator.enabled = true; // Включаем анимацию
        }
    }

    private void ActivateRagdoll()
    {
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = false; // Включаем физику
        }

        foreach (var col in ragdollColliders)
        {
            col.enabled = true; // Включаем коллайдеры
        }

        if (animator != null)
        {
            animator.enabled = false; // Отключаем анимацию
        }
    }

    private void ActivateDeathSequence()
    {
        if (isDead) return; // Если персонаж уже мертв, не повторять

        isDead = true; // Помечаем, что персонаж умер

        // Отключаем физику главного Rigidbody персонажа
        if (mainRigidbody != null)
        {
            mainRigidbody.isKinematic = true;
        }

        // Активируем Ragdoll
        ActivateRagdoll();

        // Запускаем катсцену смерти
        if (deathCutscene != null)
        {
            deathCutscene.Play();
        }

        Debug.Log("Персонаж умер, катсцена началась!");
    }
}
