using System.Reflection;

namespace Qwitter.Core.Application.RestApiClient;

public class ParamInfo
{
    public object Value { get; set; }
    public ParameterInfo Info { get; set; }

    public ParamInfo(object value, ParameterInfo info)
    {
        Value = value;
        Info = info;
    }
}