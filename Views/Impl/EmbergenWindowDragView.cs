using UnityEngine;
using UnityEngine.EventSystems;

namespace PortfolioViewer.Views.Impl
{
    public class EmbergenWindowDragView : ViewBase, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        //Events
        public event System.Action<Vector2> DragStarted;
        public event System.Action<Vector2> DragEnded;
        public event System.Action<Vector2> Dragged;
        
        [SerializeField] private float dragSpeed = 1f;
        [SerializeField] private Vector2 minDragPosition;
        [SerializeField] private Vector2 maxDragPosition;
        
        public float DragSpeed => dragSpeed;
        public Vector2 MinDragPosition => minDragPosition;
        public Vector2 MaxDragPosition => maxDragPosition;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            DragStarted?.Invoke(eventData.position);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            Dragged?.Invoke(eventData.position);
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            DragEnded?.Invoke(eventData.position);
        }
    }
}