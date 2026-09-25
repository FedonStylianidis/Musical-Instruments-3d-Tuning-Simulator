
using UnityEngine;
using System.Collections;

public class HarpsichordKey : Key
{
    public HarpsichordNote Note;

    private AudioSource audioSource;

    public float ReleaseFadeDuration = 0.15f;

    private Coroutine fadeCoroutine;

    private float TuningPitch = 1f;


    public override void Start()
    {
        RotationAxis = Axes.X_Axis;

        Angle_for_Released = 0F;

        Angle_for_Pressed = 2.5F;


        audioSource =
            GetComponent<AudioSource>();


        base.Start();
    }


    private Harpsichord getHarpsichord()
    {
        return GetComponentInParent<Harpsichord>();
    }


    private KeyData getData()
    {
        Harpsichord h =
            getHarpsichord();

        return h.Database.GetKeyData(Note);
    }


    /*
    These six keys exist on the 60-key
    3D model but do not correspond to
    tuning pins in the 54-pin instrument.

    Left side:
        F1
        F#1

    Right side:
        C#6
        D6
        D#6
        E6
    */
    private bool isDummyKey()
    {
        return
            Note == HarpsichordNote.F1 ||
            Note == HarpsichordNote.FSharp1 ||
            Note == HarpsichordNote.CSharp6 ||
            Note == HarpsichordNote.D6 ||
            Note == HarpsichordNote.DSharp6 ||
            Note == HarpsichordNote.E6;
    }


    public override void press()
    {
        base.press();


        /*
        Dummy keys do not produce sound.
        */
        if (isDummyKey())
            return;


        KeyData data =
            getData();


        if (fadeCoroutine != null)
        {
            StopCoroutine(
                fadeCoroutine
            );

            fadeCoroutine =
                null;
        }


        audioSource.volume =
            1f;


        audioSource.clip =
            data.Sound;


        audioSource.pitch =
            TuningPitch;


        audioSource.Play();

        Harpsichord harpsichord =
    getHarpsichord();

            harpsichord.showKeyOnTuner(
                this
            );
        
    }


    public void setTuningPitch(
        float pitch)
    {
        TuningPitch =
            pitch;


            audioSource.pitch =
                TuningPitch;
        
    }

    public float getTuningPitch()
    {
        return TuningPitch;
    }

    public override void release()
    {
        base.release();


            FadeOutAndStop(
                ReleaseFadeDuration
            );
        
    }


    public void release(
        float duration)
    {
        StartCoroutine(
            releaseAfterDuration(
                duration
            )
        );
    }


    public void startTuningSound()
    {
        press();
    }


    public void stopTuningSound()
    {
        release();
    }


    private IEnumerator releaseAfterDuration(
        float duration)
    {
        yield return
            new WaitForSeconds(
                duration
            );


        release();
    }


    private void FadeOutAndStop(
        float fadeDuration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(
                fadeCoroutine
            );
        }


        fadeCoroutine =
            StartCoroutine(
                fadeOutAndStop(
                    fadeDuration
                )
            );
    }


    private IEnumerator fadeOutAndStop(
        float fadeDuration)
    {
        float startVolume =
            audioSource.volume;


        float timer =
            0f;


        while (timer < fadeDuration)
        {
            timer +=
                Time.deltaTime;


            float t =
                timer /
                fadeDuration;


            audioSource.volume =
                startVolume *
                Mathf.Sqrt(
                    1f - t
                );


            yield return null;
        }


        audioSource.Stop();


        audioSource.volume =
            startVolume;


        fadeCoroutine =
            null;
    }


    public override string getTooltipName()
    {
        /*
        Dummy keys receive their own
        language-dependent tooltip.
        */
        if (isDummyKey())
        {
            return MouseUI.AttributedName(
                Labels.HarpsichordDummyKey
            );
        }


        KeyData data =
            getData();


        if (data == null)
            return "";


        return
            MouseUI.AttributedName(
                Labels.HarpsichordKey
            )
            + " "
            + data.DisplayName;
    }
}