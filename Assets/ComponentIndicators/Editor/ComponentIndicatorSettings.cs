using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComponentIndicators
{
    public static class ComponentIndicatorSettings {

        /// <summary>
        /// This is the dictionary used by the component indicator class to draw textures.
        /// To add a new icon, add a new entry to this dictionary with the type of the component
        /// and the path to the texture you wish to use. 
        /// </summary>
        public static Dictionary<Type, string> ComponentIconPaths = new Dictionary<Type, string>()
        {
            {typeof(Camera), "ComponentIndicators/camera" },
            {typeof(Rigidbody), "ComponentIndicators/rigidbody" }
        };

        /// <summary>
        /// This dicitionary affects the enabled status of each of the scripts that use this. 
        /// </summary>
        public static Dictionary<Type, bool> ComponentEnabledSetting = new Dictionary<Type, bool>()
        {
            {typeof(HierarchyComponentIcons), true},
        };
    }
}
