using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace MobileGame.Managers
{
    /// <summary>
    /// 입력 처리를 관리하는 매니저
    /// 터치 및 모바일 입력에 최적화
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        [Header("터치 설정")]
        [SerializeField] private float touchSensitivity = 1f;
        [SerializeField] private float swipeThreshold = 50f;

        // 터치 이벤트
        public event Action<Vector2> OnTouchStarted;
        public event Action<Vector2> OnTouchMoved;
        public event Action<Vector2> OnTouchEnded;
        public event Action<Vector2, Vector2> OnSwipe; // (시작 위치, 방향)

        private Vector2 touchStartPosition;
        private Vector2 touchCurrentPosition;
        private bool isTouching;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            HandleTouchInput();
        }

        /// <summary>
        /// 터치 입력 처리
        /// </summary>
        private void HandleTouchInput()
        {
            // 모바일 터치
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

                if (!isTouching)
                {
                    // 터치 시작
                    isTouching = true;
                    touchStartPosition = touchPosition;
                    touchCurrentPosition = touchPosition;
                    OnTouchStarted?.Invoke(touchPosition);
                }
                else
                {
                    // 터치 이동
                    touchCurrentPosition = touchPosition;
                    OnTouchMoved?.Invoke(touchPosition);
                }
            }
            // 마우스 입력 (에디터 테스트용)
            else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();

                if (!isTouching)
                {
                    isTouching = true;
                    touchStartPosition = mousePosition;
                    touchCurrentPosition = mousePosition;
                    OnTouchStarted?.Invoke(mousePosition);
                }
                else
                {
                    touchCurrentPosition = mousePosition;
                    OnTouchMoved?.Invoke(mousePosition);
                }
            }
            // 터치 종료
            else if (isTouching)
            {
                isTouching = false;
                OnTouchEnded?.Invoke(touchCurrentPosition);

                // 스와이프 감지
                DetectSwipe();
            }
        }

        /// <summary>
        /// 스와이프 감지
        /// </summary>
        private void DetectSwipe()
        {
            Vector2 swipeVector = touchCurrentPosition - touchStartPosition;
            float swipeDistance = swipeVector.magnitude;

            if (swipeDistance >= swipeThreshold)
            {
                Vector2 swipeDirection = swipeVector.normalized;
                OnSwipe?.Invoke(touchStartPosition, swipeDirection);

                Debug.Log($"[InputManager] 스와이프 감지: 방향 {swipeDirection}, 거리 {swipeDistance}");
            }
        }

        /// <summary>
        /// 월드 좌표로 변환된 터치 위치 가져오기
        /// </summary>
        public Vector3 GetTouchWorldPosition(Camera camera = null)
        {
            if (camera == null)
                camera = Camera.main;

            if (camera == null)
            {
                Debug.LogError("[InputManager] 카메라를 찾을 수 없습니다.");
                return Vector3.zero;
            }

            Vector2 screenPosition = isTouching ? touchCurrentPosition : Vector2.zero;
            return camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, camera.nearClipPlane));
        }

        /// <summary>
        /// 현재 터치 중인지 확인
        /// </summary>
        public bool IsTouching() => isTouching;

        /// <summary>
        /// 현재 터치 위치 가져오기
        /// </summary>
        public Vector2 GetTouchPosition() => touchCurrentPosition;

        /// <summary>
        /// 터치 시작 위치 가져오기
        /// </summary>
        public Vector2 GetTouchStartPosition() => touchStartPosition;
    }
}
