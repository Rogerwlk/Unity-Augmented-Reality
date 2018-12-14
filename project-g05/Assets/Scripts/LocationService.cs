namespace YelpReview
{
    using System.Collections;
    using UnityEngine;

    public class LocationService : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(StartLocationService());
        }

        private void Update()
        {
        }

        private IEnumerator StartLocationService()
        {
            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("user has not enable GPS");
                yield break;
            }

            Input.location.Start();
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
            }

            if (maxWait <= 0)
            {
                Debug.Log("Time out");
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determine device location");
                yield break;
            }

            yield break;
        }
    }
}