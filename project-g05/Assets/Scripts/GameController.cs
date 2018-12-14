namespace YelpReview
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private Button addReviewButton;

        [SerializeField]
        private InputField reviewInputField;

        [SerializeField]
        private Canvas reviewCanvas;

        [SerializeField]
        private Button refreshButton;
        
        private const float moveThreshold = 80f; // threshold to determine slow or fast move
        private const string apiKey = "KSWgnL7BGYLq6w1pIBDvirPZGzG2I_0veAos0bjSDiYWmK_pRJx9jRG-Q4NKSDdct3YnnQi5jYboRavm4wibOxbL0uIv4XqkPQzEu2gl7awW8hnMwnJIisOmnQjeW3Yx";

        private GameObject reviewPrefab;
        private Vector3 randomPos;
        private Vector2 prevPos;
        private GameObject clickedObject = null;
        private int reviewIndex = -1;
        private bool findRestaurant = false;
        private List<Yelp.Api.Models.Review> reviews;
        private Dictionary<float, Sprite> rating;

        private void Start()
        {
            reviewPrefab = Resources.Load<GameObject>("Review");
            addReviewButton.onClick.AddListener(AddReview);
            refreshButton.onClick.AddListener(Refresh);
            reviews = new List<Yelp.Api.Models.Review>();
            rating = new Dictionary<float, Sprite>
            {
                { 0f, Resources.Load<Sprite>("Stars/stars0") },
                { 1f, Resources.Load<Sprite>("Stars/stars1") },
                { 1.5f, Resources.Load<Sprite>("Stars/stars1half") },
                { 2f, Resources.Load<Sprite>("Stars/stars2") },
                { 2.5f, Resources.Load<Sprite>("Stars/stars2half") },
                { 3f, Resources.Load<Sprite>("Stars/stars3") },
                { 3.5f, Resources.Load<Sprite>("Stars/stars3half") },
                { 4f, Resources.Load<Sprite>("Stars/stars4") },
                { 4.5f, Resources.Load<Sprite>("Stars/stars4half") },
                { 5f, Resources.Load<Sprite>("Stars/stars5") }
            };
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                if (!clickedObject)
                {
                    FindClickedObject();
                }
                if (clickedObject)
                {
                    Debug.Log("go into interaction");
                    InteractWithReview();
                }
            }
            else
            {
                clickedObject = null;
            }
        }

        private void FindClickedObject()
        {
            Vector2 mousePos = Input.mousePosition;
            prevPos = mousePos;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 50f));
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                clickedObject = hit.transform.gameObject;
                Debug.Log("hit something");
            }
        }

        public void InteractWithReview()
        {
            // 与显示出的n条评论词条进行touch互动:
            // 手指移动某词条；慢滑移动，快滑删除
            // 被遮挡词条暴露会靠前(等同于size放大)，删除词条会读取新的词条
            // 若x ,y 为斜线移动， 则词条有一定随机概率词条在z axis上被抛远
            Vector2 newPos = Input.mousePosition;

            // our moving fashion
            // if slow move, do transform, and overlap transform is applicable(*later)
            if (Vector2.Distance(newPos, prevPos) < moveThreshold)
            {
                clickedObject.transform.position += new Vector3(Input.GetTouch(0).deltaPosition.x, -Input.GetTouch(0).deltaPosition.y, 0f);
            }
            else // if fast move, replace clicked review with a new one
            {
                Destroy(clickedObject);
                SpawnReview(reviews[reviewIndex]);
            }
            prevPos = newPos;
        }

        private string WrapText(string text)
        {
            string newString = "";
            for (int i = 0; i < text.Length; i++)
            {
                newString += text[i];
                if (i != 0 && i % 20 == 0)
                {
                    newString += '\n';
                }
            }
            return newString;
        }

        private void SpawnReview(Yelp.Api.Models.Review newComment)
        {
            // instantiate review prefab as child of reviewCanvas
            GameObject reviewObj = Instantiate(reviewPrefab, Vector3.zero, Quaternion.Euler(180f, 0, 0), reviewCanvas.transform);
            // random a location on canvas
            RectTransform rect = reviewObj.GetComponent<RectTransform>();
            rect.anchoredPosition3D = Vector3.zero;
            float ran1 = Random.Range(0.2f, 0.8f);
            float ran2 = Random.Range(0.2f, 0.8f);
            rect.anchorMin = new Vector2(ran1, ran2);
            rect.anchorMax = new Vector2(ran1, ran2);

            // set new text
            TextMesh reviewText = reviewObj.GetComponentInChildren<TextMesh>();
            reviewText.text = newComment.User.Name + '\n' + WrapText(newComment.Text);
            // set new rating
            Image reviewImage = reviewObj.GetComponentInChildren<Image>();
            reviewImage.sprite = rating[newComment.Rating];
            reviewImage.color = Color.white;
            
            // add a tight collider
            BoxCollider collider = reviewObj.AddComponent<BoxCollider>();
            collider.size = new Vector3(140f, 190f, 0.1f);
            collider.center = new Vector3(0f, -30f, 0f);
            reviewIndex++;
            if (reviewIndex == reviews.Count)
                reviewIndex = 0;
        }

        private void AddReview()
        {
        	if (findRestaurant && reviewInputField.text != null)
        	{
                Yelp.Api.Models.User user = new Yelp.Api.Models.User
                {
                    Name = "Roger"
                };

                Yelp.Api.Models.Review review = new Yelp.Api.Models.Review
                {
                    Rating = 5,
                    Text = reviewInputField.text,
                    User = user
                };
                reviews.Add(review);
                SpawnReview(review);
                
        		reviewInputField.text = null;
        	}
        }

        private void ResetReviews()
        {
            reviews.Clear();
            reviewIndex = -1;
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Review");
            foreach (GameObject obj in objs)
                Destroy(obj);
        }

        private void Refresh()
        {
            ResetReviews();
            findRestaurant = true;
            FindRestaurantAsync();
        }

        private async void FindRestaurantAsync()
        {
            double latitude = Input.location.lastData.latitude;
            double longitude = Input.location.lastData.longitude;

            Debug.Log("latitude" + latitude);
            Debug.Log("longitude" + longitude);
            var businessResponse = await FindBusiness(latitude, longitude);

            GameObject shop = GameObject.FindGameObjectWithTag("ShopName");
            Text shopText = shop.GetComponent<Text>();
            string close = "Open";
            string address = businessResponse.Location.Address1;
            if (businessResponse.Location.Address2 != "")
                address += ", " + businessResponse.Location.Address2;
            if (businessResponse.IsClosed)
                close = "Closed";
            shopText.text = businessResponse.Name + '\n' + address + '\n' + businessResponse.DistanceAway.ToString("0.#") + "m away\n" + close;

            shop = GameObject.FindGameObjectWithTag("ShopRating");
            Image shopImage = shop.GetComponent<Image>();
            shopImage.sprite = rating[businessResponse.Rating];
            shopImage.color = Color.white;

            var reviewsResponse = await FindReview(businessResponse.Id);
            var temp = reviewsResponse.Reviews;
            for (int i = 0; i < temp.Length; i++)
                reviews.Add(temp[i]);
            if (temp.Length > 0)
            {
                reviewIndex++;
                SpawnReview(reviews[reviewIndex]);
            }
        }

        async Task<Yelp.Api.Models.BusinessResponse> FindBusiness(double longitude, double latitude)
        {
            var client = new Yelp.Api.Client(apiKey);

            // Advanced search
            var request = new Yelp.Api.Models.SearchRequest
            {
                SortBy = "distance",
                Latitude = longitude,
                //Term = "cupcakes",
                Longitude = latitude,
                MaxResults = 1,
                //OpenNow = true
            };
            
            var results = await client.SearchBusinessesAllAsync(request);

            return results.Businesses[0];
        }

        async Task<Yelp.Api.Models.ReviewsResponse> FindReview(string businessID)
        {
            var client = new Yelp.Api.Client(apiKey);
            
            var results = await client.GetReviewsAsync(businessID);
            return results;
        }
    }
}