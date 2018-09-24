using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoxMotor : Motor {
	
	public override void Initialize(MovableEntity entity, InputSource input) {
		var state = entity.State as BoxEntityState;
		state.Rb = entity.GetComponent<Rigidbody>();
		state.Sfx = entity.GetComponent<SoundEffectPlayer>();
		
		state.ShadowCaster = entity.GetComponent<ShadowCaster>();
		state.Animator = entity.GetComponent<Animator>();
		state.DefaultPosition = entity.transform.position;
	    state.Cage = entity.GetComponentInChildren<MeshRenderer>();
	    state.Col = entity.GetComponentInChildren<BoxCollider>();
	}

	public override void Setup(MovableEntity entity, InputSource input) {
		
	}

	public override void Tick(MovableEntity entity, InputSource input) {
	    if (input == null)
	        return;
	    
	    var state = entity.State as BoxEntityState;
	    float vert = input.AxisY;
	    float hori = input.AxisX;
	    
	    if (Mathf.Abs(vert) >= 0.01f || Mathf.Abs(hori) >= 0.01f) {
	        Move(state, new Vector3(hori, 0, vert).normalized);
	    }

	    if (state.CanSpawn && (ActionButton(input) || state.Controller.AutoSpawn)) {
	        state.Controller.SpawnOrc();		
	    }
	}

	public override void FixedTick(MovableEntity entity, InputSource input) {
	    var state = entity.State as BoxEntityState;
        ApplyGravity(entity, state);
		TestCollisions(state);
	}

	public override void LateTick(MovableEntity entity, InputSource input) {

	}

    private static void ApplyGravity(MovableEntity entity, BoxEntityState state) {
        Vector3 gravity = Physics.gravity * state.GravityScale;
        state.Rb.AddForce(gravity, ForceMode.Acceleration);
    }

    private bool ActionButton(InputSource input) {
        return input.ActionDownButton || input.ActionLeftButton || input.ActionRightButton || input.ActionUpButton;
    }
    
    public void Move(BoxEntityState state, Vector3 dir) {
        state.Rb.AddForce(dir * state.MoveSpeed * Time.deltaTime);
    }

    public void EnableBox(BoxEntityState state) {
        state.Cage.enabled = true;
        state.Col.enabled = true;
    }

    public void DisableBox(BoxEntityState state) {
     
        state.Cage.enabled = false;
        state.Col.enabled = false;
        ScreenEffects.Instance.CreateBoxParticles(state.transform.position);
        ScreenEffects.Instance.ScreenShake(0.1f, 1f);
        GlobalAudio.Instance.PlayByIndex(1);
    }

    public void ResetToDefault(BoxEntityState state) {
        state.StopAllCoroutines();
        state.Rb.isKinematic = true;     
        state.RoofPoint.position =  state.DefaultPosition + Vector3.up * -200;
        state.transform.position =  state.DefaultPosition;
        state.transform.rotation = Quaternion.identity;
        state.JointRb.isKinematic = true;
        state.JointRb.transform.position = state.transform.position;
        state.Cage.enabled = false;
        state.Col.enabled = true;
        
        state.ShadowCaster.SetLineEnabled(false);
    }
    
    public void LowerBox(BoxEntityState state) {
        state.StopAllCoroutines();
        state.StartCoroutine(LowerSpawner(state));
               
    }

    public void RaiseBox(BoxEntityState state) {
        state.StopAllCoroutines();
        state.StartCoroutine(RaiseSpawner(state));
    }
    
    public void SetDefaultRaised(BoxEntityState state, float raised) {
        state.DefaultRaised = raised;
    }

    public void SetDefaultLowered(BoxEntityState state, float lowered) {
        state.DefaultLowered = lowered;
    }

    public void Free(BoxEntityState state) {
        state.Rb.isKinematic = false;
        state.JointRb.isKinematic = false;
        state.Rb.constraints = RigidbodyConstraints.None;
        state.JointRb.constraints = RigidbodyConstraints.None;  
        state.ShadowCaster.SetLineEnabled(true);
    }
    
    public void SetDefaultPosition(BoxEntityState state, Vector3 position, float loweredOffSet, float raisedOffSet) {
        SetDefaultLowered(state, position.y + loweredOffSet);
        SetDefaultRaised(state, position.y + raisedOffSet);
    }
    
    private IEnumerator RaiseSpawner(BoxEntityState state) {
        state.ShadowCaster.SetLineEnabled(false);
        
        float timer = 0;
        while (timer < 2f) {
            state.RoofPoint.position = Vector3.Lerp(state.RoofPoint.position,
                new Vector3(state.RoofPoint.position.x, state.DefaultRaised, state.RoofPoint.position.z), timer / 2f);
            timer += Time.deltaTime;
            yield return null;
        }
    }
	
    private IEnumerator LowerSpawner(BoxEntityState state) {
  
        
        state.Sfx.PlaySFxByIndex(0, Random.Range(0.9f, 1.1f));
        state.Sfx.PlayForSeconds(1, Random.Range(0.9f, 1.1f), 4f);
        Vector3 targetPos = new Vector3(state.RoofPoint.position.x, state.DefaultLowered, state.RoofPoint.position.z);
        float timer = 0;
        while (timer < 2f) {
            state.RoofPoint.position = Vector3.Lerp(state.RoofPoint.position, targetPos, timer / 2f);
            timer += Time.deltaTime;
            yield return null;
        }	
        state.ShadowCaster.SetLineEnabled(true);
        state.Controller.ReadyToSpawn();
   
    }

    private void TestCollisions(BoxEntityState state) {

        var cols = Physics.BoxCastAll(state.transform.position, Vector3.one * 4.1f, Vector3.up,
            state.transform.rotation, 1f);
        if (cols.Length > 0 && state.Col.enabled) {
            foreach (var col in cols) {
                
                if (col.collider.gameObject == state.gameObject)
                    continue;

                if (col.collider.CompareTag("Player")) {

                    var otherEntity = col.collider.GetComponent<MovableEntity>();
                    var otherMotor = otherEntity.Motor as OrcMotor;
                    var otherState = otherEntity.State as OrcEntityState;
                    otherMotor.Damage(otherState,
                        (col.transform.position - state.transform.position).normalized, 500, 2f, false, false,
                        state.Controller.PlayerNumber);

                }
                else if (col.collider.CompareTag("HollowBox") && state.Col.enabled) {
                    state.Controller.SpawnOrc();
                }
            }
        }        
    }
}
