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

    private bool FinaleStarted = false;

    private EgoController Ego;


    void Start()
    {
        AllInstrumentsTunedMessage.SetActive(false);

        Ego = GameObject.Find("Ego")
            .GetComponent<EgoController>();


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
        FindFirstObjectByType<SnareDrum>()
           .PlaySnareDemo();
        yield return new WaitForSeconds(
           1.2f
       );

        FindFirstObjectByType<Harpsichord>()
            .PlayToccataDemo();

        FindFirstObjectByType<Recorder>()
            .PlayRecorderDemo();

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