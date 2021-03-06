using System.Collections.Specialized;
using System.Text;
using System.Web.Mvc;
using CodeConverters.Core.Diagnostics;

namespace CodeConverters.Mvc.Diagnostics
{
    public class MvcErrorLogEvent
    {
        private readonly string _httpMethod;
        private readonly string _url;
        private readonly NameValueCollection _formData;
        private readonly NameValueCollection _headers;
        private readonly string _jsonPayload;

        public MvcErrorLogEvent(ExceptionContext context)
        {
            _httpMethod = context.HttpContext.Request.HttpMethod;
            _headers = context.HttpContext.Request.Headers;
            _url = context.RequestContext.HttpContext.Request.RawUrl;
            _formData = context.HttpContext.Request.Form ?? new NameValueCollection();
            if (context.HttpContext.Request.ContentType == "application/json")
                _jsonPayload = Json.GetPayload(context.HttpContext.Request);
        }

        public override string ToString()
        {
            var logEvent = new StringBuilder();
            logEvent.AppendFormat("Url={0}|", _url);
            logEvent.AppendFormat("Method={0}|", _httpMethod);
            logEvent.AppendFormat("Headers={0}|", _headers.Scrub().ToLogFormat());
            if (_formData.HasKeys())
                logEvent.AppendFormat("FormData={0}|", _formData.Scrub().ToLogFormat());
            if (!string.IsNullOrWhiteSpace(_jsonPayload))
                logEvent.AppendFormat("JsonPayload={0}|", _jsonPayload.ScrubJson());
            return logEvent.ToString();
        }
    }
}