using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class BasePanaceanPlant : MonoBehaviour
{
    [SerializeField] protected PanaceanPlantSO panaceanPlantSO;

    public GameObject GameObject
    {
        get { return gameObject; }
    }

    public PanaceanPlantSO GetPanaceanPlantSO()
    {
        return panaceanPlantSO;
    }

    /*public virtual void Equipped(){}
	
	public virtual void Unequipped(){}*/
}
