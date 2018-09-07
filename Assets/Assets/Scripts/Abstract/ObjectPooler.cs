﻿using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler> {



	[System.Serializable]
	public class Pool {
		public string tag;
		public GameObject prefab;
		public int size;
	}


	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	private void Start() {
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach (var pool in pools) {
			Queue<GameObject> objectPool = new Queue<GameObject>();

			for (int i = 0; i < pool.size; i++) {
				GameObject obj = Instantiate(pool.prefab, transform);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}
			
			poolDictionary.Add(pool.tag, objectPool);
		}
	}

	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {


		if (!poolDictionary.ContainsKey(tag))
			return null;
		
		GameObject obj = poolDictionary[tag].Dequeue();
		if (obj == null)
			obj = Instantiate(poolDictionary[tag].Peek());
		
		obj.SetActive(true);
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		
		poolDictionary[tag].Enqueue(obj);

		return obj;
	}
	
	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent) {

		if (!poolDictionary.ContainsKey(tag))
			return null;
		
		GameObject obj = poolDictionary[tag].Dequeue();

		if (obj == null) {
			foreach (var pool in pools) {
				if (pool.tag != tag) continue;
				obj = Instantiate(pool.prefab);
				break;
			}
		}
			
		
		obj.SetActive(true);
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		obj.transform.parent = parent;

		poolDictionary[tag].Enqueue(obj);
		
		return obj;
	}
}
