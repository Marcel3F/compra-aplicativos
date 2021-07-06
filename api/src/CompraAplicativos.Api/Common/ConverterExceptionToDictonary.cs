using System;
using System.Collections.Generic;
using System.Linq;

namespace CompraAplicativos.Api.Common
{
    public static class ConverterExceptionToDictonary
    {
        public static Dictionary<string, string[]> ToDictionary<T>(this T exception) where T : Exception
        {
            Dictionary<string, IList<string>> erros = new Dictionary<string, IList<string>>
            {
                [typeof(T).Name] = new List<string>()
            };
            erros[typeof(T).Name].Add(exception.Message);
            return erros.ToDictionary(item => item.Key, item => item.Value.ToArray());
        }
    }
}
