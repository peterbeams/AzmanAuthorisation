using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lockdown.AcceptanceTests.Common
{
    public static class Resources
    {
        public static string LoadResourceFromAssemblyContainingType(this Type t, string resourceName)
        {
            var assem = t.Assembly;
            using (var stream = assem.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
