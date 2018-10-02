#if UNITY_EDITOR
using EzEditor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArenaState))]
public class ArenaEditor : Editor {

	private ArenaState _target;

	private AnimationCurve oscilation = null;
    private int _maxLife;
    private float _loweredTime;
    private float _oscilationFrequency;
    private float _offSetter;
	
	private bool _individualPlataformEditing;
	private bool _plataformFoldout;

	private bool foldout;

	private Editor[] _editors;
	

	public override void OnInspectorGUI() {
		if (_target == null) {
			_target = target as ArenaState;
			_editors = new Editor[_target.GreatEvents.Count];
		}

		DrawDefaultInspector();
		
		DrawEventEditor();

		//DrawPlataformsArray();
	}

	void DrawEventEditor() {
		int i = 0;
		foreach (var ev in _target.GreatEvents) {
			DrawSettingsEditor(ev, ref _editors[i]);
			i++;
		}
	}

	void DrawSettingsEditor(Object settings, ref Editor editor) {

		if (settings != null) {
			foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
			using (var check = new EditorGUI.ChangeCheckScope()) {
		
				if (foldout) {
					CreateCachedEditor(settings, null, ref editor);
					editor.OnInspectorGUI();
				}			
			}	
		}		
	}
	
	private void DrawPlataformsArray() {
		_plataformFoldout = gui.EzFoldout("Plataforms", _plataformFoldout);

		if (!_plataformFoldout)
			return;

		if (_target.Plataforms.Count == 0) {
			gui.EzHelpBox("Nenhuma plataforma foi adicionada", MessageType.Warning, true);
		}
		
		if (gui.EzButton("Individual Plataform Editing")) {
			_individualPlataformEditing = !_individualPlataformEditing;
		}

        if (!_individualPlataformEditing) {
            gui.EzLabel("OscilationCurve");
            oscilation = gui.EzCurveField("", _target.Plataforms[0]._oscilationCurve);
            _maxLife = gui.EzIntField("Max Health", _target.Plataforms[0]._maxLife);
            _loweredTime = gui.EzFloatField("Lowered Time", _target.Plataforms[0]._loweredTime);
            _oscilationFrequency = gui.EzFloatField("Oscilation Frequency", _target.Plataforms[0]._oscilationFrequency);
            _offSetter = gui.EzFloatField("OffSetter", _target.Plataforms[0]._offSetter);
	        foreach (var plat in _target.Plataforms) {
		        plat._oscilationCurve = oscilation;
		        plat._maxLife = _maxLife;
                plat._loweredTime = _loweredTime;
                plat._oscilationFrequency = _oscilationFrequency;
                plat._offSetter = _offSetter;
	        }
        }

		foreach (var plat in _target.Plataforms) {
            gui.EzSpacer(0f, 50f);
            if (_individualPlataformEditing) {
                using (gui.Horizontal()) {

                    gui.EzObjectField("Platform", plat, 20f);

                    if (gui.EzButton(gui.DeleteButton)) {
                        _target.Plataforms.Remove(plat);
                    }
                }
                using (gui.Vertical()) {
                    gui.EzLabel("OscilationCurve");
                    plat._oscilationCurve = gui.EzCurveField("", plat._oscilationCurve);
                    plat._maxLife = gui.EzIntField("Max Health", plat._maxLife);
                    plat._loweredTime = gui.EzFloatField("Lowered Time", plat._loweredTime);
                    plat._oscilationFrequency = gui.EzFloatField("Oscilation Frequency", plat._oscilationFrequency);
                    plat._offSetter = gui.EzFloatField("OffSetter", plat._offSetter);
                }
            }
            else {
//                plat._oscilationCurve = plat._oscilationCurve;
//                plat._maxLife = plat._maxLife;
//                plat._loweredTime = _loweredTime;
//                plat._oscilationFrequency = _oscilationFrequency;
//                plat._offSetter = _offSetter;
            }
			gui.EzSpacer(0f, 50f);
		}
	}
}
#endif
