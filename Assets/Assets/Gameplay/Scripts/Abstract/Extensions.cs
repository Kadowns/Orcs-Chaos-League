using System;
using System.Collections.Generic;
using UnityEngine;


namespace OCL {
    
    public static class Extensions {

        public static T AddChildWithComponent<T>(this GameObject go, string name = "child") where T : Component {
            var obj = new GameObject(name);
            obj.transform.SetParent(go.transform);
            return obj.AddComponent<T>();
        }


        public static void ForEach<T>(this Queue<T> queue, Action<T> action) {
            for (byte i = 0; i < queue.Count; i++) {
                var element = queue.Dequeue();
                action(element);
                queue.Enqueue(element);
            }
        }      
    }
}