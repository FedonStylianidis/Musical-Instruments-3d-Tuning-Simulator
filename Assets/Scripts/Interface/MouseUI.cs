using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;




#if UNITY_EDITOR
using UnityEditor;
#endif


public enum Labels
{
    NonInteractable,
    HarpsichordKey,
    HarpsichordDummyKey,
    HarpsichordTuningPanel,
    HarpsichordRegulatorPin,
    HarpsichordTuningHammer,
    Recorder,
    RecorderHole,
    RecorderMouthpiece,
    SnareDrumTuningPanel,
    SnareDrumLug,
    SnareDrumSlice,
    SnareDrumTuningKey
}


public enum Languages
{
    EN = 0,
    GR = 1
}



public class GameCursor
{

    public Texture2D Shape;
    public Vector2 Offset;

    public GameCursor(Texture2D _Shape, Vector2 _Offset)
    {
        Shape = _Shape;
        Offset = _Offset;
    }

}


public class OrdinaryNames
{

    public List<string> LanguageSpecificNames = new List<string>();

    public OrdinaryNames(string _NameEn, string _NameGr)
    {
        LanguageSpecificNames.Add(_NameEn);
        LanguageSpecificNames.Add(_NameGr);
    }

}


public class MouseUI : MonoBehaviour
{

    static GameObject Ego;

    static Texture2D WedgeCursor, FingerCursor, HandCursor, GrabCursor, PressingFingerCursor, EyeCursor, SpinArrowCursor;

    public static int Wedge = 0;
    static int Finger = 1;
    static int ClickingFinger = 2;
    static int Hand = 3;
    static int Grab = 4;
    static int Eye = 5;
    static int SpinArrow = 6;

    static int CurrentCursor;

    static List<GameCursor> Cursors;

    static Event occurence;

    static int Tooltip_MiddleX;
    static int Tooltip_MiddleY;

    static int Tooltip_SizeX;
    static int Tooltip_SizeY;

    static Vector2 Coords_for_FrozenToolTip;

    static string Text_for_Tooltip;

    static Texture2D TooltipBG;

    public Labels Label;

    [HideInInspector]
    public bool Zoomable, Pressable, Rotatable, Movable, Receptable;

    [HideInInspector]
    public GameObject Place;

    [HideInInspector]
    public bool TemporarilyInaccessible;

    string UserFriendlyName;


    static GameObject ObjectHoveringOver;
    public static GameObject ObjectBeingCarried;

    Vector3 VectorDistance_from_Camera;

    const float MinimumDistance_for_Interaction = 2F;

    static GameObject ObjectMouseIsDownOn;

    public static bool Rotating; //public because called by EgoController

    static Vector3 RayHitPoint_for_MouseDown;

    static bool UsingTwoObjectsTogether;

    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(MouseUI)), CanEditMultipleObjects]
    public class MouseUI_Editor : Editor
    {

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            MouseUI mouseUI = (MouseUI)target;

          if (mouseUI.Label != Labels.NonInteractable)
            {
                showAttributes(mouseUI);
            }

            if (mouseUI.Label != Labels.NonInteractable &&
     BooleanValues[mouseUI.Label][3])
            {
                showPlace(mouseUI);
            }

        }


        void showAttributes(MouseUI _mouseUI)
        {
            if (_mouseUI.Label == Labels.NonInteractable)
                return;

            List<bool> values =
                BooleanValues[_mouseUI.Label];

            EditorGUILayout.Space();

            EditorGUILayout.LabelField(
                "Interaction Properties",
                EditorStyles.boldLabel
            );

            EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.Toggle(
                "Zoomable",
                values[0]
            );

            EditorGUILayout.Toggle(
                "Pressable",
                values[1]
            );

            EditorGUILayout.Toggle(
                "Rotatable",
                values[2]
            );

            EditorGUILayout.Toggle(
                "Movable",
                values[3]
            );

            EditorGUILayout.Toggle(
                "Receptable",
                values[4]
            );

            EditorGUI.EndDisabledGroup();
        }
       

        void showPlace(MouseUI _mouseUI)
        {

            EditorGUILayout.Space();

            _mouseUI.Place = EditorGUILayout.ObjectField("Place", _mouseUI.Place, typeof(GameObject), true) as GameObject;

        }

    }

