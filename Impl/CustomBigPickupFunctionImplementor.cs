using ExtraObjectiveSetup.Utils;
using Il2CppSystem.Data;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSExt.TacticalBigPickup.Impl
{
    /**
     * To implememt new function, just inherit this class and override FunctionName and SetupCustomBigPickupFunction
     */
    public abstract class CustomBigPickupFunctionImplementor
    {
        private static Dictionary<string, CustomBigPickupFunctionImplementor> s_implementors = new();

        public static void SetupCustomBigPickupFunctions(LG_PickupItem item, List<string> functions)
        {
            foreach (var f in functions)
            {
                if (s_implementors.TryGetValue(f, out var implementor))
                {
                    implementor.SetupCustomBigPickupFunction(item);
                }
                else
                {
                    EOSLogger.Error($"ICustomBigPickupFunctionImplementor: function '{f}' not found");
                }
            }
        }

        static CustomBigPickupFunctionImplementor()
        {
            var implTypes = typeof(CustomBigPickupFunctionImplementor).Assembly.GetTypes()
                        .Where(x => !x.IsAbstract)
                        .Where(x => x.IsAssignableTo(typeof(CustomBigPickupFunctionImplementor)));

            foreach (var i in implTypes)
            {
                var instance = (CustomBigPickupFunctionImplementor)Activator.CreateInstance(i);
                if (s_implementors.TryGetValue(instance.FunctionName, out var existing))
                {
                    EOSLogger.Error($"CustomBigPickupFunctionImplementor: Duplicate {nameof(instance.FunctionName)}!");
                    continue;
                }

                s_implementors[instance.FunctionName] = instance;
            }
        }

        protected abstract string FunctionName { get; }

        private void RegisterImplementor()
        {
            if (s_implementors.ContainsKey(FunctionName))
            {
                EOSLogger.Warning($"ICustomBigPickupFunctionImplementor: found duplicate function name '{FunctionName}'");
            }

            s_implementors[FunctionName] = this;
        }

        public abstract void SetupCustomBigPickupFunction(LG_PickupItem item);

        protected CustomBigPickupFunctionImplementor()
        {
            RegisterImplementor();
        }
    }
}
