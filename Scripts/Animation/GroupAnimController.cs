namespace AutoAnim.Scripts.Animation
{
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.UI;

    public class GroupAnimController : MonoBehaviour
    {
        [SerializeField]
        private float delay = 0;

        [SerializeField]
        private float delayPerRect = 0.05f;

        [SerializeField]
        private float duration = 0.4f;

        [SerializeField]
        private List<AnimationType> animationTypes = new() { AnimationType.Zoom };

        [SerializeField]
        private Ease ease = Ease.OutBack;

        [SerializeField, InlineButton("QuickReferences", Label = "", Icon = SdfIconType.LightningFill)]
        private List<Transform> customElements;

        [SerializeField]
        private bool playOnEnable = true;

        [SerializeField]
        private bool useLayoutRefresher;

        private readonly List<ElementInfo> infos = new();
        private Sequence sequence;
        private LayoutRefresher refresher;
        private bool[] isContain;

        private void Awake()
        {
            CheckContains();
            if (useLayoutRefresher)
                refresher = new LayoutRefresher(transform, false);
            if (customElements.Count > 0)
            {
                foreach (Transform child in customElements)
                {
                    AddElementInfo(child);
                }
                return;
            }
            foreach (Transform child in transform)
            {
                AddElementInfo(child);
            }
        }

        public void OnEnable()
        {
            if (!playOnEnable) return;
            PlayAppearAnimation();
        }

        public void QuickReferences()
        {
            customElements.Clear();
            foreach (Transform transform in transform)
            {
                customElements.Add(transform);
            }
        }

        private void AddElementInfo(Transform transform)
        {
            ElementInfo info = new ElementInfo
            {
                rect = transform.GetComponent<RectTransform>(),
            };
            info.originalPosition = info.rect.anchoredPosition;
            info.originalScale = info.rect.localScale;
            infos.Add(info);
            if (IsInUse(AnimationType.Fade))
            {
                if (transform.TryGetComponent(out CanvasGroup group))
                {
                    info.group = group;
                }
                else
                {
                    info.group = transform.gameObject.AddComponent<CanvasGroup>();
                }
            }
        }

        private void CheckContains()
        {
            isContain = new bool[System.Enum.GetNames(typeof(AnimationType)).Length];
            foreach (var type in animationTypes)
            {
                isContain[(int)type] = true;
            }
        }

        private bool IsInUse(AnimationType type) { return isContain[(int)type]; }

        public Sequence PlayAppearAnimation()
        {
            sequence?.Kill();
            sequence = DOTween.Sequence();
            foreach (var (info, index) in infos.WithIndex())
            {
                // Zoom
                if (IsInUse(AnimationType.Zoom) || (IsInUse(AnimationType.ZoomVertical) && IsInUse(AnimationType.ZoomHorizontal)))
                {
                    info.rect.localScale = Vector3.zero;
                }
                else if (IsInUse(AnimationType.ZoomHorizontal))
                {
                    info.rect.localScale = new Vector3(0, info.originalScale.y, 1);
                }
                else if (IsInUse(AnimationType.ZoomVertical))
                {
                    info.rect.localScale = new Vector3(info.originalScale.x, 0, 1);
                }
                if (IsInUse(AnimationType.Zoom) || IsInUse(AnimationType.ZoomHorizontal) || IsInUse(AnimationType.ZoomVertical))
                    sequence.Insert(delay + delayPerRect * index, info.rect.DOScale(info.originalScale, duration).SetEase(ease));

                // Fade
                if (IsInUse(AnimationType.Fade))
                {
                    info.group.alpha = 0;
                    sequence.Insert(delay + delayPerRect * index, info.group.DOFade(1, duration).SetEase(ease));
                }
            }
            sequence.SetUpdate(true);
            if (useLayoutRefresher)
            {
                sequence.OnUpdate(() =>
                {
                    refresher.Refresh();
                });
            }
            return sequence;
        }

        public class ElementInfo
        {
            public RectTransform rect;
            public CanvasGroup group;

            public Vector2 originalPosition;
            public Vector2 originalScale;
        }

        public enum AnimationType
        {
            Zoom,
            ZoomHorizontal,
            ZoomVertical,
            Fade,
        }
    }
    
    public class LayoutRefresher
    {
        private readonly bool                isNoneRecusively = false;
        private readonly List<LayoutElement> layoutElements   = new List<LayoutElement>();

        public LayoutRefresher(Transform root, bool isNoneRecusively)
        {
            this.isNoneRecusively = isNoneRecusively;
            AddLayoutElements((RectTransform) root);
        }

        public void Refresh()
        {
            foreach (LayoutElement element in layoutElements)
            {
                if (element.LayoutGroup != null)
                {
                    element.LayoutGroup.SetLayoutHorizontal();
                    element.LayoutGroup.SetLayoutVertical();
                }

                if (element.ContentSizeFitter != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(element.RectTransform);
                }
            }
        }

        public void AddLayoutElements(RectTransform rectTransform)
        {
            if (rectTransform == null)
            {
                return;
            }

            if (!isNoneRecusively)
            {
                foreach (RectTransform child in rectTransform)
                {
                    AddLayoutElements(child);
                }
            }

            LayoutElement layoutElement = new LayoutElement
            {
                RectTransform     = rectTransform,
                ContentSizeFitter = rectTransform.GetComponent<ContentSizeFitter>(),
                LayoutGroup       = rectTransform.GetComponent<LayoutGroup>()
            };

            if (layoutElement.LayoutGroup != null || layoutElement.ContentSizeFitter != null)
            {
                layoutElements.Add(layoutElement);
            }
        }
    }

    public class LayoutElement
    {
        public RectTransform     RectTransform     { get; set; }
        public LayoutGroup       LayoutGroup       { get; set; }
        public ContentSizeFitter ContentSizeFitter { get; set; }
    }
    
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));
    }
}