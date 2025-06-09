using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using _Project.Scripts.Interfaces;

namespace _Project.Scripts.UI
{
    public class SimpleJoystick : MonoBehaviour, IJoystick, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _handleImage;

        private CanvasGroup _canvasGroup;
        private Vector2 _inputVector = Vector2.zero;
        private float _backgroundRadius;

        float IJoystick.Horizontal => (_backgroundRadius > 0f) ? (_inputVector.x / _backgroundRadius) : 0f;
        float IJoystick.Vertical => (_backgroundRadius > 0f) ? (_inputVector.y / _backgroundRadius) : 0f;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void Start()
        {
            _backgroundRadius = _backgroundImage.rectTransform.rect.width * 0.5f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_backgroundImage.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
            
            Vector2 clamped = Vector2.ClampMagnitude(localPoint, _backgroundRadius);
            _inputVector = clamped;
            _handleImage.rectTransform.anchoredPosition = clamped;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _inputVector = Vector2.zero;
            _handleImage.rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}