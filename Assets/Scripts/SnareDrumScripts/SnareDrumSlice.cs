/*using UnityEngine;

public class SnareDrumSlice : InteractableObject
{
    public int SliceNumber;
    public AudioClip Sound;
    private AudioSource audioSource;
    private SnareDrum snareDrum;

    public float TargetTension = 1f;
    public float CurrentTension = 1f;

    public float MinimumTension = 0.7f;
    public float MaximumTension = 1.3f;

    public override void Start()
    {
        base.Start();

        snareDrum =
            GetComponentInParent<SnareDrum>();

        audioSource =GetComponent<AudioSource>();
    }

    public override void press()
    {
        if (audioSource == null ||
            Sound == null)
        {
            return;
        }

        audioSource.clip = Sound;
        audioSource.pitch = 1f;
        audioSource.Play();
    }

    public void updatePitch()
    {
        if (audioSource == null)
            return;

        float tensionRatio =
            CurrentTension / TargetTension;

        float pitchRatio =
            Mathf.Sqrt(tensionRatio);

        audioSource.pitch =
            pitchRatio;
    }
    public override string getTooltipName()
    {
        return "SNARE MEMBRANE PART " + SliceNumber;
    }
}  */

using UnityEngine;
using System.Collections;

public class SnareDrumSlice
    : InteractableObject
{
    public int SliceNumber;

    public AudioClip Sound;


    [Header("Membrane Tension")]

    // T0:
    // Normalized target tension.
    // T0 = 1 means correctly tuned.
    public float TargetTension = 1f;


    // T:
    // Current normalized membrane tension.
    //
    // T < 1 : below target tension
    // T = 1 : target tension
    // T > 1 : above target tension
    public float CurrentTension = 1f;


    public float MinimumTension = 0.7f;

    public float MaximumTension = 1.3f;


    [Header("Audio")]

    // Time needed for the sound to fade
    // completely after M is released.
    public float ReleaseFadeDuration = 0.15f;


    private SnareDrum snareDrum;

    private AudioSource audioSource;

    private Coroutine fadeCoroutine;

    [Header("Tuning Accuracy")]
    public float TuningToleranceCents = 5f;

    [Header("Glow")]
    public GameObject GlowVisual;

    public override void Start()
    {
        base.Start();


        snareDrum =
            GetComponentInParent<SnareDrum>();


        audioSource =
            GetComponent<AudioSource>();


        updatePitch();

        if (GlowVisual != null)
        {
            GlowVisual.SetActive(false);
        }
    }


    public override string getTooltipName()
    {
        return "SNARE MEMBRANE PART " +
            SliceNumber;
    }


    public override void press()
    {
        if (audioSource == null ||
            Sound == null)
        {
            return;
        }

        // Set the pitch according to the
        // current membrane tension.
        updatePitch();

        audioSource.volume = 1f;
        audioSource.loop = false;
        audioSource.clip = Sound;

        // Left click plays the membrane
        // normally from beginning to end.
        audioSource.Play();

        StartCoroutine(
        glowForOneSecond()
    );
    }

    public void playSound()
    {
        if (audioSource == null ||
            Sound == null)
        {
            return;
        }


        // If the previous sound is still
        // fading out, cancel that fade.
        if (fadeCoroutine != null)
        {
            StopCoroutine(
                fadeCoroutine
            );

            fadeCoroutine = null;
        }


        // Restore full volume.
        audioSource.volume = 1f;


        // Calculate the correct pitch from
        // the current membrane tension.
        updatePitch();


        audioSource.clip =
            Sound;


        // Keep the sound playing while
        // M is held.
        audioSource.loop = true;


        audioSource.Play();
    }


    public void stopSound()
    {
        if (audioSource == null)
            return;


        // Stop any previous fade before
        // starting a new one.
        if (fadeCoroutine != null)
        {
            StopCoroutine(
                fadeCoroutine
            );
        }


        fadeCoroutine =
            StartCoroutine(
                FadeOutAndStop(
                    ReleaseFadeDuration
                )
            );
    }


    private IEnumerator FadeOutAndStop(
        float duration)
    {
        float startVolume =
            audioSource.volume;

        float elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            elapsedTime +=
                Time.deltaTime;


            float t =
                elapsedTime /
                duration;


            // Square-root fade.
            //
            // This gives a smoother perceived
            // release than a simple linear fade.
            audioSource.volume =
                startVolume *
                Mathf.Sqrt(
                    1f - Mathf.Clamp01(t)
                );


            yield return null;
        }


        // The fade has finished.
        audioSource.volume = 0f;

        audioSource.loop = false;

        audioSource.Stop();


        // Restore volume so that the next
        // playSound() starts normally.
        audioSource.volume = 1f;


        fadeCoroutine = null;
    }


    public void updatePitch()
    {
        if (audioSource == null)
            return;


        // Circular membrane physics:
        //
        //      f        T
        //     ---- = √ ----
        //      f0       T0
        //
        // T  = current membrane tension
        // T0 = target membrane tension

        float tensionRatio =
            CurrentTension /
            TargetTension;


        float pitchRatio =
            Mathf.Sqrt(
                tensionRatio
            );


        // Unity pitch is a frequency ratio.
        audioSource.pitch =
            pitchRatio;
    }

    public float getCentsFromTarget()
    {
        float tensionRatio =
            CurrentTension /
            TargetTension;

        float pitchRatio =
            Mathf.Sqrt(
                tensionRatio
            );

        float cents =
            1200f *
            Mathf.Log(
                pitchRatio,
                2f
            );

        return cents;
    }


    public bool isCorrectlyTuned()
    {
        float cents =
            getCentsFromTarget();

        return Mathf.Abs(cents) <=
            TuningToleranceCents;
    }

    public void startGlow()
    {
        if (GlowVisual != null)
        {
            GlowVisual.SetActive(true);
        }
    }


    public void stopGlow()
    {
        if (GlowVisual != null)
        {
            GlowVisual.SetActive(false);
        }
    }

    private IEnumerator glowForOneSecond()
    {
        startGlow();

        yield return new WaitForSeconds(0.1f);

        stopGlow();
    }
}