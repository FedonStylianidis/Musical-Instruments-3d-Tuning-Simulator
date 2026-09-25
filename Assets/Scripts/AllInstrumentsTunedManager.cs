using UnityEngine;
using System.Collections;

public class AllInstrumentsTunedManager : MonoBehaviour
{
    public GameObject AllInstrumentsTunedMessage;
    public MessageManager MessageManager;

    public float DemoDelay = 2f;

    private bool HarpsichordTuned;
    private bool RecorderTuned;
    private bool SnareDrumTuned;

    private bool FinaleStarted;

    private EgoController Ego;

    private Harpsichord Harpsichord;
    private Recorder Recorder;
    private SnareDrum SnareDrum;

    void Start()
    {
        AllInstrumentsTunedMessage.SetActive(false);

        Ego = GameObject.Find("Ego")
            .GetComponent<EgoController>();
        Harpsichord =
    FindFirstObjectByType<Harpsichord>();

        Recorder =
            FindFirstObjectByType<Recorder>();

        SnareDrum =
            FindFirstObjectByType<SnareDrum>();

    }


    void Update()
    {
        if (!FinaleStarted &&
            HarpsichordTuned &&
            RecorderTuned &&
            SnareDrumTuned &&
            Ego.Mode == Modes.Navigation)
        {
            FinaleStarted = true;

            MessageManager.showMessageOnce(
                AllInstrumentsTunedMessage,
                10f
            );

            StartCoroutine(playFinale());
        }
    }


    private IEnumerator playFinale()
    {
        yield return new WaitForSeconds(
            DemoDelay
        );

        SnareDrum.PlaySnareDemo();

        yield return new WaitForSeconds(
            1.2f
        );

        Harpsichord.PlayToccataDemo();

        Recorder.PlayRecorderDemo();
    }

    public void harpsichordCompleted()
    {
        HarpsichordTuned = true;
    }


    public void recorderCompleted()
    {
        RecorderTuned = true;
    }


    public void snareDrumCompleted()
    {
        SnareDrumTuned = true;
    }
}