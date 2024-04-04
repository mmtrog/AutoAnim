using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Minh_Trong.Scripts.UIAutoAnimation
{
    public enum LoopType
    {
        ZoomInZoomOut = 0,
        RotateInOutBack = 1,
    }
    
    public enum ChangeStatusType
    {
        Zoom = 0,
        Fade = 1
    }
    
    public class UIAutoAnim : MonoBehaviour
    {
        [Space(5)] [Header("GENERAL")] 
        
        [Space(5)]
        [SerializeField] [Tooltip("Time delay at the start")]
        private float startDelay = 0.3f;
        
        [Space(5)] 
        [SerializeField] [Tooltip("Customize transform's standard local scale")]
        private bool customTransform;
        
        [SerializeField] 
        private Vector3 scale = new Vector3(1, 1, 0);
        
        [SerializeField] 
        private Vector3 rotation = new Vector3(0, 0, 0);
        
        [Space(5)]
        [SerializeField] [Tooltip("Use system timescale or not")] 
        private bool useTimeScale = true;
        
        
        
        [Space(5)] [Header("APPEAR")] [Space(15)] 
        
        [SerializeField] [Tooltip("Use animation when appearing")] 
        private bool useAppearAnim;

        [SerializeField] [Tooltip("Animation style")] 
        private ChangeStatusType A_Type = 0;
        
        [SerializeField] [Tooltip("Animation playback time")] 
        private float A_Duration = 0.3f;
        
        [SerializeField] [Tooltip("Animation ease chart")] 
        private Ease A_Ease = Ease.OutBack;
        
        
        
        [Space(5)] [Header("DISAPPEAR")] [Space(15)] 
        
        [SerializeField] [Tooltip("Use animation when disappearing")] 
        private bool useDisAppearAnim;
        
        [SerializeField] [Tooltip("Animation style")] 
        private ChangeStatusType DA_Type = 0;
        
        [SerializeField] [Tooltip("Animation playback time")] 
        private float DA_Duration = 0.3f;
        
        [SerializeField] [Tooltip("Animation ease chart")] 
        private Ease DA_Ease = Ease.InOutSine;
        
        
        
        [Space(5)] [Header("LOOP")] [Space(15)] 
        
        [SerializeField] [Tooltip("Use animation when looping")] 
        private bool useLoopAnim;
        
        [SerializeField] [Tooltip("Animation style")] 
        private LoopType L_Type;
        
        [SerializeField] [Tooltip("Magnitude position")] 
        private Vector3 posChange;
        
        [SerializeField] [Tooltip("Magnitude rotation")] 
        private Vector3 rotChange;
        
        [SerializeField] [Tooltip("Animation playback time")] 
        private float L_Duration = 0.5f;

        [SerializeField] [Tooltip("Number of loops per animation phase")] 
        private int L_LoopPerPhase = 1;
        
        [SerializeField] [Tooltip("Delay time between two phases")] 
        private float L_DelayPerPhase = 0.5f;
        
        [SerializeField] [Tooltip("Animation ease chart")] 
        private Ease L_Ease = Ease.InOutSine;
        
        
        
        [Space(5)] [Header("REQUIRED COMPONENTS")] [Space(15)] 
        
        [SerializeField] [Tooltip("Manually drag or automatically get components in the script")] 
        private bool manualSetUp = true;
        
        [SerializeField] [Tooltip("Rect transform of the game object")] 
        private RectTransform trans;

        [SerializeField] [Tooltip("Canvas group of the game object")] 
        private CanvasGroup group;
        
        // ---------------------------------------------------------------------------------------------------------------------------------------- //

        #region Local Variable
        
        //Bool

        private bool isSetUp;
        
        
        //Int
        
        private int loopCounter;
        
        // Float

        private float delayPerPhaseLoop;
        
        
        //Wait for seconds
        
        private WaitForSeconds startDelayWait;
        
        
        //DOTween
        
        private Tweener tween;
        
        private Sequence sequence;

        #endregion

        private void Start()
        {
            isSetUp = false;
            
            if (trans == null)
            {
                if (!TryGetComponent(out trans)) return;
            }
            
            if (group == null)
            {
                if (!TryGetComponent(out group))
                {
                    gameObject.AddComponent<CanvasGroup>();
                    
                    if (!TryGetComponent(out group)) return;
                } 
            }   
            
            if (!customTransform)
            {
                scale = trans.localScale;
                rotation = trans.rotation.eulerAngles;
            }
            else
            {
                trans.localScale = scale;
            }
            
            if (startDelay > 0)
            {
                startDelayWait = new WaitForSeconds(startDelay);
            }

            if (useAppearAnim)
            {
                switch (A_Type)
                {
                    case ChangeStatusType.Zoom:
                        trans.localScale = Vector3.zero;

                        group.alpha = 1;
                        break;
                    case ChangeStatusType.Fade:
                        trans.localScale = scale;

                        group.alpha = 0;
                        break;
                }
            }
            
            isSetUp = true;
        }

        private void OnEnable()
        {
            StartCoroutine(EnableState());
        }

        private IEnumerator EnableState()
        {
            if (!isSetUp)
            {
                yield return new WaitForEndOfFrame();
                
                yield return new WaitUntil(() => isSetUp);   
            }
            
            Active();
        }
        
        public void Active()
        {
            if (useAppearAnim)
            {
                StartCoroutine(PlayAppear());
            }
            else if(useLoopAnim)
            {
                PlayLoop();
            }
        }

        public void DeActive(bool deActiveGameObject)
        {
            StartCoroutine(PlayDisAppear(deActiveGameObject));
        }
        
        private IEnumerator PlayAppear()
        {
            group.interactable = false;

            yield return startDelayWait;
            
            switch (A_Type)
            {
                case ChangeStatusType.Zoom:
                    tween = trans.DOScale(scale, A_Duration).SetUpdate(!useTimeScale).SetEase(A_Ease);

                    yield return tween.WaitForCompletion();
                    break;
                case ChangeStatusType.Fade:
                    tween = group.DOFade(1, A_Duration).SetUpdate(!useTimeScale).SetEase(A_Ease);

                    yield return tween.WaitForCompletion();
                    break;
            }
            
            group.interactable = true;

            if (useLoopAnim)
            {
                PlayLoop();
            }
        }

        private IEnumerator PlayDisAppear(bool deActiveGameObject)
        {
            group.interactable = false;
            
            switch (DA_Type)
            {
                case ChangeStatusType.Zoom:
                    tween = trans.DOScale(Vector3.zero, DA_Duration).SetUpdate(!useTimeScale).SetEase(DA_Ease);

                    yield return tween.WaitForCompletion();
                    break;
                case ChangeStatusType.Fade:
                    tween = group.DOFade(0, DA_Duration).SetUpdate(!useTimeScale).SetEase(DA_Ease);

                    yield return tween.WaitForCompletion();
                    break;
            }

            if (deActiveGameObject)
            {
                gameObject.SetActive(false);
            }
        }
        
        private void PlayLoop()
        {
            switch (L_Type)
            {
                case LoopType.ZoomInZoomOut:
                    ZoomInZoomOut();
                    break;
                case LoopType.RotateInOutBack:
                    RotateInOutBack();
                    break;
            }
        }

        
        private void ZoomInZoomOut()
        {
            var target = new Vector3(scale.x + posChange.x, scale.y + posChange.y, 0);
            
            sequence = DOTween.Sequence();

            sequence.Pause();

            sequence.SetAutoKill(false);

            sequence.SetDelay(L_DelayPerPhase);
            
            //sequence.Append(trans.DOScale(scale, 0.05f).SetEase(Ease.InOutSine));
            
            sequence.Append(trans.DOScale(target, L_Duration / 4).SetUpdate(!useTimeScale).SetEase(L_Ease));

            sequence.Append(trans.DOScale(scale, L_Duration / 4).SetUpdate(!useTimeScale).SetEase(L_Ease));

            sequence.Append(trans.DOScale(target, L_Duration / 4).SetUpdate(!useTimeScale).SetEase(L_Ease));

            sequence.Append(trans.DOScale(scale, L_Duration / 4).SetUpdate(!useTimeScale).SetEase(L_Ease))
                .OnComplete(() =>
                {
                    StartCoroutine(CheckSequence());
                });
            
            sequence.Play();
        }
        
        private void RotateInOutBack()
        {
            var target1 = new Vector3(rotation.x, rotation.y, rotation.z + rotChange.z);
            
            var target2 = new Vector3(rotation.x, rotation.y, rotation.z - rotChange.z);
            
            sequence = DOTween.Sequence();
            
            sequence.Pause();

            sequence.SetAutoKill(false);

            //sequence.SetDelay(0);

            //sequence.Append(trans.DOLocalRotate(rotation, delayPerPhaseLoop).SetUpdate(!useTimeScale).SetEase(Ease.Linear));
            
            sequence.Append(trans.DOLocalRotate(target1, L_Duration / 4).SetUpdate(!useTimeScale).SetEase(Ease.Linear));

            sequence.Append(trans.DOLocalRotate(target2, L_Duration / 2).SetUpdate(!useTimeScale).SetEase(Ease.Linear));
            
            sequence.Append(trans.DOLocalRotate(rotation, L_Duration / 4).SetUpdate(!useTimeScale).SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    StartCoroutine(CheckSequence());
                });
            
            sequence.Play();  
        }

        private IEnumerator CheckSequence()
        {
            if (L_LoopPerPhase > 0)
            {
                loopCounter++;
            
                if (loopCounter >= L_LoopPerPhase)
                {
                    loopCounter = 0;

                    delayPerPhaseLoop = L_DelayPerPhase;
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
        
        private void OnDisable()
        {
            loopCounter = L_LoopPerPhase;
            
            StopAllCoroutines();

            trans.DOKill();   
            
            sequence.Kill();
            
            if (useAppearAnim)
            {
                switch (A_Type)
                {
                    case ChangeStatusType.Zoom:
                        trans.localScale = Vector3.zero;

                        group.alpha = 1;
                        break;
                    case ChangeStatusType.Fade:
                        trans.localScale = scale;

                        group.alpha = 0;
                        break;
                }
            }
            
            trans.localRotation = Quaternion.Euler(rotation);
        }
    }
}
