using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Logic;
using Game.Utils;
using UnityEngine;

namespace Game.Managers
{
    public class ItemsManager : Singleton<ItemsManager>
    {
        private UIManager _uiManager;
        private GameManager _gameManager;

        private List<DraggableItem> _lastDraggingItems;
        private Vector3 _lastItemPosition;
        private bool _isNew;

        protected override void Initialize()
        {
            _uiManager = UIManager.Instance;
            _gameManager = GameManager.Instance;
        }

        public bool CanDrag(DraggableItem item)
        {
            return !DOTween.IsTweening(item.RectTransform);
        }

        public void StartDrag(DraggableItem item)
        {
            _uiManager.SetStatus("drag");

            _isNew = !_gameManager.Items.Contains(item);
            if (!_isNew)
            {
                var itemIndex = _gameManager.Items.IndexOf(item);
                _lastDraggingItems = _gameManager.Items.GetRange(itemIndex, _gameManager.Items.Count - itemIndex);
                _gameManager.Items.RemoveRange(itemIndex, _gameManager.Items.Count - itemIndex);

                _lastItemPosition = item.RectTransform.anchoredPosition;
            }

            item.RectTransform.SetParent(_uiManager.Overlay);
        }

        public void EndDrag(DraggableItem item)
        {
            if (CanDropOnBoard(item))
            {
                _uiManager.SetStatus("drop_on_board");
                DropOnBoard(item, true);
                ResetState();
                return;
            }

            if (CanDropInHole(item))
            {
                _uiManager.SetStatus("drop_in_hole");
                DropInHole(item);
                ResetState();
                return;
            }

            if (!_isNew)
            {
                _uiManager.SetStatus("drop_back");
                DropOnBoard(item, false);
                ResetState();
                return;
            }

            _uiManager.SetStatus("destroy");
            DestroyItem(item, true);
            ResetState();
        }

        private bool CanDropOnBoard(DraggableItem item)
        {
            if (!item.IsItemInside(_uiManager.RightPart))
                return false;

            if (_gameManager.Items.Count > 0)
            {
                var lastRectTransform = _gameManager.Items.Last().RectTransform;
                var localPoint = lastRectTransform.InverseTransformPoint(item.RectTransform.position);

                if (localPoint.y < lastRectTransform.rect.height / 2)
                {
                    return false;
                }

                if (localPoint.x < -lastRectTransform.rect.width / 2 || localPoint.x > lastRectTransform.rect.width / 2)
                {
                    return false;
                }
            }

            return true;
        }

        private void DropOnBoard(DraggableItem item, bool useTargetPosition)
        {
            if (_gameManager.Items.Count > 0)
            {
                var lastRectTransform = _gameManager.Items.Last().RectTransform;

                item.RectTransform.SetParent(lastRectTransform);
                if (useTargetPosition)
                {
                    var targetPosition = new Vector2(
                        lastRectTransform.InverseTransformPoint(item.RectTransform.position).x,
                        lastRectTransform.rect.height / 2 + item.RectTransform.rect.height / 2);
                    item.RectTransform.DOLocalMove(targetPosition, 0.2f);
                }
                else
                {
                    item.RectTransform.DOAnchorPos(_lastItemPosition, 0.2f);
                }
            }
            else if (!useTargetPosition)
            {
                item.RectTransform.DOAnchorPos(_lastItemPosition, 0.2f);
            }

            if (_lastDraggingItems != null)
            {
                _gameManager.Items.AddRange(_lastDraggingItems);
            }
            else
            {
                _gameManager.Items.Add(item);
            }
        }

        private bool CanDropInHole(DraggableItem item)
        {
            return item.RectTransform.IsObjectInsideHole(_uiManager.HoleRectTransform);
        }

        private void DropInHole(DraggableItem item)
        {
            if (_isNew)
            {
                SetAnimation(item, 0);
            }
            else
            {
                for (var i = _lastDraggingItems.Count - 1; i >= 0; i--)
                {
                    _lastDraggingItems[i].RectTransform.SetParent(_uiManager.Overlay);
                    _lastDraggingItems[i].RectTransform.SetAsLastSibling();
                    SetAnimation(_lastDraggingItems[i], 0.15f * (_lastDraggingItems.Count - 1 - i));
                }
            }

            return;


            void SetAnimation(DraggableItem tempItem, float delay)
            {
                var targetPosition = _uiManager.HoleRectTransform.position;
                DOTween.Sequence()
                    .Join(tempItem.RectTransform.DOScale(1.1f, 0.1f))
                    .Append(tempItem.RectTransform.DOScale(0f, 0.4f))
                    .Insert(0, tempItem.RectTransform.DOJump(targetPosition, 150f, 1, 0.5f))
                    .SetDelay(delay)
                    .OnComplete(() => DestroyItem(tempItem, false));
            }
        }

        private void DestroyItem(DraggableItem item, bool withAnimation)
        {
            if (withAnimation)
            {
                item.RectTransform.DOScale(0, 0.4f).OnComplete(() => Destroy(item.gameObject));
            }
            else
            {
                Destroy(item.gameObject);
            }
        }

        private void ResetState()
        {
            _lastDraggingItems = null;
            _lastItemPosition = Vector3.zero;
        }
    }
}