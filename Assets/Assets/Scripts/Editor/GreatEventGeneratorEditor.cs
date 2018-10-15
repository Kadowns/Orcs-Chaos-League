#if UNITY_EDITOR
using Assets.Scripts.Scenario.Events.GreatEvents;
using EzEditor;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor {
    
    [CustomEditor(typeof(GreatEventGenerator))]
    public class GreatEventGeneratorEditor : UnityEditor.Editor {

        private GreatEventGenerator _target;

        private EventContext _newContext;


        public override void OnInspectorGUI() {

            if (_target == null) {
                _target = target as GreatEventGenerator;
            }
            
            DrawEventContextList();
            
            
        }



        private void DrawEventContextList() {

            foreach (var context in _target.EventContexts) {
                using (gui.Horizontal()) {
                    gui.EzObjectField(context.GetType().Name, context);
                    if (gui.EzButton(gui.DeleteButton)) {
                        DeleteContextObject(context);
                        break;
                    }                
                }    
            }
            
            

            using (gui.Horizontal()) {
                if (gui.EzButton(gui.AddButton)) {
                    _newContext = CreateContextObject<SpawnContext>();
                    _target.EventContexts.Add(_newContext);
                    _newContext = null;
                }
                else if (gui.EzButton(gui.AddButton)) {
                    _newContext = CreateContextObject<FooContext>();
                    _target.EventContexts.Add(_newContext);
                    _newContext = null;
                }
            }      
        }


        private void DeleteContextObject(EventContext context) {
            var obj = context.gameObject;
            _target.EventContexts.Remove(context);
            DestroyImmediate(obj);
        }


        private T CreateContextObject<T>() where T : Component {
            var obj = new GameObject();
            obj.transform.SetParent(_target.transform);
            var context = obj.AddComponent<T>();
            return context;
        }
    }
}
#endif // UNITY_EDITOR