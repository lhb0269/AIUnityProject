using UnityEngine;
using UnityEngine.UI;
using VContainer;
using MobileGame.Interfaces;

namespace MobileGame.UI
{
    /// <summary>
    /// 모든 팝업의 기본 클래스
    /// 공통 기능인 표시/숨김, 닫기 버튼, 모달 블로커를 제공합니다.
    /// DI를 통해 UIManager를 주입받습니다.
    /// </summary>
    public class BasePopup : MonoBehaviour
    {
        [Inject] protected IUIManager uiManager;
        #region 직렬화 필드

        [Header("팝업 기본 요소")]
        [SerializeField] protected Button closeButton;
        [SerializeField] protected Image blockerImage;

        #endregion

        #region Unity 생명주기

        /// <summary>
        /// 시작 시 닫기 버튼 이벤트 등록
        /// </summary>
        protected virtual void Start()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(OnCloseButtonClicked);
            }
            else
            {
                Debug.LogWarning($"[{GetType().Name}] closeButton이 할당되지 않았습니다.");
            }

            // 블로커 이미지의 RaycastTarget 확인
            if (blockerImage != null)
            {
                blockerImage.raycastTarget = true;
            }
            else
            {
                Debug.LogWarning($"[{GetType().Name}] blockerImage가 할당되지 않았습니다.");
            }
        }

        /// <summary>
        /// 파괴 시 이벤트 해제
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            }
        }

        #endregion

        #region 공개 메서드

        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);

            if (blockerImage != null)
            {
                blockerImage.gameObject.SetActive(true);
            }

            Debug.Log($"[{GetType().Name}] 팝업 표시");
        }

        /// <summary>
        /// 팝업을 숨깁니다.
        /// </summary>
        public virtual void Hide()
        {
            if (blockerImage != null)
            {
                blockerImage.gameObject.SetActive(false);
            }

            gameObject.SetActive(false);

            Debug.Log($"[{GetType().Name}] 팝업 숨김");
        }

        #endregion

        #region 보호된 메서드

        /// <summary>
        /// 닫기 버튼 클릭 시 호출되는 핸들러
        /// </summary>
        protected virtual void OnCloseButtonClicked()
        {
            Debug.Log($"[{GetType().Name}] 닫기 버튼 클릭");

            // DI로 주입된 UIManager를 통해 팝업 닫기
            if (uiManager != null)
            {
                uiManager.ClosePopup(this);
            }
            else
            {
                // UIManager가 주입되지 않은 경우 직접 숨김
                Hide();
                Debug.LogWarning($"[{GetType().Name}] UIManager가 주입되지 않았습니다.");
            }
        }

        #endregion
    }
}
