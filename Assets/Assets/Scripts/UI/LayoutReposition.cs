using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LayoutReposition : MonoBehaviour {


       [SerializeField] private GameObject[] _childs;
       
       private RectTransform _rectTransform;
       private HorizontalLayoutGroup _layout;
       private float _childWidth;


       private void Awake() {
              _rectTransform = GetComponent<RectTransform>();
              _layout = GetComponent<HorizontalLayoutGroup>();          
       }

       private void Start() {
              _childWidth = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
              _layout.padding.left = (int)_childWidth / -2;
       }

//       public void DeactivateChild() {
//              foreach (var child in _childs) {
//                     child.SetActive(false);
//              }
//       }

       public void ResetActiveChild() {
              foreach (var child in _childs) {
                     if (!child.activeInHierarchy)
                            continue;
                     
                     child.SetActive(false);
                     child.SetActive(true);
              }
       }

       public void RecalculatePosition() {
              var childsNum = ActiveChildren() - 1;
              float leftOffSet = (childsNum * (_childWidth + _layout.spacing)) / -2;
              _rectTransform.anchoredPosition = new Vector3(leftOffSet, _rectTransform.position.y, _rectTransform.position.z);
       }

       private int ActiveChildren() {
              int count = 0;
          
              foreach (var child in _childs) {
                     if (!child.gameObject.activeInHierarchy)
                            continue;                 
                     count++;
              }
              return count;
       }
       

}
