using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartGame : MonoBehaviour
{
	[SerializeField] StringData p1Name;
	[SerializeField] StringData p2Name;
	[SerializeField] StringData p3Name;
	[SerializeField] StringData p4Name;

	[SerializeField] TMP_Text p1NameField;
	[SerializeField] TMP_Text p2NameField;
	[SerializeField] TMP_Text p3NameField;
	[SerializeField] TMP_Text p4NameField;
	
	public void DoIt()
	{
		p1Name.value = p1NameField.text;
		p2Name.value = p2NameField.text;
		p3Name.value = p3NameField.text;
		p4Name.value = p4NameField.text;
	}
}
