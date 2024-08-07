using UnityEngine;

namespace SupersonicWisdomSDK
{
    internal class SwNoTouchTimer : SwTimer
    {
        #region --- Mono Override ---

        protected override void Update()
        {
            if (SwUtils.IsRunningOnDevice() && Input.touchCount > 0 || !SwUtils.IsRunningOnDevice() && Input.GetMouseButton(0))
            {
                if (IsDisabled) return;
                
                if (IsEnabled || IsPaused || DidFinish)
                {
                    SwInfra.Logger.Log($"SwNoTouchTimer | StartTimer due to touch | {Name}");
                    StartTimer();

                    return;
                }
            }

            base.Update();
        }

        #endregion


        #region --- Public Methods ---

        public new static SwNoTouchTimer Create(GameObject gameObject, string name = "", float duration = 0, bool shouldPauseWhenUnityOutOfFocus = false)
        {
            return CreateGeneric<SwNoTouchTimer>(gameObject, name, duration, shouldPauseWhenUnityOutOfFocus);
        }

        #endregion
    }
}