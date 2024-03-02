﻿//
//SpingManager.cs for unity-chan!
//
//Original Script is here:
//ricopin / SpingManager.cs
//Rocket Jump : http://rocketjump.skr.jp/unity3d/109/
//https://twitter.com/ricopin416
//
//Revised by N.Kobayashi 2014/06/24
//           Y.Ebata
//
using UnityEngine;
using System.Collections;

namespace UnityChan
{
	public class SpringManager : MonoBehaviour
	{
		//Kobayashi
		// DynamicRatio is paramater for activated level of dynamic animation 
		public float dynamicRatio = 1.0f;

		//Ebata
		public float			stiffnessForce;
		public AnimationCurve	stiffnessCurve;
		public float			dragForce;
		public AnimationCurve	dragCurve;
		public SpringBone[] springBones;
		public GameObject[] rootNodes;

		void Start ()
		{
			UpdateParameters ();
		}
	
		void Update ()
		{
#if UNITY_EDITOR
		//Kobayashi
		if(dynamicRatio >= 1.0f)
			dynamicRatio = 1.0f;
		else if(dynamicRatio <= 0.0f)
			dynamicRatio = 0.0f;
		//Ebata
		UpdateParameters();
#endif
		}
		[ContextMenu("骨骼綁定")]
    	private void GetEveryBones()
    	{
			SpringCollider[] springColliders = GetComponentsInChildren<SpringCollider>();
    	    foreach (GameObject rootNode in rootNodes)
    	    {
    	        foreach (Transform transformNode in rootNode.GetComponentsInChildren<Transform>())
    	        {
    	            if (transformNode.childCount > 0)
    	            {
    	                transformNode.gameObject.AddComponent<SpringBone>();
    	                transformNode.GetComponent<SpringBone>().child = transformNode.GetChild(0);
						transformNode.GetComponent<SpringBone>().colliders = springColliders;
    	            }
    	        }
    	    }
			springBones = GetComponentsInChildren<SpringBone>();
    	}
		[ContextMenu("骨骼解除綁定")]
		private void DelEveryBones()
		{
    	    foreach (GameObject rootNode in rootNodes)
    	    {
    	        foreach (Transform transformNode in rootNode.GetComponentsInChildren<Transform>())
    	        {
    	            if (transformNode.childCount > 0)
    	            {
    	                DestroyImmediate(transformNode.gameObject.GetComponent<SpringBone>());
    	            }
    	        }
    	    }
		}
		private void LateUpdate ()
		{
			//Kobayashi
			if (dynamicRatio != 0.0f) {
				for (int i = 0; i < springBones.Length; i++) {
					if (dynamicRatio > springBones [i].threshold) {
						springBones [i].UpdateSpring ();
					}
				}
			}
		}

		private void UpdateParameters ()
		{
			UpdateParameter ("stiffnessForce", stiffnessForce, stiffnessCurve);
			UpdateParameter ("dragForce", dragForce, dragCurve);
		}
	
		private void UpdateParameter (string fieldName, float baseValue, AnimationCurve curve)
		{
			var start = curve.keys [0].time;
			var end = curve.keys [curve.length - 1].time;
			//var step	= (end - start) / (springBones.Length - 1);
		
			var prop = springBones [0].GetType ().GetField (fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
		
			for (int i = 0; i < springBones.Length; i++) {
				//Kobayashi
				if (!springBones [i].isUseEachBoneForceSettings) {
					var scale = curve.Evaluate (start + (end - start) * i / (springBones.Length - 1));
					prop.SetValue (springBones [i], baseValue * scale);
				}
			}
		}
	}
}