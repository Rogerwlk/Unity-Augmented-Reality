using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech.View
{
	/// <summary>
	/// Represents each planet GameObject.
	/// </summary>
	public class Planet : MonoBehaviour
	{
		//Enums
		public enum PlanetType
		{
			None,
			Mercury,
			Venus,
			Earth,
			Mars,
			Jupiter,
			Saturn,
			Uranus,
			Neptune
		}

		//Structs/classes
		public struct PlanetData
		{
			//How long it takes for this planet to make a full revolution around the sun. (One relative year)
			public float revolutionPeriod;
			//How long it takes for this planet to make full rotation along its own axis. (One relative day)
			public float rotationPeriod;

			public PlanetData (float revolutionPeriod, float rotationPeriod)
			{
				this.revolutionPeriod = revolutionPeriod;
				this.rotationPeriod = rotationPeriod;
			}
		}

		//Readonly/const
		protected readonly Dictionary<PlanetType,PlanetData> planetData = new Dictionary<PlanetType, PlanetData> () {
			{ PlanetType.Mercury,new PlanetData (88f, 59f) },
			{ PlanetType.Venus,new PlanetData (224.7f, 243f) },
			{ PlanetType.Earth,new PlanetData (365.2f, 24f) },
			{ PlanetType.Mars,new PlanetData (687f, 24.6f) },
			{ PlanetType.Jupiter,new PlanetData (4328.9f, 9.9f) },
			{ PlanetType.Saturn,new PlanetData (10752.9f, 10.6f) },
			{ PlanetType.Uranus,new PlanetData (30660f, 16.8f) },
			{ PlanetType.Neptune,new PlanetData (60225f, 16.1f) },
		};

		//Serialized
		[SerializeField]
		protected PlanetType planetType;
		[SerializeField]
		protected Transform sunTransform;
		
		/////Protected/////
		//References
		protected Vector3 startPosition;
		protected Rigidbody rigidbody;
		//Primitives
		protected float startDistanceFromSun;

        private Vector3 m_localPosition;
        private Quaternion m_localRotation;
        private Vector3 m_velocity;
        private Vector3 m_angularVelocity;
        //private Vector3

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{
			this.rigidbody = GetComponentInChildren<Rigidbody> ();
			this.startDistanceFromSun = Vector3.Distance (sunTransform.position, transform.position);
			this.startPosition = transform.position;
            m_localPosition = rigidbody.transform.localPosition;
            m_localRotation = rigidbody.transform.localRotation;
            m_velocity = rigidbody.velocity;
            m_angularVelocity = rigidbody.angularVelocity;
        }
		
		protected void Start ()
		{
        }
		
		protected void Update ()
		{	
			UpdateOrbit ();
			UpdateRotation ();
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// Planet Functions
		//

		/// <summary>
		/// Resets my transform's position to start position.
		/// Resets my rigidbody's velocity, angularVelocity, localPosition, and localRotation.
		/// </summary>
		public void Reset()
		{
            //TODO: Fill.
            transform.position = startPosition;
            rigidbody.transform.localPosition = m_localPosition;
            rigidbody.transform.localRotation = m_localRotation;
            rigidbody.velocity = m_velocity;
            rigidbody.angularVelocity = m_angularVelocity;
		}

		/// <summary>
		/// Orbit/revolve around the sun.
		/// </summary>
		protected void UpdateOrbit ()
		{
			float myPlanetRevolutionPeriod = planetData[planetType].revolutionPeriod;
            //TODO:Fill.
            transform.RotateAround(sunTransform.position, Vector3.up, RevolutionPeriodToAngle(myPlanetRevolutionPeriod));
		}

		/// <summary>
		/// Updates the rotation around the planet's own axis.
		/// </summary>
		protected void UpdateRotation ()
		{
            float myPlanetRotationPeriod = planetData[planetType].rotationPeriod;
            //TODO:Fill.
            transform.RotateAround(transform.position, Vector3.up, RotationPeriodToAngle(myPlanetRotationPeriod));
		}
		
		////////////////////////////////////////
		//
		// Utility Functions

		/// <summary>
		/// Converts a planets revolution period to a rotation angle.
		/// </summary>
		/// <returns>The rotation angle.</returns>
		/// <param name="revolutionPeriod">Revolution period.</param>
		protected float RevolutionPeriodToAngle (float revolutionPeriod)
		{
			//Inversely proportional
			float angle = 1f / revolutionPeriod;
			//Make it big
			angle *= 10000f;
			//Account for different frame rates
			angle *= Time.deltaTime;

			return angle;
		}

		/// <summary>
		/// Converts a planets rotation period to a rotation angle.
		/// </summary>
		/// <returns>The rotation angle.</returns>
		/// <param name="rotationPeriod">Rotation period.</param>
		protected float RotationPeriodToAngle (float rotationPeriod)
		{
			//Inversely proportional
			float angle = 1f / rotationPeriod;
			//Make it big
			angle *= 1000f;
			//Account for different frame rates
			angle *= Time.deltaTime;

			return angle;
		}

	}
}