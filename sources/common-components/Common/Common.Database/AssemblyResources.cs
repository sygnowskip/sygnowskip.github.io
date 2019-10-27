using System;
using System.IO;
using System.Reflection;

namespace Common.Database
{
    public static class AssemblyResources
    {
        public static string Get(string resourceName)
        {
            var assembly = Assembly.GetAssembly(typeof(AssemblyResources));
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));

                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}