#endif
    #endregion


    void Start()
    {

        Ego = GameObject.Find("Ego");

        WedgeCursor = Resources.Load("wedge") as Texture2D;
        FingerCursor = Resources.Load("finger") as Texture2D;
        PressingFingerCursor = Resources.Load("pressing_finger") as Texture2D;
        HandCursor = Resources.Load("hand") as Texture2D;
        GrabCursor = Resources.Load("grab") as Texture2D;
        EyeCursor = Resources.Load("eye") as Texture2D;
        SpinArrowCursor = Resources.Load("spin_arrow") as Texture2D;


        Cursors = new List<GameCursor>();

        Cursors.Add(new GameCursor(WedgeCursor, Vector2.zero));
        Cursors.Add(new GameCursor(FingerCursor, new Vector2(9, 2)));
        Cursors.Add(new GameCursor(PressingFingerCursor, new Vector2(9, 2)));
        Cursors.Add(new GameCursor(HandCursor, new Vector2(11, 3)));
        Cursors.Add(new GameCursor(GrabCursor, new Vector2(7, 2)));
        Cursors.Add(new GameCursor(EyeCursor, new Vector2(12, 12)));
        Cursors.Add(new GameCursor(SpinArrowCursor, Vector2.zero));

        Tooltip_MiddleX = 120;
        Tooltip_SizeX = 2 * Tooltip_MiddleX;

        Tooltip_SizeY = 50;
        Tooltip_MiddleY = Tooltip_SizeY + 5;

        TooltipBG = Resources.Load("ui_tooltip") as Texture2D;

        ObjectHoveringOver = null;

        ObjectBeingCarried = null;

        ObjectMouseIsDownOn = null;

        Rotating = false;

        TemporarilyInaccessible = false;

        UserFriendlyName = AttributedName(Label);

        if (Label != Labels.NonInteractable)           ///  solves the pressable bug of the keys cause it initializes them with the label, otherwise they dont initialize correctly
        {
            Zoomable = BooleanValues[Label][0];       
            Pressable = BooleanValues[Label][1];
            Rotatable = BooleanValues[Label][2];
            Movable = BooleanValues[Label][3];
            Receptable = BooleanValues[Label][4];
        } 

        CurrentCursor = Wedge;

        RayHitPoint_for_MouseDown = Vector3.zero;

        UsingTwoObjectsTogether = false;

    }



    public static void switchCursor(int _CursorID)
    {

        CurrentCursor = _CursorID;

        Cursor.visible = true;

        Cursor.SetCursor(Cursors[CurrentCursor].Shape,
                         Cursors[CurrentCursor].Offset,
                         CursorMode.Auto);

    }

    public static void hideCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.visible = false;
    }

    static Camera getActiveCamera()    
    {
        Camera[] cameras =
            Camera.allCameras;

        foreach (Camera cam in cameras)
        {
            if (cam.enabled &&
                cam.gameObject.activeInHierarchy)
            {
                return cam;
            }
        }

        return null;
    }

    public static void callOnGUI()
    {

#if !UNITY_EDITOR
        if (EgoController.CursorFrozen)
            DisplayFakeCursor ();
#endif

        occurence = Event.current;

        if (!UsingTwoObjectsTogether && !Rotating && ObjectHoveringOver != null &&
            ObjectBeingCarried != ObjectHoveringOver &&
            ObjectHoveringOver.GetComponent<MouseUI>().Label != Labels.NonInteractable)
        {

            DisplayTooltip();
        }
        else if (Rotating)
            DisplayFrozenTooltip();

    }


    static void DisplayFakeCursor()
    {
        if (!UsingTwoObjectsTogether &&
     !(ObjectBeingCarried != null && ObjectHoveringOver == null))
            GUI.DrawTexture(new Rect(Screen.width / 2 - Cursors[CurrentCursor].Offset.x,
                Screen.height / 2 - Cursors[CurrentCursor].Offset.y, 32, 32),
                Cursors[CurrentCursor].Shape);
    }


    static void DisplayTooltip()
    {
        GUI.skin.box.alignment = TextAnchor.MiddleCenter;
        GUI.skin.box.normal.background = TooltipBG;
        GUI.skin.box.fontSize = 16;
        GUI.skin.box.normal.textColor = UnityEngine.Color.yellow;
        GUI.backgroundColor = new UnityEngine.Color(0F, 0F, 0F, 0.3F);
        GUI.Box(new Rect(occurence.mousePosition.x - Tooltip_MiddleX,
            occurence.mousePosition.y - Tooltip_MiddleY, Tooltip_SizeX, Tooltip_SizeY),
            Text_for_Tooltip);
    }


    static void DisplayFrozenTooltip()
    {
        GUI.skin.box.alignment = TextAnchor.MiddleCenter;
        GUI.skin.box.normal.background = TooltipBG;
        GUI.skin.box.fontSize = 16;
        GUI.skin.box.normal.textColor = UnityEngine.Color.yellow;
        GUI.backgroundColor = new UnityEngine.Color(0F, 0F, 0F, 0.3F);
        GUI.Box(new Rect(Coords_for_FrozenToolTip.x - Tooltip_MiddleX,
                Coords_for_FrozenToolTip.y - Tooltip_MiddleY, Tooltip_SizeX, Tooltip_SizeY),
                Text_for_Tooltip);
    }


    void OnMouseOver()
    {


        if (Label != Labels.NonInteractable && !TemporarilyInaccessible && (!Movable ||
            EgoController.PermittingCollection_of_Objects[Ego.GetComponent<EgoController>().Mode])) 
        {

           
            Camera activeCamera =  
       getActiveCamera();

            if (activeCamera == null)
                return;

            VectorDistance_from_Camera =
                transform.position -
                activeCamera.transform.position;

            if (!Rotating && VectorDistance_from_Camera.magnitude < MinimumDistance_for_Interaction)
            {

                Text_for_Tooltip = UserFriendlyName;     

                InteractableObject interactable =
                    GetComponent<InteractableObject>();

                if (interactable != null)
                {
                    string CustomTooltip =
                        interactable.getTooltipName();

                    if (CustomTooltip != "")
                        Text_for_Tooltip = CustomTooltip;
                }                                              



                //if (ObjectBeingCarried == null && ObjectMouseIsDownOn == null)
                if (ObjectMouseIsDownOn == null)
                {

                    ObjectHoveringOver = gameObject;


                }
                else if (Receptable && ObjectBeingCarried != null)
                    //cursor is already grab in this case // not anymore
                    ObjectHoveringOver = gameObject;

                // if (Receptable)     
                //    switchCursor(Hand);  // THE RECEPTABLE SWITCHES TO HAND ONLY WHEN SOMETHING CAN INTERACT WITH IT AND IS BEING CARRIED
                if (Receptable && ObjectBeingCarried != null)
                    switchCursor(Hand);
                else if (Pressable)
                    switchCursor(Finger);
                else if (Zoomable)
                    switchCursor(Eye);
                else if (Rotatable)
                    switchCursor(SpinArrow);
                else if (Movable)
                    switchCursor(Hand);
                else
                    switchCursor(Wedge);

            }

        }

    }


    void OnMouseExit()
    {

        if (Label != Labels.NonInteractable && !Rotating)
        {

           
            if (ObjectMouseIsDownOn == null)
                ObjectHoveringOver = null;
            if (ObjectBeingCarried == null)
                switchCursor(Wedge);
            else
               
                hideCursor(); 

        }
       
    }


    void OnMouseDown()
    {

        if (Label != Labels.NonInteractable &&
            ObjectHoveringOver == gameObject)
        {

            ObjectMouseIsDownOn = gameObject;

            if (Movable && ObjectBeingCarried == null)
            {// A movable object can be picked up only
             // when no other object is being carried.

                switchCursor(Grab);

            }
            else if (Rotatable && !Rotating && ObjectBeingCarried == null)
            {

                Coords_for_FrozenToolTip =
                    new Vector2(occurence.mousePosition.x, occurence.mousePosition.y);

                switchCursor(Grab);

            }
            else if (Pressable)
            {

                GetComponent<InteractableObject>().press();

                switchCursor(ClickingFinger);

            }
            else if (Zoomable)
            {

                GetComponent<InteractableObject>().zoom();

                switchCursor(Wedge);

            }
            else if (Receptable && ObjectBeingCarried != null)
            {

                switchCursor(Hand);

            }

        }
        else if (ObjectBeingCarried != null && ( tag == "ToolsStand" || tag == "DropArea" || tag == "RecorderStand"))
        {

            // Store the drop-surface point where
            // the mouse press started.

            Camera activeCamera = getActiveCamera();
            if (activeCamera == null)   
                return;
            Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);

           
            VectorDistance_from_Camera = hit.point - activeCamera.transform.position;

            if (VectorDistance_from_Camera.magnitude < MinimumDistance_for_Interaction)
            {

                RayHitPoint_for_MouseDown = hit.point;

                hideCursor();

            }

        }

    }

    void OnMouseDrag()
    {
     

        if (Rotatable && ObjectMouseIsDownOn == gameObject)
        {// Rotate only the object on which
         // the mouse press started.

            Rotating = true;

             float mouse_dx = Input.GetAxis("Mouse X");
             float mouse_dy = Input.GetAxis("Mouse Y");

             if (mouse_dx != 0F || mouse_dy != 0F)
                GetComponent<RotatableObject>().rotate(new Vector2(mouse_dx, -mouse_dy));

        }

    }



    async Task OnMouseUp()
    {
        if (Pressable)                                       
            GetComponent<InteractableObject>().release();

        if (Rotating)
        {

            GetComponent<InteractableObject>().doneRotating();

            Rotating = false;

            switchCursor(Wedge); //if the cursor stops over the rotatable, it will immediately become a hand due to OnMouseOver()

        }
        else if (Label != Labels.NonInteractable)
        {
            
            if (ObjectMouseIsDownOn == gameObject && ObjectHoveringOver == gameObject)
            {

              
                
                    if (Movable && ObjectBeingCarried == null) 
                {
                    GetComponent<MovableObject>()
                        .prepareForCarrying();

                    if (Place != null &&
                        Place.GetComponent<InteractableObject>())
                    {
                        Place
                            .GetComponent<InteractableObject>()
                            .evacuate(gameObject);
                    }

                    Ego
                        .GetComponent<EgoController>()
                        .attach(gameObject);

                    ObjectBeingCarried =
                        gameObject;

                    hideCursor();
                }
                else if (Receptable && ObjectBeingCarried != null)
                {//clicking on an object to put the object being carried on it
                   

                    await tryUsingTogether_with_ObjectBeingCarried();

                    //cursor and ObjectBeingCarried variable are being dealt inside tryUsingTogether_with_ObjectBeingCarried()

                }

            }

        }
       
        else if (
   ObjectBeingCarried != null &&
   ( tag == "DropArea" ||
     tag == "ToolsStand" ||
     tag == "RecorderStand"))
        {
            // Second ray cast - when mouse is up.
            Camera activeCamera =
                getActiveCamera();

            if (activeCamera == null)
                return;


            Ray ray =
                activeCamera.ScreenPointToRay(
                    Input.mousePosition
                );

            RaycastHit hit;

            Physics.Raycast(
                ray,
                out hit
            );


            if (MathFunctions.ApproximateProximity_of(
                    RayHitPoint_for_MouseDown,
                    hit.point,
                    0.1F))
            {



                // RECORDER STAND

                if (tag == "RecorderStand")
                {
                    if (ObjectBeingCarried.GetComponent<MouseUI>().Label == Labels.Recorder)
                    {
                        MovableObject movableObject =
                            ObjectBeingCarried.GetComponent<MovableObject>();

                        if (movableObject != null)
                        {
                            movableObject.returnToOriginalRestingPose();
                        }

                        ObjectBeingCarried
                            .GetComponent<MouseUI>()
                            .Place = gameObject;

                        ObjectBeingCarried = null;

                        switchCursor(Wedge);
                    }
                    else
                    {
                        hideCursor();
                    }
                }
                // --------------------------------
                // TOOLS STAND
                // --------------------------------

                else if (tag == "ToolsStand")
                {
                    if (ObjectBeingCarried.GetComponent<MouseUI>().Label != Labels.Recorder)
                    {
                        MovableObject movableObject =
                            ObjectBeingCarried.GetComponent<MovableObject>();

                        if (movableObject != null)
                        {
                            movableObject.returnToOriginalRestingPose();
                        }

                        ObjectBeingCarried
                            .GetComponent<MouseUI>()
                            .Place = gameObject;

                        ObjectBeingCarried = null;

                        switchCursor(Wedge);
                    }
                    else
                    {
                        hideCursor();
                    }
                }


                // --------------------------------
                //  DROP AREA
                // --------------------------------

                else if (
                            !ObjectBeingCarried
                                .GetComponent<MouseUI>()
                                .AreThereObjects_around(
                                    hit.point
                                ))
                        {
                            ObjectBeingCarried.transform
                                .SetParent(
                                    null
                                );


                            ObjectBeingCarried
                                .GetComponent<MovableObject>()
                                .restoreUprightRotation();


                            ObjectBeingCarried.transform.position =
                                new Vector3(
                                    hit.point.x,
                                    hit.point.y +
                                    ObjectBeingCarried
                                        .GetComponent<MovableObject>()
                                        .Y_Offset_for_Relocation,
                                    hit.point.z
                                );


                            ObjectBeingCarried
                                .GetComponent<MovableObject>()
                                .prepareForRelocation();


                            ObjectBeingCarried
                                .GetComponent<MouseUI>()
                                .Place =
                                gameObject;


                            ObjectBeingCarried =
                                null;


                            switchCursor(Wedge);
                        }
                        else
                        {
                            hideCursor();
                        }
            }
            else
            {
                hideCursor();
            }
        }

        ObjectMouseIsDownOn = null;

        ObjectHoveringOver = null; //It needs to be set to null every time, otherwise the tooltip may continue after done rotating an object

    }


    async Task tryUsingTogether_with_ObjectBeingCarried()
    {


        UsingTwoObjectsTogether = true;

        Values_After_JointUse ResultValues = await GetComponent<InteractableObject>().use_with(ObjectBeingCarried);

        UsingTwoObjectsTogether = false;

        
        if (ResultValues.JointUse_TookPlace)   
        {
            GameObject carriedObject =
                ObjectBeingCarried;


            setInteractivity(
                ResultValues.Receptor_NewInteractivity
            );


            if (ResultValues.Receptor_NewPlace != null)
            {
                Place =
                    ResultValues.Receptor_NewPlace;
            }


            if (carriedObject != null)
            {
                carriedObject
                    .GetComponent<MouseUI>()
                    .setInteractivity(
                        ResultValues
                            .ObjectBeingCarried_NewInteractivity
                    );
            }


            if (ResultValues.ObjectBeingCarried_NewPlace != null)
            {
                if (carriedObject != null)
                {
                    carriedObject
                        .GetComponent<MouseUI>()
                        .Place =
                        ResultValues
                            .ObjectBeingCarried_NewPlace;
                }

                ObjectBeingCarried =
                    null;

                switchCursor(Wedge);
            }
            else
            {
                if (carriedObject != null)
                {
                    Ego
                        .GetComponent<EgoController>()
                        .attach(carriedObject);

                    ObjectBeingCarried =
                        carriedObject;

                    hideCursor();
                }
            }
        }

    }


    bool AreThereObjects_around(Vector3 _HitPoint)
    {

        Collider[] OtherColliders = Physics.OverlapSphere(_HitPoint, GetComponent<MeshRenderer>().bounds.size.magnitude / 2);

        for (int i = 0; i < OtherColliders.Length; i++)
        {

            if (OtherColliders[i].gameObject.tag != "Bench" &&
                OtherColliders[i].gameObject.tag != "Room" &&
                OtherColliders[i].gameObject.tag != "DropArea")
            {

                return true;

            }

        }

        return false;

    }




    public void setInteractivity(bool _InteractivityState)
    {

        for (int i = 0; i < GetComponents<BoxCollider>().Length; i++)
            GetComponents<BoxCollider>()[i].enabled = _InteractivityState;

        for (int i = 0; i < transform.childCount; i++)
        {

            if (transform.GetChild(i).GetComponent<MouseUI>())
                transform.GetChild(i).GetComponent<MouseUI>().setInteractivity(_InteractivityState);

        }

    }


    static readonly Dictionary<Labels, OrdinaryNames> NameAttribution = new Dictionary<Labels, OrdinaryNames> {
        {Labels.NonInteractable, new OrdinaryNames(null,null)},
        {Labels.HarpsichordKey, new OrdinaryNames ("KEY","ΠΛΗΚΤΡΟ") }, 
        {Labels.HarpsichordDummyKey, new OrdinaryNames("DUMMY KEY", "ΑΝΕΝΕΡΓΟ ΠΛΗΚΤΡΟ")},
        {Labels.HarpsichordTuningPanel, new OrdinaryNames("TUNING AREA", "ΠΕΡΙΟΧΗ ΚΟΥΡΔΙΣΜΑΤΟΣ")},
        {Labels.HarpsichordRegulatorPin, new OrdinaryNames("REGULATOR PIN", "ΠΕΙΡΟΣ ΚΟΥΡΔΙΣΜΑΤΟΣ")},
        {Labels.HarpsichordTuningHammer, new OrdinaryNames("TUNING HAMMER", "ΚΛΕΙΔΙ ΚΟΥΡΔΙΣΜΑΤΟΣ")},
        {Labels.Recorder, new OrdinaryNames("RECORDER", "ΦΛΟΓΕΡΑ")},
        {Labels.RecorderHole, new OrdinaryNames("RECORDER HOLE", "ΟΠΗ ΦΛΟΓΕΡΑΣ")},
        {Labels.RecorderMouthpiece, new OrdinaryNames("RECORDER MOUTHPIECE", "ΕΠΙΣΤΟΜΙΟ ΦΛΟΓΕΡΑΣ" )},
        {Labels.SnareDrumTuningPanel,new OrdinaryNames( "SNARE DRUM", "ΤΑΜΠΟΥΡΟ") },
        {Labels.SnareDrumLug,new OrdinaryNames("SNARE LUG","ΒΙΔΑ ΣΥΣΦΙΞΗΣ")},
        {Labels.SnareDrumSlice, new OrdinaryNames( "SNARE MEMBRANE PART", "ΤΜΗΜΑ ΜΕΜΒΡΑΝΗΣ ΤΑΜΠΟΥΡΟΥ")},
        {Labels.SnareDrumTuningKey, new OrdinaryNames( "SNARE DRUM TUNING KEY", "ΚΛΕΙΔΙ ΧΟΡΔΙΣΜΑΤΟΣ ΤΑΜΠΟΥΡΟΥ")
},


    };

    public static string AttributedName(Labels _NameLabel)
    {
        return NameAttribution[_NameLabel].LanguageSpecificNames[(int)Specs.Language];
    }


    public static readonly Dictionary<Labels, List<bool>> BooleanValues =
        new Dictionary<Labels, List<bool>>() {
        //1st: Zoomable; 2nd: Pressable; 3rd: Rotatable; 4th: Movable; 5th: Receptable
        {Labels.HarpsichordKey, new List<bool>{false,true,false,false,false }},   
        {Labels.HarpsichordDummyKey, new List<bool>{false,true,false,false,false}},
        {Labels.HarpsichordTuningPanel, new List<bool>{true,false,false,false,false}},
        {Labels.HarpsichordRegulatorPin, new List<bool>{false,false,false,false,true}},
        {Labels.HarpsichordTuningHammer, new List<bool>{false,false,false,true,false}},
        {Labels.Recorder, new List<bool>{false,false,false,true,false}},
        {Labels.RecorderHole, new List<bool>{false,true,false,false,false}},
        {Labels.RecorderMouthpiece, new List<bool>{false,true,false,false,false}},
        {Labels.SnareDrumTuningPanel,new List<bool>{true, false, false, false, false}},
        {Labels.SnareDrumLug,new List<bool>{false, false, false, false, true}},
        {Labels.SnareDrumSlice,new List<bool>{false, true, false, false, false}},
        {Labels.SnareDrumTuningKey,new List<bool>{false, false, false, true, false}},
        };  


}
