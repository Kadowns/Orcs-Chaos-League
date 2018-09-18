#if UNITY_EDITOR
using EzEditor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArenaState))]
public class ArenaEditor : Editor {

	private ArenaState _target;

	private AnimationCurve oscilation = null;
	
	private bool _individualPlatformOscilationEditor;
	private bool _plataformFoldout;
	
	

	public override void OnInspectorGUI() {
		if (_target == null)
			_target = target as ArenaState;


		
		
		DrawPlataformsArray();
		
		
	}

	private void DrawPlataformsArray() {
		_plataformFoldout = gui.EzFoldout("Plataforms", _plataformFoldout);

		if (!_plataformFoldout)
			return;

		if (_target.Plataforms.Count == 0) {
			gui.EzHelpBox("Nenhuma plataforma foi adicionada", MessageType.Warning, true);
		}
		
		if (gui.EzButton("Individual Oscilation Curve")) {
			_individualPlatformOscilationEditor = !_individualPlatformOscilationEditor;
		}


	
		if (!_individualPlatformOscilationEditor) {
			gui.EzLabel("OscilationCurve");
			oscilation = gui.EzCurveField("",
				AnimationCurve.Linear(0f, 0,
					1, 1));
		}

		foreach (var plat in _target.Plataforms) {
			gui.EzSpacer(0f, 50f);
			using (gui.Horizontal()) {

				gui.EzObjectField("Platform", plat, 20f);
					

				if (gui.EzButton(gui.DeleteButton)) {
					_target.Plataforms.Remove(plat);
				}				
			}	
			using (gui.Vertical()) {
				if (_individualPlatformOscilationEditor) {
					gui.EzLabel("OscilationCurve");
					gui.EzCurveField("", plat.GetComponent<PlataformBehaviour>()._oscilationCurve);
				}
				else {
					plat.GetComponent<PlataformBehaviour>()._oscilationCurve = oscilation;
				}
			}
			gui.EzSpacer(0f, 50f);
		}
	}
}
#endif
