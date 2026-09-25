using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    // Stores the messages that have already been shown once.
    private HashSet<GameObject> MessagesAlreadyShown =
        new HashSet<GameObject>();


    // Shows a message for a specific amount of time.
    // This message can be shown again later.
    public void showMessage(
        GameObject message,
        float duration)
    {
        if (message == null)
            return;

        StartCoroutine(
            showMessageForDuration(
                message,
                duration
            )
        );
    }


    // Shows a message only the first time this method
    // is called for that specific message.
    public void showMessageOnce(
        GameObject message,
        float duration)
    {
        if (message == null)
            return;

        // If this message was already shown, do nothing.
        if (MessagesAlreadyShown.Contains(message))
            return;

        // Remember that this message has now been shown.
        MessagesAlreadyShown.Add(message);

        StartCoroutine(
            showMessageForDuration(
                message,
                duration
            )
        );
    }


    // Activates the message, waits, and then hides it.
    private IEnumerator showMessageForDuration(
        GameObject message,
        float duration)
    {
        message.SetActive(true);

        yield return new WaitForSeconds(duration);

        message.SetActive(false);
    }


    // Shows a message without automatically hiding it.
    public void showMessage(GameObject message)
    {
        if (message == null)
            return;

        message.SetActive(true);
    }


    // Hides a message.
    public void hideMessage(GameObject message)
    {
        if (message == null)
            return;

        message.SetActive(false);
    }
}