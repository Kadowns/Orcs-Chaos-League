using UnityEngine;

public class BotInput : InputSource {
    
    public Transform Orc { get; set; }
    public Transform Box { get; set; }

    private Transform _target;

    private float jumpTimer = 0, nextJumpTime = 0.15f;
    
    private void Update() {

        if (_target == null) {
            FindTarget();
        }
        else {

            Vector3 moveDir = (_target.position -Orc.position);
            moveDir.Set(moveDir.x, 0, moveDir.z);
            float dist = moveDir.sqrMagnitude;
            moveDir.Normalize();

            Debug.Log(dist);
            if (dist > 50) {
                AxisX = moveDir.x;
                AxisY = moveDir.z;
            }
            else {
                AxisX = 0;
                AxisY = 0;
            }

            if (ShouldJump(moveDir) && Time.time > jumpTimer) {
                ActionDownButtonPressed = true;
                jumpTimer = Time.time + nextJumpTime;
            }
            else {
                ActionDownButtonPressed = false;
            }
        }
    }


    private bool ShouldJump(Vector3 moveDir) {
        RaycastHit hit;
        return !Physics.Raycast(transform.position + Vector3.up * 3, (moveDir + Vector3.down).normalized, out hit,
            40f, 1 << LayerMask.NameToLayer("Ground"));
    }

    private void FindTarget() {
        var orcs = Physics.OverlapSphere(transform.position, 100f, 1 << LayerMask.NameToLayer("Players"));
        if (orcs.Length > 0) {
            float minDist = float.MaxValue;
            foreach (var orc in orcs) {
                float dist = (orc.transform.position - transform.position).sqrMagnitude;

                if (dist < minDist) {
                    minDist = dist;
                    _target = orc.transform;
                }
            }
        }
    }
}
