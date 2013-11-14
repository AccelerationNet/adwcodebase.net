using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Acceleration.Extensions;

namespace Acceleration.Build
{
    public interface IVersionInfo {
        string InformationalVersion { get; }
        string Version { get; }
    }

    sealed class VersionInfo : IVersionInfo {
        public string InformationalVersion { get; set; }
        public string Version { get; set; }
    }

    public static class Versioning
    {
        /// <summary>
        /// Version info for the assembly containing <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IVersionInfo VersionInfo<T>() {
            return VersionInfo(typeof(T));
        }

        /// <summary>
        /// Version info for the assembly containing <paramref name="type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IVersionInfo VersionInfo(Type type) {
            if (type == null) throw new ArgumentNullException("type");
            return VersionInfo(type.Assembly);
        }

        /// <summary>
        /// Version info for the <paramref name="assembly"/>
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IVersionInfo VersionInfo(Assembly assembly) {
            if (assembly == null) throw new ArgumentNullException("assembly");
            var v = new VersionInfo();
            var inf = assembly.GetAttribute<AssemblyInformationalVersionAttribute>();
            if (inf != null) v.InformationalVersion = inf.InformationalVersion;
            v.Version = assembly.GetName().Version.ToString();
            return v;
        }
    }
}
