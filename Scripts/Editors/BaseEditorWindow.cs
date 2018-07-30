using System;
using CCore.Input;
using UnityEditor;
using UnityEngine;

namespace CCore.Editors
{    
    public abstract class BaseEditorWindow : EditorWindow
    {
        protected Rect windowRect { get { return position; } }
        
        public static void ShowWindow<T>()
        {
            EditorWindow.GetWindow(typeof(T));
        }

        protected virtual void Update()
        {

        }
        
        protected virtual void OnGUI()
        {
            Event e = Event.current;

            if (e.type == EventType.MouseDown)
            {
                OnMouseDown(e.mousePosition, (GUIMouseButton)e.button);
            }
            
            if (e.type == EventType.MouseDrag)
            {
                OnMouseDrag(e.mousePosition, (GUIMouseButton)e.button);
            }

            if (e.type == EventType.MouseUp)
            {
                OnMouseUp(e.mousePosition, (GUIMouseButton)e.button);
            }
        }

        protected abstract void OnMouseDown(Vector2 position, GUIMouseButton mouseButton);

        protected abstract void OnMouseDrag(Vector2 position, GUIMouseButton mouseButton);

        protected abstract void OnMouseUp(Vector2 position, GUIMouseButton mouseButton);
    }
}
