/*using UnityEngine;
using System.Collections;

public class HarpsichordKey : Key
{
    public HarpsichordNote Note;

    private AudioSource audioSource;

    public float ReleaseFadeDuration = 0.15f;   //for sound fading out when releasing a key

    private Coroutine fadeCoroutine;

    private float TuningPitch = 1f; //for the tuning

    public override void Start()
    {
        RotationAxis = Axes.X_Axis;

        Angle_for_Released = 0F;
        Angle_for_Pressed = 2.5F;

        audioSource = GetComponent<AudioSource>();

        base.Start();

    }

    private Harpsichord getHarpsichord()     //helper function finds parent harpsichord  //public if mouseUI calls it
    {
        return GetComponentInParent<Harpsichord>();
    }
    private KeyData getData()        //helper function finds keydata of note
    {
        Harpsichord h = getHarpsichord();

        if (h == null)
            return null;

        return h.Database.GetKeyData(Note);
    }



    public override void press()
    {
        base.press();

        KeyData data = getData();

        if (data == null || data.Sound ==null)
            return;


        audioSource.clip = data.Sound;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        audioSource.volume = 1f;
        audioSource.clip = data.Sound;
        audioSource.pitch = TuningPitch; //for the tuning
        audioSource.Play();

      //  if (data.Sound != null)
       //     audioSource.PlayOneShot(data.Sound);
    }
    public void setTuningPitch(float pitch)  // for the tuning
    {
        TuningPitch = pitch;

        if (audioSource != null)
        {
            audioSource.pitch = TuningPitch;
        }
    }
    public override void release()
    {
        base.release();

        if (audioSource != null)
            FadeOutAndStop(ReleaseFadeDuration);
    }
   /* public override void release()
    {
        base.release();

        if (audioSource != null)
            audioSource.Stop();
    }  */

/*  public void release(float duration)                              //this one
  {
      StartCoroutine(releaseAfterDuration(duration));
  }

  public void startTuningSound()  //convenience method for smeantic clarity when called from the pin for  the tuning
  {
      press();
  }


  public void stopTuningSound()  //convenience method for smeantic clarity when called from the pin for  the tuning
  {
      release();
  }  



  private IEnumerator releaseAfterDuration(float duration)
  {
      yield return new WaitForSeconds(duration);

      release();
  }
  /*  public override void release()
    {
        base.release();
    }  */

/* public override string getTooltipName()
 {
     return MouseUI.AttributedName(Labels.HarpsichordKey)
   + " "
   + KeyName;
 }  */

/*private void FadeOutAndStop(float fadeDuration)                     //this one
{
    if (fadeCoroutine != null)
        StopCoroutine(fadeCoroutine);

    fadeCoroutine = StartCoroutine(fadeOutAndStop(fadeDuration));
}

private IEnumerator fadeOutAndStop(float fadeDuration)
{
    float startVolume = audioSource.volume;
    float timer = 0f;

    while (timer < fadeDuration)
    {
        timer += Time.deltaTime;
         float t = timer / fadeDuration;
        // audioSource.volume = startVolume * Mathf.Pow(1f - t, 2f);  //exponential damping
        audioSource.volume = startVolume * Mathf.Sqrt(1f - t);   //softer exponential damping
        //audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration); //linear damping
        yield return null;
    }

    audioSource.Stop();
    audioSource.volume = startVolume;
    fadeCoroutine = null;
}
public override string getTooltipName()
{
    KeyData data = getData();

    if (data == null)
        return "";

    return MouseUI.AttributedName(Labels.HarpsichordKey)
           + " "
           + data.DisplayName;
}
}  */

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


        if (h == null)
            return null;


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


        if (
            data == null ||
            data.Sound == null)
        {
            return;
        }


        audioSource.clip =
            data.Sound;


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


        if (harpsichord != null)
        {
            harpsichord.showKeyOnTuner(
                this
            );
        }
    }


    public void setTuningPitch(
        float pitch)
    {
        TuningPitch =
            pitch;


        if (audioSource != null)
        {
            audioSource.pitch =
                TuningPitch;
        }
    }

    public float getTuningPitch()
    {
        return TuningPitch;
    }

    public override void release()
    {
        base.release();


        if (audioSource != null)
        {
            FadeOutAndStop(
                ReleaseFadeDuration
            );
        }
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