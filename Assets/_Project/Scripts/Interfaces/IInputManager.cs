using System;
using UnityEngine;

namespace MobileGame.Interfaces
{
    /// <summary>
    /// 입력 관리 인터페이스
    /// 터치 및 모바일 입력 이벤트를 정의
    /// </summary>
    public interface IInputManager
    {
        // 터치 이벤트
        event Action<Vector2> OnTouchStarted;
        event Action<Vector2> OnTouchMoved;
        event Action<Vector2> OnTouchEnded;
        event Action<Vector2, Vector2> OnSwipe; // (시작 위치, 방향)

        // 입력 상태 조회
        Vector3 GetTouchWorldPosition(Camera camera = null);
        bool IsTouching();
        Vector2 GetTouchPosition();
    }
}
