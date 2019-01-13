using Assets.Scripts.PlayerControllers.Input;
using UnityEngine;


[CreateAssetMenu(menuName = "InputSource/BotInput")]
public class BotInput : InputSource {
       
    public override void Tick(InputController input) {
        ClearInput(input);

        var memory = BotBrains.BotBrainMemories[input.PlayerNumber];
        
        if (memory.ThisOrc.gameObject.activeInHierarchy && ArenaController.Instance.GameStated) {
            
            if (memory.Target == null) {
                FindTarget(memory);
            }
            else {                   
                if (!memory.Target.gameObject.activeInHierarchy) {
                    memory.Target = null;
            
                }
            }
            
           
            bool shouldStomp = false;

            if (memory.Target != null) {

                MoveToPosition(input, memory, memory.Target.position);
                
                
                if (memory.DistanceToTarget < 50) {

                    input.Axis = Vector2.zero;
                    if (Time.time > memory.InputTimer){
                        memory.InputTimer = Time.time + memory.NextInputTime;
                        switch (Random.Range(1, 2)) {
                            case 1:
                                input.ActionLeftButtonPressed = true;
                                break;
                            case 2:
                             //   shouldStomp = Random.Range(0f, 1f) > 0.2f;
                                break;
                        }                 
                    }
                }           
            }
            else {
                MoveToPosition(input, memory, Vector3.zero);
                
                if (memory.DistanceToTarget < 16) {
                    input.Axis = Vector2.zero;
                    if (Time.time > memory.InputTimer){
                        memory.InputTimer = Time.time + memory.NextInputTime;
                        input.DPadUpPressed = true;
                    }
                } 
            }
         
            if ((ShouldJump(input.Axis, memory) || shouldStomp) && Time.time > memory.InputTimer) {
                input.ActionDownButtonPressed = true;
                memory.InputTimer = Time.time + memory.NextInputTime;
            }
        }
    }

    public void MoveToPosition(InputController input, BotBrainMemory memory, Vector3 target) {
        Vector3 moveDir = Vector3.zero;
        moveDir = (target - memory.ThisOrc.position);
        moveDir.Set(moveDir.x, 0, moveDir.z);
        memory.DistanceToTarget = moveDir.sqrMagnitude;
        moveDir.Normalize();
        input.Axis = new Vector2(moveDir.x, moveDir.z);
    }

    private void ClearInput(InputController input) {
        input.Axis = Vector2.zero;
        input.CenterButtonPresssed = false;
        input.ActionDownButton = false;
        input.ActionUpButton = false;
        input.ActionLeftButton = false;
        input.ActionRightButton = false;
        input.ActionDownButtonPressed = false;
        input.ActionUpButtonPressed = false;
        input.ActionLeftButtonPressed = false;
        input.ActionRightButtonPressed = false;
        input.ActionDownButtonReleased = false;
        input.ActionUpButtonReleased = false;
        input.ActionLeftButtonReleased = false;
        input.ActionRightButtonReleased = false;
        input.DPadUp = false;
        input.DPadDown = false;
        input.DPadLeft = false;
        input.DPadRight = false;
        input.DPadUpPressed = false;
        input.DPadDownPressed = false;
        input.DPadLeftPressed = false;
        input.DPadRightPressed = false;
        input.DPadUpReleased = false;
        input.DPadDownReleased = false;
        input.DPadLeftReleased = false;
        input.DPadRightReleased = false;
    }


    private bool ShouldJump(Vector3 moveDir, BotBrainMemory memory) {
        RaycastHit hit;
        return !Physics.Raycast(memory.ThisOrc.position + Vector3.up * 3, (moveDir + Vector3.down).normalized, out hit,
            6f, 1 << LayerMask.NameToLayer("Ground"));
    }

    private void FindTarget(BotBrainMemory memory) {
        var orcs = Physics.OverlapSphere(memory.ThisOrc.position, 600f, 1 << LayerMask.NameToLayer("Players"));
        if (orcs.Length > 0) {
            float minDist = float.MaxValue;
            foreach (var orc in orcs) {
                if (orc.gameObject == memory.ThisOrc.gameObject)
                    continue;
                
                float dist = (orc.transform.position - memory.ThisOrc.position).sqrMagnitude;

                if (dist < minDist) {
                    minDist = dist;
                    memory.Target = orc.transform;
                }
            }
        }
    }
}
