namespace ARVRAssignments
{
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using System.Collections.Generic;
	using Vuforia;
	using System;
	using UnityEngine.SceneManagement;

	public class GameController : MonoBehaviour
	{
		public const float r_LineDrawSpeed = 0.5f;

		public enum GameState
		{
			AddingMarkers,
			AnimatingLineDraw,
			ReadyToSpawnSentinel,
			ReadyToHitSentinel
		};

		public enum GameTag
		{
			Robot
		};

		[SerializeField]
		public ContentPositioningBehaviour m_ContentPositioningBhvr;

		[SerializeField]
		public AnchorInputListenerBehaviour m_AnchorInputLstnrBhvr;

		[SerializeField]
		public LineRenderer m_LineRenderer;

		[SerializeField]
		public TextMesh m_DistanceTextHldr;

		[SerializeField]
		public Button m_ResetButton;

		private GameState m_nowState;

		//TODO: Declare the class members here.
		private int m_MarkerNum;

		private Vector3[] m_Positions;

		private float m_StartTime;

		private float m_JourneyLength;

		private Vector3 m_MidPoint;

		private float m_TimeTrigger;

		private GameObject m_Sentinel;

		private void Start()
		{
			m_Positions = new Vector3[2];
			m_MarkerNum = 0;
			m_nowState = GameState.AddingMarkers;
			m_LineRenderer.startWidth = m_LineRenderer.endWidth = 0.005f;
			m_LineRenderer.positionCount = m_Positions.Length;
			//TODO: Initalise the class members and event listeners.
			m_ContentPositioningBhvr.OnContentPlaced.AddListener(SpawnNewMarker);
			m_LineRenderer.enabled = false;
			m_DistanceTextHldr.text = "";
			m_ResetButton.onClick.AddListener(Clear);
		}

		private void SpawnNewMarker(GameObject newMarker)
		{
			//TODO: Implement mini task 2 and part of mini task 3 here.
			if (m_nowState == GameState.AddingMarkers) {
				Transform markerTransform = newMarker.GetComponent<Transform>();
				markerTransform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
				Debug.Log(m_MarkerNum.ToString());
				m_Positions[m_MarkerNum] = newMarker.transform.position;
				m_MarkerNum += 1;
				if (m_MarkerNum == 2) {
					// m_ContentPositioningBhvr.OnContentPlaced.RemoveListener(SpawnNewMarker);
					m_AnchorInputLstnrBhvr.enabled = false;
					m_ContentPositioningBhvr.OnContentPlaced.RemoveAllListeners();
					m_nowState = GameState.AnimatingLineDraw;
					m_StartTime = Time.time;
					m_JourneyLength = Vector3.Distance(m_Positions[0], m_Positions[1]);
					m_MidPoint = (m_Positions[0] + m_Positions[1]) / 2f;
					m_ContentPositioningBhvr.DuplicateStage = false;
				}
			}
		}

		private void Update()
		{
			if (m_nowState == GameState.AnimatingLineDraw) {
				//TODO: Implement mini task 3 and 4 here.
				m_LineRenderer.enabled = true;
				float distCovered = (Time.time - m_StartTime) * r_LineDrawSpeed;

				float fracJourney = distCovered / m_JourneyLength;

				Vector3[] positions = new Vector3[2];

				positions[0] = m_Positions[0];
				positions[1] = Vector3.Lerp(m_Positions[0], m_Positions[1], fracJourney);
				
				m_LineRenderer.SetPositions(positions);

				if (positions[1] == m_Positions[1]) {
					m_DistanceTextHldr.text = Math.Round(m_JourneyLength, 2).ToString() + 'm';
					m_DistanceTextHldr.transform.localPosition = m_MidPoint;
					m_DistanceTextHldr.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
					m_DistanceTextHldr.transform.localRotation = Quaternion.Euler(90f, 0, 0);
					m_DistanceTextHldr.color = Color.red;
					m_nowState = GameState.ReadyToSpawnSentinel;
					m_TimeTrigger = Time.time;
				}
			}
			else if (m_nowState == GameState.ReadyToSpawnSentinel && Input.touchCount > 0 && Time.time - m_TimeTrigger > 0.4) {
				//TODO: Implement mini task 5 and 6 here.
				m_Sentinel = Instantiate(Resources.Load<GameObject>("Robot Kyle"));
				Instantiate(Resources.Load("Spawn"));
				m_Sentinel.transform.localPosition = m_MidPoint;
				m_Sentinel.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
				m_Sentinel.transform.localRotation = Quaternion.Euler(0, 180f, 0);
				m_nowState = GameState.ReadyToHitSentinel;
				m_TimeTrigger = Time.time;
			}
			else if (m_nowState == GameState.ReadyToHitSentinel && Input.touchCount > 0 && Time.time - m_TimeTrigger > 0.4) {
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(Input.touchCount - 1).position);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit)) {
					Debug.Log("tag:"+hit.collider.gameObject.tag);
					Debug.Log("name:"+hit.collider.gameObject.name);
					if (hit.collider.gameObject.tag == GameTag.Robot.ToString()) {
						GameObject cube = Instantiate(Resources.Load<GameObject>("Cube"));
						cube.transform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
						cube.transform.localPosition = m_MidPoint + new Vector3(0, 0.18f, 0);
						Instantiate(Resources.Load("Hit"));
					}
				}
				m_TimeTrigger = Time.time;
			}
		}
		private void Clear()
		{
			m_ResetButton.onClick.RemoveAllListeners();
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			Start();
		}
	}
}