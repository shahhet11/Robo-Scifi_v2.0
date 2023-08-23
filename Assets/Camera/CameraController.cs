using UnityEngine;


    public class CameraController : MonoBehaviour
    {
        public GameObject MyPlayer;
        public float MaxRange = 3f;
        public float Speed = 3f;

        private Vector3 _initialPosition;
        public static CameraController CamContr;
        private void Awake()
        {
            CamContr = this;
        }
        void Start()
        {
            _initialPosition = transform.position;
        }

        void Update()
        {
            //var player = ObjectLocator.GameManager.Player;
            var player = MyPlayer;
            var targetOffset = Vector3.zero;

            if(player != null)
            {
                targetOffset = new Vector3(
                    Mathf.Clamp01(Mathf.Abs(player.transform.position.x) / MaxRange) * Mathf.Sign(player.transform.position.x) * MaxRange,
                    Mathf.Clamp01(Mathf.Abs(player.transform.position.y) / MaxRange) * Mathf.Sign(player.transform.position.y) * MaxRange,
                    Mathf.Clamp01(Mathf.Abs(player.transform.position.z) / MaxRange) * Mathf.Sign(player.transform.position.z) * MaxRange
                );
            }

            transform.position = Vector3.Lerp(
                transform.position,
                _initialPosition + targetOffset,
                Speed * Time.deltaTime
            );
        }
    
}