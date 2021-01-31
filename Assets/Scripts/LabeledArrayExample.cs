using UnityEngine;

public class LabeledArrayExample : MonoBehaviour
{
	[LabeledArray(new string[] { "First", "Second", "Third" })]
	public int[] labeledValues;
	
	public enum Order
	{
		First,
		Second,
		Third
	}
	
	[LabeledArray(typeof(Order))]
	public int[] enumLabeledValues;
}