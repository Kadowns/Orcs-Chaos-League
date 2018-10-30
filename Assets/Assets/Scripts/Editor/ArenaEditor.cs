﻿#if UNITY_EDITOR
using EzEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArenaState))]
public class ArenaEditor : Editor {

	private ArenaState _target;

	private LavaGeyser _newGeyser;
	private bool _lavaGeyserFoldout;

	private PlataformBehaviour _newPlataform;
	private bool _plataformsFoldout;

	private GreatEvent _newGreatEvent;
	private Editor[] _greatEventEditors;
	private bool[] _greatEventEditorsFoldout;
	
	

	public override void OnInspectorGUI() {
		if (_target == null) {
			_target = target as ArenaState;
			_greatEventEditors = new Editor[_target.GreatEvents.Count];
			_greatEventEditorsFoldout = new bool[_target.GreatEvents.Count];
		}
		
		gui.EzObjectArray("Lava Geysers", _target.LavaGeysers, ref _newGeyser, ref _lavaGeyserFoldout);
		gui.EzObjectArray("Lava Geysers", _target.Plataforms, ref _newPlataform, ref _plataformsFoldout);
		DrawGlobalPlatformOptions(ref _target.GlobalPlatformSettings);
		DrawListEditor();

	}

	void DrawListEditor() {

		if (_greatEventEditors.Length != _target.GreatEvents.Count || _greatEventEditorsFoldout.Length != _target.GreatEvents.Count) {
			_greatEventEditors = new Editor[_target.GreatEvents.Count];
			_greatEventEditorsFoldout = new bool[_target.GreatEvents.Count];
		}

		for (int i = 0; i < _target.GreatEvents.Count; i++) {

			if (!DrawSettingsEditor(_target.GreatEvents[i], ref _greatEventEditors[i], ref _greatEventEditorsFoldout[i])) {
				break;
			}

		}

		_newGreatEvent = gui.EzObjectField("New Great Event", _newGreatEvent);
		if (_newGreatEvent != null) {
			_target.GreatEvents.Add(_newGreatEvent);
			_newGreatEvent = null;
		}	
	}

	private bool DrawSettingsEditor(Object settings, ref Editor editor, ref bool foldout) {

		if (settings != null) {
			foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
			using (var check = new EditorGUI.ChangeCheckScope()) {
		
				if (foldout) {
					CreateCachedEditor(settings, null, ref editor);
					editor.OnInspectorGUI();
				}			
				
				if (gui.EzButton("Delete")) {
					_target.GreatEvents.Remove(settings as GreatEvent);
					return false;
				}
			}	
		}
		return true;
	}

	private void DrawGlobalPlatformOptions(ref PlataformBehaviour.PlataformSettings settings) {
		settings.Curve = gui.EzCurveField("", settings.Curve);
		settings.MaxHealth = gui.EzIntField("Health", settings.MaxHealth);
		settings.LoweredTime = gui.EzFloatField("Lowered Time", settings.LoweredTime);
		settings.OscilationFrequency = gui.EzFloatField("Oscilation Frequency", settings.OscilationFrequency);
		settings.OscilationScale = gui.EzFloatField("Oscilation Scale", settings.OscilationScale);
	}
}
#endif
