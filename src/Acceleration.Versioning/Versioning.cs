using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace Acceleration.Versioning
{
    /// <summary>
    /// version information
    /// </summary>
    public interface IVersionInfo {
        string InformationalVersion { get; }
        string Version { get; }
    }

    /// <summary>
    /// Helpers and extensions to get version information from 
    /// assembly attributes.
    /// </summary>
    public class VersionInfo : IVersionInfo
    {
        public string InformationalVersion { get; set; }
        public string Version { get; set; }
        private VersionInfo() { }
        /// <summary>
        /// Version info for the assembly containing <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [SuppressMessage("Gendarme.Rules.Design.Generic",
            "AvoidMethodWithUnusedGenericTypeRule",
            Justification = "simple API is nice")]
        public static IVersionInfo Get<T>() {
            return Get(typeof(T));
        }

        /// <summary>
        /// Version info for the assembly containing <paramref name="type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [SuppressMessage("Gendarme.Rules.Portability",
            "MonoCompatibilityReviewRule",
            Justification = "checking type == null is fine")]
        public static IVersionInfo Get(Type type) {
            if (type == null) throw new ArgumentNullException("type");
            return Get(type.Assembly);
        }

        /// <summary>
        /// Version info for the <paramref name="assembly"/>
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IVersionInfo Get(Assembly assembly) {
            if (assembly == null) throw new ArgumentNullException("assembly");
            var v = new VersionInfo();
            var inf = assembly
                .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), true)
                .Select(m => m as AssemblyInformationalVersionAttribute)
                .FirstOrDefault(m => m != null);                
            if (inf != null) v.InformationalVersion = inf.InformationalVersion;
            v.Version = assembly.GetName().Version.ToString();
            return v;
        }

        
    }
}
