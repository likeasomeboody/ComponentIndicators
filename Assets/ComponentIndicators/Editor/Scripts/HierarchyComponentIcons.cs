using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ComponentIndicators
{
    [InitializeOnLoad]
    public partial class HierarchyComponentIcons
    {
        static Dictionary<GameObject, List<Type>> MarkedObjects = new Dictionary<GameObject, List<Type>>();

        static Dictionary<Type, Texture2D> ComponentIcons = new Dictionary<Type, Texture2D>();

        /// <summary>
        /// Constructor
        /// </summary>
        static HierarchyComponentIcons()
        {
            if (ComponentIndicatorSettings.ComponentEnabledSetting[typeof(HierarchyComponentIcons)])
            {
                // Initialize
                MarkedObjects = new Dictionary<GameObject, List<Type>>();
                EditorApplication.update += UpdateObjects;
                EditorApplication.hierarchyWindowItemOnGUI += UpdateHierarchy;

                // load textures
                foreach (var type in ComponentIndicatorSettings.ComponentIconPaths.Keys)
                {
                    var path = ComponentIndicatorSettings.ComponentIconPaths[type];
                    var tex = (Texture2D)Resources.Load(path);
                    ComponentIcons.Add(type, tex);
                }
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~HierarchyComponentIcons()
        {
            // dispose of created textures
            foreach(var tex in ComponentIcons.Values)
            {
                UnityEngine.Object.Destroy(tex);
            }
        }

        /// <summary>
        /// Update list of objects to be drawn
        /// </summary>
        static void UpdateObjects()
        {
            if (ComponentIndicatorSettings.ComponentEnabledSetting[typeof(HierarchyComponentIcons)])
            {
                // get all gameobjects
                var allGameObjects = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
                MarkedObjects = new Dictionary<GameObject, List<Type>>();

                // add them and their relevant types to the dictionary
                foreach (GameObject g in allGameObjects)
                {
                    var components = g.GetComponents<Component>();
                    var componentTypes = components.Select(c => c.GetType());
                    foreach (var type in ComponentIcons.Keys)
                    {
                        if (componentTypes.Contains(type))
                        {
                            if (!MarkedObjects.ContainsKey(g))
                            {
                                MarkedObjects.Add(g, new List<Type>());
                            }
                            MarkedObjects[g].Add(type);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draw the textures
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="selectionRect"></param>
        static void UpdateHierarchy(int instanceID, Rect selectionRect)
        {
            if (ComponentIndicatorSettings.ComponentEnabledSetting[typeof(HierarchyComponentIcons)])
            {
                // get the gameobject
                var gameObject = MarkedObjects.Keys.SingleOrDefault(g => g.GetInstanceID() == instanceID);

                if (gameObject == null)
                {
                    return;
                }

                if (!MarkedObjects.Keys.Contains(gameObject))
                {
                    return;
                }

                Rect r = new Rect(selectionRect);
                r.width = 18;
                r.x = selectionRect.width;

                foreach (var type in MarkedObjects[gameObject])
                {
                    // place the icoon to the right of the list:
                    r.x -= 20;
                    var tex = ComponentIcons[type];
                    GUI.Label(r, tex);
                }
            }
        }
    }
}
