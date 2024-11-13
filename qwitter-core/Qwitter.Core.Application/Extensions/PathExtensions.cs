using System.Reflection;

namespace Qwitter.Core.Application.Extensions;

public static class PathExtensions
{
    public static string AssemblyPath(string filename) => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, filename);
}