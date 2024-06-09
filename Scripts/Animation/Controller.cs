namespace AutoAnim.Scripts.Animation
{
    using System.Collections;
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;

    public enum IdleType
    {
        NoUse = 0,
        Zoom = 1,
        Waggle = 2,
        Spin = 3,
        Jelly = 4
    }
    
    public enum TerminalType
    {
        NoUse  = 0,
        Zoom  = 1,
        Fade  = 2,
        Jelly = 3
    }
    
    public enum PressType
    {
        NoUse   = 0,
        Zoom   = 1,
        Waggle = 2,
        Spin   = 3,
        Fade   = 4,
        Jelly  = 5
    }

    public enum State
    {
        Open = 0,
        Idle = 1,
        Press = 2,
        Close = 3
    }
    
    public class Controller : MonoBehaviour
    {
        #region General settings
        
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("General", centerLabel: true)]
        [Tooltip("Time delay at the start")]
        protected float startDelay;
        
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("General", centerLabel: true), Space(5)]
        [Tooltip("Use system timescale or not")] 
        protected bool unscaleTime;
        
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("General", centerLabel: true), Space(5)]
        [Tooltip("Customize transform's standard local scale")]
        protected bool customTransform;
        
        [ShowIf("CustomTransform")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("General/Custom Transform", showLabel: false, centerLabel: true)]
        protected Vector3 scale = new(1, 1, 1);
        
        [ShowIf("CustomTransform")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("General/Custom Transform", showLabel: false, centerLabel: true)]
        protected Vector3 rotation = new(0, 0, 0);

        public bool CustomTransform => customTransform;
        
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("General", centerLabel: true), Space(5)]
        [Tooltip("Manually drag or automatically get components in the script")] 
        protected bool manualSetUp;
        
        [ShowIf("ManualSetUp")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("General/Components", showLabel: false, centerLabel: true)]
        [Tooltip("Rect transform of the game object")] 
        [LabelText("Transform")]
        protected RectTransform trans;

        [FormerlySerializedAs("group")]
        [ShowIf("ManualSetUp")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("General/Components", showLabel: false, centerLabel: true)]
        [Tooltip("Canvas group of the game object")] 
        protected CanvasGroup canvasGroup;

        public bool ManualSetUp => manualSetUp;

        public RectTransform Transform
        {
            get => trans;
            set => trans = value;
        }
        
        public CanvasGroup CanvasGroup
        {
            get => canvasGroup;
            set => canvasGroup = value;
        }

        #endregion

        #region Appear Animation

        [SerializeField, GUIColor(1, 1, 1, 1),BoxGroup("Phase", centerLabel: true), TabGroup("Phase/State", "Appear")]
        [Tooltip("Animation style")] 
        [LabelText("Type")]
        protected TerminalType aType;
        
        
        [ShowIf("UseAppearAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Appear/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Animation playback time")] 
        [LabelText("Duration")]
        protected float aDuration = 0.3f;
        
        
        [ShowIf("UseAppearAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1),BoxGroup("Phase/State/Appear/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Animation ease chart")] 
        [LabelText("Ease")]
        protected Ease aEase = Ease.OutBack;

        [ShowIf("UseAppearAnim")] 
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Appear/Setting", showLabel: false, centerLabel: true)] 
        [Tooltip("Animation ease chart")] 
        [LabelText("Callback")]
        protected UnityEvent aCallBack;
        
        public bool UseAppearAnim => aType != TerminalType.NoUse;
        
        #endregion
        
        #region Disappear Animation

        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase"), TabGroup("Phase/State", "Disappear")]
        [Tooltip("Animation style")] 
        [LabelText("Type")]
        protected TerminalType daType = 0;
        
        
        [ShowIf("UseDisAppearAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Disappear/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Animation playback time")] 
        [LabelText("Duration")]
        protected float daDuration = 0.3f;
        
        
        [ShowIf("UseDisAppearAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Disappear/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Animation ease chart")] 
        [LabelText("Ease")]
        protected Ease daEase = Ease.InOutSine;
        
        public bool UseDisAppearAnim => daType != TerminalType.NoUse;
        
        #endregion

        #region Idle Animation

        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase"), TabGroup("Phase/State", "Idle")]
        [Tooltip("Animation style")] 
        [LabelText("Type")]
        protected IdleType iType;
        
        [ShowIf("UseIdleAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Idle/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Magnitude position")] 
        [LabelText("Position Offset")]
        protected Vector3 posChange;
        
        [ShowIf("UseIdleAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Idle/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Magnitude rotation")] 
        [LabelText("Rotation Offset")]
        protected Vector3 rotChange;
        
        [FormerlySerializedAs("lDuration")]
        [ShowIf("UseIdleAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Idle/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Animation playback time")] 
        [LabelText("Duration")]
        protected float iDuration = 0.5f;
        
        [FormerlySerializedAs("lLoopPerPhase")]
        [ShowIf("UseIdleAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Idle/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Number of loops per animation phase")] 
        [LabelText("Turn per phase")]
        protected int iLoopPerPhase = 1;
        
        [FormerlySerializedAs("lDelayPerPhase")]
        [ShowIf("UseIdleAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Idle/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Delay time between two phases")] 
        [LabelText("Delay per phase")]
        protected float iDelayPerPhase = 0.5f;
        
        [FormerlySerializedAs("lEase")]
        [ShowIf("UseIdleAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Idle/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Animation ease chart")] 
        [LabelText("Ease")]
        protected Ease iEase = Ease.InOutSine;

        public bool UseIdleAnim => iType != IdleType.NoUse;
        
        #endregion

        // ---------------------------------------------------------------------------------------------------------------------------------------- //

        
        
        
        #region Local Variable
        
        //Bool

        protected bool isInitialized;
        
        
        //Int
        
        protected int loopCounter;
        
        // Float

        protected float delayPerPhaseLoop;
        
        
        //Wait for seconds
        
        protected WaitForSeconds startDelayWait;
        
        
        //DOTween
        
        protected Tweener tween;
        
        protected Sequence sequence;
        
        public Controller() { aType = 0; }

        #endregion

        protected void Start()
        {
            if (trans == null)
            {
                if (!TryGetComponent(out trans))
                {
                    Transform = gameObject.AddComponent<RectTransform>();
                }
            }
            
            if (canvasGroup == null)
            {
                if (!TryGetComponent(out canvasGroup))
                {
                    CanvasGroup = gameObject.AddComponent<CanvasGroup>();
                } 
            }   
            
            if (!customTransform)
            {
                scale    = Transform.localScale;
                rotation = Transform.rotation.eulerAngles;
            }
            else
            {
                Transform.localScale    = scale;
                Transform.localRotation = Quaternion.Euler(rotation);
            }
            
            if (startDelay > 0)
            {
                startDelayWait = new WaitForSeconds(startDelay);
            }

            if (UseAppearAnim)
            {
                switch (aType)
                {
                    case TerminalType.Zoom:
                        Transform.localScale    = Vector3.zero;
                        Transform.localRotation = CustomTransform ? Quaternion.Euler(rotation) : Quaternion.identity;
                        canvasGroup.alpha       = 1;
                        break;
                    case TerminalType.Fade:
                    case TerminalType.Jelly:
                    case TerminalType.NoUse:
                        Transform.localScale    = scale;
                        Transform.localRotation = CustomTransform ? Quaternion.Euler(rotation) : Quaternion.identity;
                        canvasGroup.alpha       = 0;
                        break;
                }
            }
            
            isInitialized = true;
        }

        protected void OnEnable()
        {
            StartCoroutine(EnableState());
        }

        protected IEnumerator EnableState()
        {
            if (!isInitialized)
            {
                yield return new WaitForEndOfFrame();
                
                yield return new WaitUntil(() => isInitialized);   
            }
            
            Active();
        }
        
        public void Active()
        {
            if (UseAppearAnim)
            {
                StartCoroutine(PlayAppear());
            }
            else if(UseIdleAnim)
            {
                PlayLoop();
            }
        }

        public void DeActive(bool deActiveGameObject)
        {
            if(!gameObject.activeInHierarchy) return;
            
            StartCoroutine(PlayDisAppear(deActiveGameObject));
        }
        
        protected IEnumerator PlayAppear()
        {
            canvasGroup.interactable = false;

            yield return startDelayWait;
            
            switch (aType)
            {
                case TerminalType.Zoom:
                    tween = trans.DOScale(scale, aDuration).SetUpdate(unscaleTime).SetEase(aEase);

                    yield return tween.WaitForCompletion();
                    break;
                case TerminalType.Fade:
                    tween = canvasGroup.DOFade(1, aDuration).SetUpdate(unscaleTime).SetEase(aEase);

                    yield return tween.WaitForCompletion();
                    break;
            }
            
            canvasGroup.interactable = true;

            if (UseIdleAnim)
            {
                PlayLoop();
            }
        }

        protected IEnumerator PlayDisAppear(bool deActiveGameObject)
        {
            canvasGroup.interactable = false;
            
            switch (daType)
            {
                case TerminalType.Zoom:
                    tween = trans.DOScale(Vector3.zero, daDuration).SetUpdate(unscaleTime).SetEase(daEase);

                    yield return tween.WaitForCompletion();
                    break;
                case TerminalType.Fade:
                    tween = canvasGroup.DOFade(0, daDuration).SetUpdate(unscaleTime).SetEase(daEase);

                    yield return tween.WaitForCompletion();
                    break;
            }

            if (deActiveGameObject)
            {
                gameObject.SetActive(false);
            }
        }
        
        protected void PlayLoop()
        {
            switch (iType)
            {
                case IdleType.Zoom:
                    ZoomInZoomOut();
                    break;
                case IdleType.Waggle:
                    RotateInOutBack();
                    break;
                case IdleType.Spin:
                    SpinAround();
                    break;
            }
        }

        
        protected void ZoomInZoomOut()
        {
            var target = new Vector3(scale.x + posChange.x, scale.y + posChange.y, 0);
            
            sequence = DOTween.Sequence();

            sequence.Pause();

            sequence.SetAutoKill(false);

            sequence.SetDelay(iDelayPerPhase);
            
            //sequence.Append(trans.DOScale(scale, 0.05f).SetEase(Ease.InOutSine));
            
            sequence.Append(trans.DOScale(target, iDuration / 4).SetUpdate(unscaleTime).SetEase(iEase));

            sequence.Append(trans.DOScale(scale, iDuration / 4).SetUpdate(unscaleTime).SetEase(iEase));

            sequence.Append(trans.DOScale(target, iDuration / 4).SetUpdate(unscaleTime).SetEase(iEase));

            sequence.Append(trans.DOScale(scale, iDuration / 4).SetUpdate(unscaleTime).SetEase(iEase))
                .OnComplete(() =>
                {
                    StartCoroutine(CheckSequence());
                });
            
            sequence.Play();
        }
        
        protected void RotateInOutBack()
        {
            var target1 = new Vector3(rotation.x, rotation.y, rotation.z + rotChange.z);
            
            var target2 = new Vector3(rotation.x, rotation.y, rotation.z - rotChange.z);
            
            sequence = DOTween.Sequence();
            
            sequence.Pause();

            sequence.SetAutoKill(false);

            //sequence.SetDelay(0);

            //sequence.Append(trans.DOLocalRotate(rotation, delayPerPhaseLoop).SetUpdate(!useTimeScale).SetEase(Ease.Linear));
            
            sequence.Append(trans.DOLocalRotate(target1, iDuration / 4).SetUpdate(unscaleTime).SetEase(Ease.Linear));

            sequence.Append(trans.DOLocalRotate(target2, iDuration / 2).SetUpdate(unscaleTime).SetEase(Ease.Linear));
            
            sequence.Append(trans.DOLocalRotate(rotation, iDuration / 4).SetUpdate(unscaleTime).SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    StartCoroutine(CheckSequence());
                });
            
            sequence.Play();  
        }

        protected void SpinAround()
        {
            sequence = DOTween.Sequence();
            
            sequence.Pause();

            sequence.SetAutoKill(false);

            //sequence.SetDelay(0);

            //sequence.Append(trans.DOLocalRotate(rotation, delayPerPhaseLoop).SetUpdate(useTimeScale).SetEase(Ease.Linear));
            
            sequence.Append(trans.DOLocalRotate(new Vector3(0,0,360f), iDuration / 4).SetRelative(true).SetUpdate(unscaleTime).SetEase(Ease.Linear)).OnComplete(() =>
            {
                StartCoroutine(CheckSequence());
            });
            
            sequence.Play();  
        }
        
        protected IEnumerator CheckSequence()
        {
            if (iLoopPerPhase > 0)
            {
                loopCounter++;
            
                if (loopCounter >= iLoopPerPhase)
                {
                    loopCounter = 0;

                    delayPerPhaseLoop = iDelayPerPhase;
                }
                else
                {
                    if (delayPerPhaseLoop != 0x0)
                    {
                        delayPerPhaseLoop = 0;   
                    }
                }

                yield return new WaitForSeconds(delayPerPhaseLoop);
            }
            
            sequence.Restart();
        }
        
        protected void OnDisable()
        {
            loopCounter = iLoopPerPhase;
            
            StopAllCoroutines();

            trans.DOKill();   
            
            sequence.Kill();
            
            if (UseAppearAnim)
            {
                switch (aType)
                {
                    case TerminalType.Zoom:
                        trans.localScale = Vector3.zero;

                        canvasGroup.alpha = 1;
                        break;
                    case TerminalType.Fade:
                        trans.localScale = scale;

                        canvasGroup.alpha = 0;
                        break;
                }
            }
            
            trans.localRotation = Quaternion.Euler(rotation);
        }
    }
}
