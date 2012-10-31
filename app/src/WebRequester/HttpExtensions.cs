namespace WebRequester
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    internal static class HttpExtensions
    {
        public static IDictionary<string, string> ToDictionary(this object obj)
        {
            try
            {
                if (obj == null)
                {
                    return new Dictionary<string, string>();
                }
                else if (obj is IDictionary<string, string>)
                {
                    return (IDictionary<string, string>)obj;
                }
                else if (obj is IDictionary<string, object>)
                {
                    var dict = (IDictionary<string, object>)obj;
                    return dict.Where(d => d.Value != null).ToDictionary(d => d.Key, d => d.Value.ToString());
                }
                else
                {
                    return obj.GetType().GetProperties().Where(p => p.GetValue(obj, null) != null).ToDictionary(
                    p => p.Name, p => p.GetValue(obj, null).ToString());    
                }
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        public static string ToGetParameters(this object obj)
        {
            var parameters = obj.ToDictionary();
            
            var builder = new StringBuilder("?");
            foreach (var param in parameters)
            {
                builder.AppendFormat("{0}={1}&", param.Key, param.Value);
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public static byte[] ToPostParameters(this object obj)
        {
            var parameters = obj.ToDictionary();
            var builder = new StringBuilder();
            foreach (var param in parameters)
            {
                builder.AppendFormat("{0}={1}&", param.Key, HttpUtility.UrlEncode(param.Value.ToString()));
            }
            builder.Remove(builder.Length - 1, 1);
            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}
