using System.Threading.Tasks;
using UnityEngine;


public enum Axes
{
    X_Axis,
    Y_Axis,
    Z_Axis
}


public class InteractableObject : MonoBehaviour
{

   
    public virtual void Start()
    {
    }

    public virtual string getTooltipName()       
    {
        return "";
    }
    public virtual void press()
    {
    }

    public virtual void zoom()
    {
    }

    public virtual void rotate(Vector2 _dv)
    {
    }

    public virtual void doneRotating()
    {
    }
    public virtual void release()    
    {
    }
    public virtual async Task<Values_After_JointUse> use_with(GameObject _OtherObject)
    {
        Values_After_JointUse ReturnValues = new Values_After_JointUse(false);
        return ReturnValues;
    }

    public virtual void evacuate(GameObject _object)
    {
    }


}
