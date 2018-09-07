using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController> {
    private float _targetScale;
    private bool _lerping;
    private float _timeToFreeze, _freezeTimer;

    private void Update() {
        if (!_lerping) {
            return;
        }

        _freezeTimer += Time.unscaledDeltaTime;
        if (_freezeTimer > _timeToFreeze) {
            Time.timeScale = _targetScale;
            _lerping = false;
            _freezeTimer = 0;
        }


//        var currentScale = Time.timeScale;
//      
//        Time.timeScale = Mathf.Lerp(currentScale, _targetScale, TransitionSpeed);
//        var max = Mathf.Max(currentScale, _targetScale);
//        var min = Mathf.Min(currentScale, _targetScale);
//        if (Mathf.Abs(max - min) < SnapThreshold) {
//            _lerping = false;
//            Time.timeScale = _targetScale;
//        }
    }

    public void DoTimeScale(float toScale, float transitionTime) {
        _targetScale = toScale;
        _timeToFreeze = transitionTime;
        _lerping = true;
    }
}
