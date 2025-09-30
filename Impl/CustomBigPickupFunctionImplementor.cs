using EOSExt.TacticalBigPickup.Definitions.Generic.BigPickup.Definition;
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

        public static void SetupCustomBigPickupFunctions(LG_PickupItem item, List<BigPickupFunction> functions)
        {
            foreach (var f in functions)
            {
                if (s_implementors.TryGetValue(f.Type, out var implementor))
                {
                    implementor.SetupCustomBigPickupFunction(item, f.SettingID);
                    EOSLogger.Log($"ICustomBigPickupFunctionImplementor: function '{f.Type}' applied to {item.name}");
                }
                else
                {
                    EOSLogger.Error($"ICustomBigPickupFunctionImplementor: function '{f.Type}' not found");
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
                var instance = (CustomBigPickupFunctionImplementor)Activator.CreateInstance(i, nonPublic: true);
                if (s_implementors.TryGetValue(instance.FunctionName, out var existing))
                {
                    EOSLogger.Error($"CustomBigPickupFunctionImplementor: Duplicate {instance.FunctionName}!");
                    continue;
                }

                EOSLogger.Log($"CustomBigPickupFunctionImplementor: registered {instance.FunctionName}!");
                s_implementors[instance.FunctionName] = instance;
            }
        }

        protected abstract string FunctionName { get; }

        //private void RegisterImplementor()
        //{
        //    if (s_implementors.ContainsKey(FunctionName))
        //    {
        //        EOSLogger.Warning($"ICustomBigPickupFunctionImplementor: found duplicate function name '{FunctionName}'");
        //    }

        //    s_implementors[FunctionName] = this;
        //}

        public abstract void SetupCustomBigPickupFunction(LG_PickupItem item, uint settingID);

        protected CustomBigPickupFunctionImplementor()
        {
            //RegisterImplementor();
        }
    }
}
