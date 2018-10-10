using UnityEngine;

public class BotInput : InputSource {
    
    public Transform ThisOrc { get; set; }
    public Transform ThisBox { get; set; }

    private Transform _target;

    private float _jumpTimer = 0;
    private const float NextJumpTime = 0.15f;

    private void Update() {
        ClearInput();

        

        if (ThisOrc.gameObject.activeInHierarchy) {
            
            if (_target == null) {
                FindTarget();
            }
            else {                   
                if (!_target.gameObject.activeInHierarchy) {
                    _target = null;
            
                }
            }
            
   
            Vector3 moveDir = Vector3.zero;
            bool shouldStomp = false;

            if (_target != null) {

                moveDir = (_target.position - ThisOrc.position);
                moveDir.Set(moveDir.x, 0, moveDir.z);
                float dist = moveDir.sqrMagnitude;
                moveDir.Normalize();
                
                
                if (dist > 50) {
                    
                    AxisX = moveDir.x;
                    AxisY = moveDir.z;
                 
                }
                else {
                    switch (Random.Range(1, 2)) {
                            case 1:
                                ActionLeftButtonPressed = true;
                                break;
                            case 2:
                                shouldStomp = Random.Range(0f, 1f) > 0.2f;
                                break;
                    }
                             
                }
            }
            else {
                moveDir = -ThisOrc.position;
                moveDir.Set(moveDir.x, 0, moveDir.z);
                float dist = moveDir.sqrMagnitude;
                moveDir.Normalize();
                
                if (dist > 16) {
                    AxisX = moveDir.x;
                    AxisY = moveDir.z;
                }
                else {
                    DPadUpPressed = true;
                }
            }
         
            if ((ShouldJump(moveDir) || shouldStomp) && Time.time > _jumpTimer) {
                ActionDownButtonPressed = true;
                _jumpTimer = Time.time + NextJumpTime;
            }
        }
    }

    private void ClearInput() {
        AxisX = 0;
        AxisY = 0;
        CenterButtonPresssed = false;
        ActionDownButton = false;
        ActionUpButton = false;
        ActionLeftButton = false;
        ActionRightButton = false;
        ActionDownButtonPressed = false;
        ActionUpButtonPressed = false;
        ActionLeftButtonPressed = false;
        ActionRightButtonPressed = false;
        ActionDownButtonReleased = false;
        ActionUpButtonReleased = false;
        ActionLeftButtonReleased = false;
        ActionRightButtonReleased = false;
        DPadUp = false;
        DPadDown = false;
        DPadLeft = false;
        DPadRight = false;
        DPadUpPressed = false;
        DPadDownPressed = false;
        DPadLeftPressed = false;
        DPadRightPressed = false;
        DPadUpReleased = false;
        DPadDownReleased = false;
        DPadLeftReleased = false;
        DPadRightReleased = false;
    }


    private bool ShouldJump(Vector3 moveDir) {
        RaycastHit hit;
        return !Physics.Raycast(ThisOrc.position + Vector3.up * 3, (moveDir + Vector3.down * 0.5f).normalized, out hit,
            40f, 1 << LayerMask.NameToLayer("Ground"));
    }

    private void FindTarget() {
        var orcs = Physics.OverlapSphere(ThisOrc.position, 600f, 1 << LayerMask.NameToLayer("Players"));
        if (orcs.Length > 0) {
            float minDist = float.MaxValue;
            foreach (var orc in orcs) {
                if (orc.gameObject == ThisOrc.gameObject)
                    continue;
                
                float dist = (orc.transform.position - ThisOrc.position).sqrMagnitude;

                if (dist < minDist) {
                    minDist = dist;
                    _target = orc.transform;
                }
            }
        }
    }
}
