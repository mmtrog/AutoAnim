namespace AutoAnim.Scripts.Animation
{
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.UI;

    public class ButtonController : Controller
    {
        #region General settings

        [ShowIf("ManualSetUp")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("General/Components", showLabel: false, centerLabel: true)]
        protected Button button;
        
        #endregion
        
        
        #region Press Animation

        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase"), TabGroup("Phase/State", "Press")]
        [Tooltip("Animation style")] 
        [LabelText("Type")]
        protected PressType pType;
        
        [ShowIf("UsePressAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Press/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Animation playback time")] 
        [LabelText("Duration")]
        protected float pDuration = 0.5f;
        
        [ShowIf("UsePressAnim")]
        [SerializeField, GUIColor(1, 1, 1, 1), BoxGroup("Phase/State/Press/Setting", showLabel:false, centerLabel:true)]
        [Tooltip("Animation ease chart")] 
        [LabelText("Ease")]
        protected Ease pEase = Ease.InOutSine;

        public bool UsePressAnim => pType != PressType.NoUse;
        
        #endregion
    }
}