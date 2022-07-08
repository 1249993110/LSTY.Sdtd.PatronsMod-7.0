using LSTY.Sdtd.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LSTY.Sdtd.PatronsMod.Extensions
{
    static class Vector3Extension
    {
        public static Position ToPosition(this Vector3 v)
        {
            return new Position(v.x, v.y, v.z);
        }

        public static Position ToPosition(this Vector3i v)
        {
            return new Position(v.x, v.y, v.z);
        }
    }
}
