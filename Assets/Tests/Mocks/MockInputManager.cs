using System;
using UnityEngine;
using MobileGame.Managers;

namespace MobileGame.Tests.Mocks
{
    /// <summary>
    /// 테스트용 InputManager Mock 클래스
    /// IInputManager 인터페이스를 구현하여 테스트 격리 제공
    /// </summary>
    public class MockInputManager : IInputManager
    {
        // 터치 이벤트
        public event Action<Vector2> OnTouchStarted;
        public event Action<Vector2> OnTouchMoved;
        public event Action<Vector2> OnTouchEnded;
        public event Action<Vector2> OnSwipe;

        // 현재 터치 상태
        public bool IsTouchingState { get; set; } = false;
        public Vector2 CurrentTouchPosition { get; set; } = Vector2.zero;

        // 이벤트 발생 카운트
        public int TouchStartedCount { get; private set; } = 0;
        public int TouchMovedCount { get; private set; } = 0;
        public int TouchEndedCount { get; private set; } = 0;
        public int SwipeCount { get; private set; } = 0;

        public Vector2 GetTouchWorldPosition(Camera camera = null)
        {
            // 테스트용 가짜 월드 좌표 반환
            return CurrentTouchPosition;
        }

        public bool IsTouching()
        {
            return IsTouchingState;
        }

        public Vector2 GetTouchPosition()
        {
            return CurrentTouchPosition;
        }

        /// <summary>
        /// 테스트용 터치 시작 시뮬레이션
        /// </summary>
        public void SimulateTouchStarted(Vector2 position)
        {
            CurrentTouchPosition = position;
            IsTouchingState = true;
            TouchStartedCount++;
            OnTouchStarted?.Invoke(position);
            Debug.Log($"[MockInputManager] SimulateTouchStarted: {position}");
        }

        /// <summary>
        /// 테스트용 터치 이동 시뮬레이션
        /// </summary>
        public void SimulateTouchMoved(Vector2 position)
        {
            CurrentTouchPosition = position;
            TouchMovedCount++;
            OnTouchMoved?.Invoke(position);
            Debug.Log($"[MockInputManager] SimulateTouchMoved: {position}");
        }

        /// <summary>
        /// 테스트용 터치 종료 시뮬레이션
        /// </summary>
        public void SimulateTouchEnded(Vector2 position)
        {
            CurrentTouchPosition = position;
            IsTouchingState = false;
            TouchEndedCount++;
            OnTouchEnded?.Invoke(position);
            Debug.Log($"[MockInputManager] SimulateTouchEnded: {position}");
        }

        /// <summary>
        /// 테스트용 스와이프 시뮬레이션
        /// </summary>
        public void SimulateSwipe(Vector2 swipeDirection)
        {
            SwipeCount++;
            OnSwipe?.Invoke(swipeDirection);
            Debug.Log($"[MockInputManager] SimulateSwipe: {swipeDirection}");
        }

        /// <summary>
        /// 테스트 초기화 - 모든 기록 삭제
        /// </summary>
        public void Reset()
        {
            IsTouchingState = false;
            CurrentTouchPosition = Vector2.zero;
            TouchStartedCount = 0;
            TouchMovedCount = 0;
            TouchEndedCount = 0;
            SwipeCount = 0;
            OnTouchStarted = null;
            OnTouchMoved = null;
            OnTouchEnded = null;
            OnSwipe = null;
        }
    }
}
