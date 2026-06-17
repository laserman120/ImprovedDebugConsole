using Sons.Ai.Vail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImprovedDebugConsole.Managers
{
    internal class UtilityManager
    {
        public static List<VailActorTypeId> GetActorTypesOfClass(VailActorClassId classId)
        {
            var result = new List<VailActorTypeId>();

            Array actorTypeIds = Enum.GetValues(typeof(VailActorTypeId));

            foreach (Sons.Ai.Vail.VailActorTypeId actorId in actorTypeIds)
            {
                if (VailTypes.GetActorClass(actorId) == classId)
                {
                    result.Add(actorId);
                }
            }

            return result;
        }
    }
}
