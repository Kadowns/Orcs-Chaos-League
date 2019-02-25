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



        private bool _eventContextListFoldout = false;

        public override void OnInspectorGUI() {

            if (_target == null) {
                _target = target as GreatEventGenerator;
            }
            
            DrawEventContextList();
          //  DrawContextOptions();
            DrawAddButtons();
        }



        private void DrawEventContextList() {

            _eventContextListFoldout = gui.EzFoldout("Event Context List", _eventContextListFoldout);
            if (!_eventContextListFoldout)
                return;

            foreach (var context in _target.EventContexts) {
                using (gui.Horizontal()) {
                    gui.EzObjectField(context.GetType().Name, context);
                    if (gui.EzButton(gui.DeleteButton)) {
                        DeleteContextObject(context);
                        break;
                    }                
                }    
            }          
        }

        private void DrawContextOptions() {
            using (gui.Horizontal()) {
            
                gui.EzLabel("Spawn Context: ");
              //  _spawnContext.ObjectToSpawnName = gui.EzTextField("Object to spawn name: ", _spawnContext.ObjectToSpawnName);
               // _spawnContext.Spawner = gui.EzTextField("Object to spawn name: ", _spawnContext.ObjectToSpawnName);

            }
        }

        private void DrawAddButtons() {
            using (gui.Horizontal()) {
                DrawButtonAddContext<SpawnContext>();
                DrawButtonAddContext<FooContext>();
            }      
        }

        private void DrawButtonAddContext<T>() where T : Component {
            if (gui.EzButton(typeof(T).Name)) {
                _newContext = CreateContextObject<T>() as EventContext;
                _target.EventContexts.Add(_newContext);
                _newContext = null;
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