using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;

namespace Boring.Mvc
{
    // From https://stackoverflow.com/questions/50918007/accepting-byte-in-a-net-core-webapi-controller
    public class BinaryInputFormatter : InputFormatter
    {
        const string binaryContentType = "application/octet-stream";
        const int bufferLength = 16384;

        public BinaryInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(binaryContentType));
        }

        public async override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            using (MemoryStream ms = new MemoryStream(bufferLength))
            {
                await context.HttpContext.Request.Body.CopyToAsync(ms);
                object result = ms.ToArray();
                return await InputFormatterResult.SuccessAsync(result);
            }
        }

        protected override bool CanReadType(Type type)
        {
            if (type == typeof(byte[]))
                return true;
            else
                return false;
        }
    }
}
