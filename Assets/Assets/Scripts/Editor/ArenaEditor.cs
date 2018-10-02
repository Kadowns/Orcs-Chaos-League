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
	
	

	public override void OnInspectorGUI() {
		if (_target == null)
			_target = target as ArenaState;


		DrawDefaultInspector();

		//DrawPlataformsArray();
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
            oscilation = gui.EzCurveField("", AnimationCurve.Linear(0f, 0, 1, 1));
            _maxLife = gui.EzIntField("Max Health", _maxLife);
            _loweredTime = gui.EzFloatField("Lowered Time", _loweredTime);
            _oscilationFrequency = gui.EzFloatField("Oscilation Frequency", _oscilationFrequency);
            _offSetter = gui.EzFloatField("OffSetter", _offSetter);
        }

		foreach (var plat in _target.Plataforms) {
            var platBehaviour = plat.GetComponent<PlataformBehaviour>();
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
                    platBehaviour._oscilationCurve = gui.EzCurveField("", platBehaviour._oscilationCurve);
                    platBehaviour._maxLife = gui.EzIntField("Max Health", platBehaviour._maxLife);
                    platBehaviour._loweredTime = gui.EzFloatField("Lowered Time", platBehaviour._loweredTime);
                    platBehaviour._oscilationFrequency = gui.EzFloatField("Oscilation Frequency", platBehaviour._oscilationFrequency);
                    platBehaviour._offSetter = gui.EzFloatField("OffSetter", platBehaviour._offSetter);
                }
            }
            else {
                platBehaviour._oscilationCurve = oscilation;
                platBehaviour._maxLife = _maxLife;
                platBehaviour._loweredTime = _loweredTime;
                platBehaviour._oscilationFrequency = _oscilationFrequency;
                platBehaviour._offSetter = _offSetter;
            }
			gui.EzSpacer(0f, 50f);
		}
	}
}
#endif
