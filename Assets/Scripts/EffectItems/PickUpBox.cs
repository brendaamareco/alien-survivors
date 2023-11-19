using System.Collections;
using UnityEngine;

public class PickUpBox : MonoBehaviour, IEntity
{
    private Animator m_Animator;
    private bool isOpening = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!isOpening && other.CompareTag("Ammunition"))
        {
            m_Animator = GetComponent<Animator>();
            m_Animator.SetBool("isOpen", true);
            StartCoroutine(OpenBoxAfterAnimation());
        }
    }

    private IEnumerator OpenBoxAfterAnimation()
    {
        isOpening = true;
        yield return new WaitForSeconds(2f);
        GameEventManager.GetInstance().Publish(GameEvent.BOX_OPENED, new EventContext(this));
        isOpening = false;
    }

    public string GetName()
    { return typeof(PickUpBox).Name; }
}