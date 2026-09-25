using UnityEngine;

public class Values_After_JointUse
{

    public bool JointUse_TookPlace;

    public GameObject ObjectBeingCarried_NewPlace;
    public GameObject Receptor_NewPlace;

    public bool ObjectBeingCarried_NewInteractivity;
    public bool Receptor_NewInteractivity;


    public Values_After_JointUse(bool _JointUse_TookPlace, GameObject _NewPlace_for_ObjectBeingCarried, bool _NewInteractivity_for_ObjectBeingCarried, GameObject _NewPlace_for_Receptor, bool _NewInteractivity_for_Receptor)
    {

        JointUse_TookPlace = _JointUse_TookPlace;

        ObjectBeingCarried_NewPlace = _NewPlace_for_ObjectBeingCarried;
        ObjectBeingCarried_NewInteractivity = _NewInteractivity_for_ObjectBeingCarried;

        Receptor_NewPlace = _NewPlace_for_Receptor;
        Receptor_NewInteractivity = _NewInteractivity_for_Receptor;

    }


    public Values_After_JointUse(bool _JointUse_TookPlace)
    {
        //called only with false argument
        JointUse_TookPlace = _JointUse_TookPlace;
    }

}
