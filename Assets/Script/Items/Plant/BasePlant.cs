using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class BasePlant : MonoBehaviour
{
    [SerializeField] protected PlantSO plantSO;

	public GameObject GameObject
	{
		get { return gameObject; }
	}
	
	public PlantSO getPlantSO()
	{
		return plantSO;
	}
	
	/*public virtual void Equipped(){}
	
	public virtual void Unequipped(){}*/
}
