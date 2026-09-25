using UnityEngine;
using System.Threading.Tasks;

public class SnareDrumLug : InteractableObject
{
    public int LugNumber;

    public SnareDrumSlice ConnectedSlice;
    [HideInInspector]
    public CarrySnareDrumTuningKey AttachedTuningKey;

    [Header("Tuning Key Attachment")]

    private SnareDrum snareDrum;

    public override void Start()
    {
        base.Start();

        snareDrum =
            GetComponentInParent<SnareDrum>();
       
    }

    public override string getTooltipName()
    {
        return "SNARE LUG " + LugNumber;
    }


    public override async Task<Values_After_JointUse> use_with(
    GameObject _OtherObject)
    {
        Values_After_JointUse result =
            new Values_After_JointUse(false);

        CarrySnareDrumTuningKey tuningKey =
            _OtherObject.GetComponent<
                CarrySnareDrumTuningKey>();

        if (tuningKey == null)
            return result;

        if (AttachedTuningKey != null)
            return result;


        // Check the tuning order BEFORE
        // performing the attachment.
        if ( !snareDrum.isValidNextLug(LugNumber))
        {
            snareDrum.showWrongTuningMessage();

            snareDrum.resetTuningExercise();

            // Joint use failed.
            // MouseUI therefore keeps the key
            // in its normal carried state.
            return result;
        }


        attachTuningKey(tuningKey);

        result =
            new Values_After_JointUse(
                true,
                gameObject,
                true,
                null,
                true
            );

        return result;
    }

    public void attachTuningKey(
    CarrySnareDrumTuningKey tuningKey)
    { 
        AttachedTuningKey =
            tuningKey;

        tuningKey.attachToLug(this);

        Transform keyTransform =
            tuningKey.transform;

        keyTransform.SetParent(
            transform,
            false
        );

        keyTransform.localPosition =
            new Vector3(
                0.00452682655f,
                0.00173494313f,
                0.309938669f
            );

    
        keyTransform.localEulerAngles =new Vector3(
        358.970001f,
        178.200012f,
        3.50223541f
    );

        keyTransform.localScale =
            new Vector3(
                20f,
                20f,
                35f
            );
        RotateSnareDrumTuningKey rotateKey =tuningKey.GetComponent<RotateSnareDrumTuningKey>();
        
            rotateKey.prepareForLug();
        
    }
   
    public override void evacuate(
    GameObject _Object)
    {
        CarrySnareDrumTuningKey tuningKey =
            _Object.GetComponent<
                CarrySnareDrumTuningKey>();

        if (tuningKey == null)
            return;

        if (AttachedTuningKey != tuningKey)
            return;

        tuningKey.transform.SetParent(
            null,
            true
        );

        AttachedTuningKey =
            null;

        tuningKey.detachFromLug();

        MouseUI mouseUI =
            tuningKey.GetComponent<MouseUI>();

            mouseUI.Place =
                null;
        
    }

    public void changeTension(float amount)
    {
      
        // Check the tuning state BEFORE
        // changing the tension.
        bool wasCorrectlyTuned =
            ConnectedSlice.isCorrectlyTuned();


        // Change the membrane tension.
        ConnectedSlice.CurrentTension =
            Mathf.Clamp(
                ConnectedSlice.CurrentTension + amount,
                ConnectedSlice.MinimumTension,
                ConnectedSlice.MaximumTension
            );


        // Recalculate the pitch produced by
        // the new membrane tension.
        ConnectedSlice.updatePitch();


        // Check the tuning state AFTER
        // changing the tension.
        bool isCorrectlyTuned =
            ConnectedSlice.isCorrectlyTuned();


        // The pitch has just ENTERED
        // the acceptable tuning range.
        if (!wasCorrectlyTuned &&
            isCorrectlyTuned)
        {
                snareDrum.showTunedMessage();

                snareDrum.registerTunedLug(
                    LugNumber
                );
            
        }
    }
    void Update()
    {
        // No tuning key attached to this lug.
        if (AttachedTuningKey == null ||
        !snareDrum.View)
            return;

    
        if (Input.GetKeyDown(KeyCode.M))
        {
            ConnectedSlice.playSound();
            ConnectedSlice.startGlow();
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            ConnectedSlice.stopSound();
            ConnectedSlice.stopGlow();
        }
    }
}