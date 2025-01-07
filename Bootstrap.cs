using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PortfolioViewer.Controllers;
using PortfolioViewer.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PortfolioViewer
{
    /// <summary>
    /// Bootstrap class for initializing controllers in the PortfolioViewer application.
    /// </summary>
    public static class Bootstrap
    {
        /// <summary>
        /// Method called after the scene has loaded to initialize controllers.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            DebugLogger.Log("Bootstrap Initialized");
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Find all MonoBehaviour controllers in the scene
            IEnumerable<IController> monoBehaviourControllers = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
                .OfType<IController>();

            // Find all non-MonoBehaviour controllers
            IEnumerable<Type> nonMonoBehaviourControllerTypes = assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IController).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract && !typeof(MonoBehaviour).IsAssignableFrom(type) && type != typeof(ControllerBase));

            // Initialize MonoBehaviour controllers
            foreach (IController controller in monoBehaviourControllers)
            {
                controller.Init();
            }

            // Initialize non-MonoBehaviour controllers
            foreach (Type type in nonMonoBehaviourControllerTypes)
            {
                IController controller = (IController)Activator.CreateInstance(type);
                controller.Init();
            }
        }
    }
}