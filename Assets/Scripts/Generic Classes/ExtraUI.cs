using UnityEngine;

public class ExtraUI : MonoBehaviour {

    public GameObject ControlCanvas;

    public void setActivationStatus (bool _NewActivationStatus) {
        ControlCanvas.SetActive (_NewActivationStatus);
    }

    

}
