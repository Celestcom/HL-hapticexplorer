﻿/* This code is licensed under the NullSpace Developer Agreement, available here:
** ***********************
** http://www.hardlightvr.com/wp-content/uploads/2017/01/NullSpace-SDK-License-Rev-3-Jan-2016-2.pdf
** ***********************
** Make sure that you have read, understood, and agreed to the Agreement before using the SDK
*/

using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//Effect tooltip?

namespace NullSpace.SDK.Demos
{
	abstract public class SuitDemo : MonoBehaviour
	{
		//Turn on my needed things
		abstract public void ActivateDemo();

		//Turn off my needed things
		abstract public void DeactivateDemo();

		abstract public void OnSuitClicked(SuitBodyCollider suit, RaycastHit hit);
		abstract public void OnSuitClicking(SuitBodyCollider suit, RaycastHit hit);
		abstract public void OnSuitNoInput();

		public Button MyEnableButton;
		public Button MyDisableButton;

		public Sprite EnabledSprite;
		public Sprite DisabledSprite;

		public KeyCode ActivateHotkey = KeyCode.None;

		public List<SuitBodyCollider> suitObjects;

		public List<GameObject> ActiveObjects;
		public List<GameObject> ActiveIfDisabledObjects;

		protected Color buttonSelected = new Color(150 / 255f, 150 / 255f, 150 / 255f, 1f);
		//protected Color buttonSelected = new Color(30 / 255f, 167 / 255f, 210 / 255f, 1f);
		protected Color buttonUnselected = new Color(255 / 255f, 255 / 255f, 255 / 255f, 1f);

		/// <summary>
		/// An overhead method that calls ActivateDemo.
		/// Enforce abstract 'need implementation' while still calling certain things without overriding.
		/// </summary>
		public void ActivateDemoOverhead()
		{
			SetEnableButtonBackgroundColor(buttonSelected);
			ActivateDemo();
			HandleRequiredObjects(true);
		}

		/// <summary>
		/// An overhead method that calls DeactivateDemo.
		/// Enforce abstract 'need implementation' while still calling certain things without overriding.
		/// </summary>
		public void DeactivateDemoOverhead()
		{
			SetEnableButtonBackgroundColor(buttonUnselected);
			DeactivateDemo();
			HandleRequiredObjects(false);
		}

		public virtual void SetupButtons()
		{
			if (MyEnableButton != null)
			{
				MyEnableButton.transform.FindChild("Icon").GetComponent<Image>().sprite = EnabledSprite;
			}
			if (MyDisableButton != null)
			{
				MyDisableButton.transform.FindChild("Icon").GetComponent<Image>().sprite = DisabledSprite;
			}
		}

		public virtual void Start()
		{
			suitObjects = new List<SuitBodyCollider>();
			suitObjects = FindObjectsOfType<SuitBodyCollider>().ToList();
			SetupButtons();
			SetEnableButtonBackgroundColor(buttonUnselected);
			DeactivateDemo();
			enabled = false;
		}

		public void HandleRequiredObjects(bool Activating)
		{
			for (int i = 0; i < ActiveObjects.Count; i++)
			{
				if (ActiveObjects[i])
				{
					//Debug.Log("Setting Active : " + ActiveObjects[i].name + "\n to " + Activating);
					ActiveObjects[i].SetActive(Activating);
				}
			}
			for (int i = 0; i < ActiveIfDisabledObjects.Count; i++)
			{
				if (ActiveIfDisabledObjects[i])
				{
					//Debug.Log("Setting Anti-Active : " + ActiveIfDisabledObjects[i].name + "\n to " + Activating);
					ActiveIfDisabledObjects[i].SetActive(!Activating);
				}
			}
		}

		public virtual void SetEnableButtonBackgroundColor(Color col)
		{
			if (MyEnableButton != null)
			{
				MyEnableButton.GetComponent<Image>().color = col;
			}
		}

		public IEnumerator ChangeColorDelayed(GameObject targetObject, Color setColor, float delay, float minDuration = .15f)
		{
			yield return new WaitForSeconds(Mathf.Clamp(delay, minDuration, float.MaxValue));
			ColorSuitCollider(targetObject, setColor);
		}

		public void ColorSuitCollider(GameObject suitCollider, Color setColor)
		{
			MeshRenderer rend = suitCollider.GetComponent<MeshRenderer>();
			if (rend != null)
			{
				rend.material.color = setColor;
			}
		}
		public void ColorSuitCollider(SuitBodyCollider suitCollider, Color setColor)
		{
			ColorSuitCollider(suitCollider.gameObject, setColor);
		}
	}

	public class SuitClickDemo : SuitDemo
	{
		//Turn on my needed things
		public override void ActivateDemo()
		{
			HandleRequiredObjects(true);

			//I need nothing
		}

		//Turn off my needed things
		public override void DeactivateDemo()
		{
			HandleRequiredObjects(false);

			//I need nothing
		}

		public override void OnSuitClicked(SuitBodyCollider clicked, RaycastHit hit)
		{
			Debug.Log("Clicked on " + clicked.name + " with a regionID value of: " + (int)clicked.regionID + "\n");
		}

		public override void OnSuitClicking(SuitBodyCollider clicked, RaycastHit hit)
		{
			Debug.Log("Clicked on " + clicked.name + " with a regionID value of: " + (int)clicked.regionID + "\n");
		}

		public override void OnSuitNoInput()
		{
		}
	}
}