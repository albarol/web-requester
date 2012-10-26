namespace WebRequester
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    internal static class HttpExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            if (obj == null) 
            {
                return new Dictionary<string, object>();
            }

            if (obj is IDictionary<string, object>)
            {
                return (IDictionary<string, object>)obj;
            }

            return obj.GetType().GetProperties().Where(p => p.GetValue(obj, null) != null).ToDictionary(p => p.Name, p => p.GetValue(obj, null));
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
