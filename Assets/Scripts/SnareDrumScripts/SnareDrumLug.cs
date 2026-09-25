using UnityEngine;
using System.Threading.Tasks;

public class SnareDrumLug : InteractableObject
{
    public int LugNumber;

    public SnareDrumSlice ConnectedSlice;
    [HideInInspector]
    public CarrySnareDrumTuningKey AttachedTuningKey;

    [Header("Tuning Key Attachment")]

    public Vector3 TuningKeyPositionOffset =
    Vector3.zero;
    private SnareDrum snareDrum;

    public override void Start()
    {
        base.Start();

        snareDrum =
            GetComponentInParent<SnareDrum>();
        AttachedTuningKey = null;
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
        if (snareDrum != null &&
            !snareDrum.isValidNextLug(LugNumber))
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
        if (tuningKey == null)
            return;

        if (AttachedTuningKey != null)
            return;


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

        /*keyTransform.localRotation =
            new Quaternion(
                0.524780512f,
                0.850618303f,
                -0.0259954669f,
                -0.0194483418f
            );*/
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

        if (rotateKey != null)
        {
            rotateKey.prepareForLug();
        }
    }
    /*public void attachTuningKey(
    CarrySnareDrumTuningKey tuningKey)
    {
        if (tuningKey == null)
            return;

        if (AttachedTuningKey != null)
            return;

        AttachedTuningKey =
            tuningKey;

        tuningKey.attachToLug(this);

        Transform keyTransform =
            tuningKey.transform;

        keyTransform.SetParent(
            null,
            true
        );

        keyTransform.position =
            transform.position +
            TuningKeyPositionOffset;

        keyTransform.SetParent(
            transform,
            true
        );
    }*/

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

        if (mouseUI != null)
        {
            mouseUI.Place =
                null;
        }
    }

    /*public void changeTension(float amount)
    {
        if (ConnectedSlice == null)
            return;
        
        ConnectedSlice.CurrentTension =
            Mathf.Clamp(
                ConnectedSlice.CurrentTension + amount,
                ConnectedSlice.MinimumTension,
                ConnectedSlice.MaximumTension
            );

        ConnectedSlice.updatePitch();
    }*/
    /* public void changeTension(float amount)
     {
         if (ConnectedSlice == null)
             return;


         // Remember whether the slice was correctly
         // tuned before this tension change.
         bool wasCorrectlyTuned =
             ConnectedSlice.isCorrectlyTuned();

         Debug.Log(
     "Slice " +
     ConnectedSlice.SliceNumber +
     " | cents = " +
     ConnectedSlice.getCentsFromTarget() +
     " | correct = " +
    ConnectedSlice. isCorrectlyTuned()
 );

         ConnectedSlice.CurrentTension =
             Mathf.Clamp(
                 ConnectedSlice.CurrentTension + amount,
                 ConnectedSlice.MinimumTension,
                 ConnectedSlice.MaximumTension
             );


         ConnectedSlice.updatePitch();


         // Check whether this rotation has moved
         // the slice into the correct tuning range.
         bool isCorrectlyTuned =
             ConnectedSlice.isCorrectlyTuned();


         // Show the message only when the slice
         // ENTERS the correct tuning range.
         //
         // This prevents the message from being
         // triggered continuously while the player
         // remains inside the tolerance.
         if (!wasCorrectlyTuned &&
             isCorrectlyTuned)
         {
             Debug.Log(
        "Lug " +
        LugNumber +
        " ENTERED tuning range"
    );
             SnareDrum snareDrum =
                 GetComponentInParent<SnareDrum>();

             if (snareDrum != null)
             {
                 snareDrum.showTunedMessage();

                 snareDrum.registerTunedLug(LugNumber);
             }
         }
     } */

    public void changeTension(float amount)
    {
        if (ConnectedSlice == null)
            return;


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
            if (snareDrum != null)
            {
                snareDrum.showTunedMessage();

                snareDrum.registerTunedLug(
                    LugNumber
                );
            }
        }
    }
    void Update()
    {
        // No tuning key attached to this lug.
        if (AttachedTuningKey == null ||
        !snareDrum.View)
            return;

        // No membrane section connected to this lug.
        if (ConnectedSlice == null)
            return;


        // Start playing when M is pressed.
        if (Input.GetKeyDown(KeyCode.M))
        {
            ConnectedSlice.playSound();
            ConnectedSlice.startGlow();
        }


        // Stop playing when M is released.
        if (Input.GetKeyUp(KeyCode.M))
        {
            ConnectedSlice.stopSound();
            ConnectedSlice.stopGlow();
        }
    }
}