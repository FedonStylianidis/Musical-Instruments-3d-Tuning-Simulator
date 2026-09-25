

using UnityEngine;

public class RotateSnareDrumTuningKey
    : LimitedRotatableObject
{
    [Header("Tuning Rod Physics")]

    // Thread pitch p:
    // axial distance travelled by the tension rod
    // during one complete 360-degree revolution.
    //
    // Unit: millimetres / revolution
    public float ThreadPitch = 0.8f;


    // Effective tension response k:
    // change in normalized membrane tension
    // produced by 1 mm of axial tension-rod movement.
    //
    // The membrane tension in this simulation
    // is normalized, with the target tension = 1.
    //
    // Unit: normalized tension / millimetre
    public float TensionChangePerMillimeter = 0.1f;


    private CarrySnareDrumTuningKey carryKey;


    public override void Start()
    {
        base.Start();

        carryKey =
            GetComponent<
                CarrySnareDrumTuningKey>();
    }


    public void prepareForLug()
    {
        // The tuning key has already been placed
        // in its correct orientation on the lug.
        //
        // This orientation becomes the zero-angle
        // reference for its subsequent rotation.

        StartingRotation =
            transform.rotation;

        CurrentAngle = 0f;
    }


    protected override void afterRotation(
        float actualRotation)
    {
        // ------------------------------------------------
        // 1. TUNING-ROD DISPLACEMENT
        // ------------------------------------------------
        //
        // A threaded rod converts rotational motion
        // into linear (axial) motion.
        //
        //              Δθ
        // Δx = p * --------
        //             360°
        //
        // p  = thread pitch (mm / revolution)
        // Δθ = rotation of tuning key (degrees)
        // Δx = axial displacement of rod (mm)

        float displacementMillimeters =
            ThreadPitch *
            actualRotation /
            360f;


        // ------------------------------------------------
        // 2. MEMBRANE TENSION CHANGE
        // ------------------------------------------------
        //
        // We approximate the local mechanical response
        // of the drumhead/hoop/lug system as linear:
        //
        // ΔT = k * Δx
        //
        // ΔT = change in normalized membrane tension
        // k  = effective tension response
        // Δx = axial rod displacement (mm)
        //
        // k is a calibration parameter representing
        // the combined mechanical response of the
        // tension rod, hoop and membrane.

        float tensionChange =
            displacementMillimeters *
            TensionChangePerMillimeter;


        // Apply the calculated tension change
        // to the membrane section connected
        // to this lug.

        carryKey.AttachedLug.changeTension(
            tensionChange
        );
    }

   
}