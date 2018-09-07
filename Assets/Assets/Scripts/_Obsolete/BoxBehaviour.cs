using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;


public class BoxBehaviour : MonoBehaviour {

    [SerializeField] private Transform _roofPoint;
    [SerializeField] private float _moveSpeed = 500f;
    [SerializeField] private GameObject _cageDoor;
    [SerializeField] private Animator _slotText;
    
    private float _defaultRaised, _defaultLowered;

    private SoundEffectPlayer _sfx;

    private SpawnerBehaviour _spawner;
    
    private ShadowCaster _shadowCaster;

    private ParticleSystem _ps;

    private Renderer _cage;

    private Collider _col;

    private Rigidbody _rb;
    
    private Animator _animator;

    private GameObject _player;

    private Vector3 _defaultPosition;
    
    private bool _playerInside;

    private void Start() {

        _rb = GetComponent<Rigidbody>();
        _sfx = GetComponent<SoundEffectPlayer>();
        _ps = GetComponent<ParticleSystem>();
        _cage = GetComponentInChildren<MeshRenderer>();
        _col = GetComponentInChildren<BoxCollider>();
        _shadowCaster = GetComponent<ShadowCaster>();
        _animator = GetComponent<Animator>();
        _defaultPosition = transform.position;

    }

    public void BoxClosed() {
        
    }

    public void OpenBox() {
        _animator.SetBool("PlayerInside", false);
    }
    
    public void Move(Vector3 dir) {
        _rb.AddForce(dir * _moveSpeed * Time.fixedDeltaTime);
    }

    public void EnableBox() {
        _cage.enabled = true;
        _col.enabled = true;
    }

    public void DisableBox() {
        _ps.Stop();
        _cage.enabled = false;
        _col.enabled = false;
        ScreenEffects.Instance.CreateBoxParticles(transform.position);
        ScreenEffects.Instance.ScreenShake(0.1f, 1f);
        GlobalAudio.Instance.PlayByIndex(1);
    }

    public void ResetToDefault() {
        StopAllCoroutines();
        _cageDoor.SetActive(false);
        _slotText.SetTrigger("Free");
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _player = null;
        _spawner = null;
        _roofPoint.position = _defaultPosition + Vector3.up * -200;
        transform.position = _defaultPosition;
        transform.rotation = Quaternion.identity;
        _cage.enabled = false;
        _col.enabled = true;
        _ps.Stop();
        _shadowCaster.SetLineEnabled(false);

    }
    
    public void LowerBox() {
        StopAllCoroutines();
        StartCoroutine(LowerSpawner());
               
    }

    public void RaiseBox() {
        StopAllCoroutines();
        StartCoroutine(RaiseSpawner());
    }
    
    public void SetDefaultRaised(float raised) {
        _defaultRaised = raised;
    }

    public void SetDefaultLowered(float lowered) {
        _defaultLowered = lowered;
    }

    public void Free() {
        _rb.constraints = RigidbodyConstraints.None;
        _shadowCaster.SetLineEnabled(true);
    }
    
    public void SetDefaultPosition(Vector3 position, float loweredOffSet, float raisedOffSet) {
        SetDefaultLowered(position.y + loweredOffSet);
        SetDefaultRaised(position.y + raisedOffSet);
    }
    
    private IEnumerator RaiseSpawner() {
        _shadowCaster.SetLineEnabled(false);
        
        float timer = 0;
        while (timer < 2f) {
            _roofPoint.position = Vector3.Lerp(_roofPoint.position,
                new Vector3(_roofPoint.position.x, _defaultRaised, _roofPoint.position.z), timer / 2f);
            timer += Time.deltaTime;
            yield return null;
        }
    }
	
    private IEnumerator LowerSpawner() {
  
        
        _sfx.PlaySFxByIndex(0, Random.Range(0.9f, 1.1f));
        _sfx.PlayForSeconds(1, Random.Range(0.9f, 1.1f), 4f);
        Vector3 targetPos = new Vector3(_roofPoint.position.x, _defaultLowered, _roofPoint.position.z);
        float timer = 0;
        while (timer < 2f) {
            _roofPoint.position = Vector3.Lerp(_roofPoint.position, targetPos, timer / 2f);
            timer += Time.deltaTime;
            yield return null;
        }	
        _shadowCaster.SetLineEnabled(true);
        _spawner.ReadyToSpawn();
   
    }

    public bool PlayerInside() {
        return _playerInside;
    }

    private void OnCollisionEnter(Collision col) {
     
        
        if (col.collider.CompareTag("Player")) {

            if (_player == null) {
                _player = col.gameObject;
            
                //QUE HORRROOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOR
                _spawner = _player.GetComponent<FighterBehaviour>().GetParent().GetComponent<SpawnerBehaviour>();
                _spawner.SetDefaultSpawner(gameObject);
            
                _shadowCaster.SetLineColor(_spawner.GetComponent<PlayerPointer>().GetPlayerColor());
                GameController.Instance.IncreaseReadyPlayers();
                _player.SetActive(false);
                _playerInside = true;     
                _cageDoor.SetActive(true);
                _slotText.SetTrigger("Close");
            }
            else {
                col.collider.GetComponent<FighterBehaviour>()
                    .Damage((col.transform.position - transform.position).normalized, 500, 2f, false, false);
            }
              
        }
        else if (col.collider.CompareTag("HollowBox")) {
            _spawner.SpawnNewOrc();
        }
    }

    private IEnumerator Burning() {
        
        yield return new WaitForSeconds(2.5f);
        _spawner.SpawnNewOrc();
    }
}
