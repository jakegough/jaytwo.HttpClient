using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using jaytwo.MimeHelper;

namespace jaytwo.HttpClient
{
    internal static class ContentTypeEvaluator
    {
        public static bool IsJsonContent(HttpResponse httpResponse)
        {
            var contentTypeHeader = httpResponse.GetHeaderValue(Constants.Headers.ContentType);
            return IsJsonContent(contentTypeHeader);
        }

        public static bool IsJsonContent(string contentType)
        {
            var mediaTypeHeader = MediaTypeHeaderValue.Parse(contentType);
            if (mediaTypeHeader.MediaType == MediaType.application_json
                || mediaTypeHeader.MediaType.EndsWith("/json")
                || mediaTypeHeader.MediaType.EndsWith("+json"))
            {
                return true;
            }

            return false;
        }

        public static bool CouldBeJsonContent(HttpResponse httpResponse)
        {
            return CouldBeJsonContent(httpResponse.Content) || CouldBeJsonContent(httpResponse.ContentBytes);
        }

        public static bool CouldBeJsonContent(string content)
        {
            if (content != null)
            {
                return (content.StartsWith("{") && content.EndsWith("}"))
                    || (content.StartsWith("[") && content.EndsWith("]"));
            }

            return false;
        }

        public static bool CouldBeJsonContent(byte[] content)
        {
            if (content != null)
            {
                return (content.First() == '{' && content.Last() == '}')
                    || (content.First() == '[' && content.Last() == ']');
            }

            return false;
        }

        public static bool IsXmlContent(HttpResponse httpResponse)
        {
            var contentTypeHeader = httpResponse.GetHeaderValue(Constants.Headers.ContentType);
            return IsXmlContent(contentTypeHeader);
        }

        public static bool IsXmlContent(string contentType)
        {
            var mediaTypeHeader = MediaTypeHeaderValue.Parse(contentType);
            if (mediaTypeHeader.MediaType == MediaType.text_xml
                || mediaTypeHeader.MediaType.EndsWith("/xml")
                || mediaTypeHeader.MediaType.EndsWith("+xml"))
            {
                return true;
            }

            return false;
        }

        public static bool CouldBeXmlContent(HttpResponse httpResponse)
        {
            return CouldBeXmlContent(httpResponse.Content) || CouldBeXmlContent(httpResponse.ContentBytes);
        }

        public static bool CouldBeXmlContent(string content)
        {
            if (content != null)
            {
                return content.StartsWith("<") && content.EndsWith(">");
            }

            return false;
        }

        public static bool CouldBeXmlContent(byte[] content)
        {
            if (content != null)
            {
                return content.First() == '<' && content.Last() == '>';
            }

            return false;
        }

        public static bool IsBinaryContent(HttpContent content)
        {
            var mediaType = content.Headers?.ContentType?.MediaType ?? string.Empty;
            return IsBinaryMediaType(mediaType);
        }

        public static bool IsBinaryMediaType(string mediaType)
        {
            var knownBinaryMediaTypes = new[]
            {
                MediaType.application_octet_stream,
            };

            var binaryMediaTypePrefixes = new[]
            {
                "image/",
                "audio/",
                "video/",
                "font/",
                "application/vnd.openxmlformats-officedocument.",
                "application/vnd.ms-",
            };

            var binaryMediaTypeSuffixes = new[]
            {
                "/zip",
                "/pdf",
                "-compressed",
            };

            var isKnownBinaryMediaType = knownBinaryMediaTypes.Contains(mediaType);
            var hasBinaryMediaTypePrefix = binaryMediaTypePrefixes.Any(x => mediaType.StartsWith(x));
            var hasBinaryMediaTypeSuffix = binaryMediaTypeSuffixes.Any(x => mediaType.EndsWith(x));

            var result = isKnownBinaryMediaType || hasBinaryMediaTypePrefix || hasBinaryMediaTypeSuffix;
            return result;
        }

        public static bool IsStringContent(HttpContent content)
        {
            var mediaType = content.Headers?.ContentType?.MediaType ?? string.Empty;
            return IsStringMediaType(mediaType);
        }

        public static bool IsStringMediaType(string mediaType)
        {
            var knownStringMediaTypes = new[]
            {
                MediaType.multipart_form_data,
                MediaType.application_x_www_form_urlencoded,
            };

            var stringMediaTypePrefixes = new[]
            {
                "text/",
            };

            var stringMediaTypeSuffixes = new[]
            {
                "/json",
                "+json",
                "/xml",
                "+xml",
                "/javascript",
            };

            var isKnownStringMediaType = knownStringMediaTypes.Contains(mediaType);
            var hasStringMediaTypePrefix = stringMediaTypePrefixes.Any(x => mediaType.StartsWith(x));
            var hasStringMediaTypeSuffix = stringMediaTypeSuffixes.Any(x => mediaType.EndsWith(x));
            var isJsonMediaType = IsJsonContent(mediaType);
            var isXmlMediaType = IsXmlContent(mediaType);

            var isStringMediaType = isKnownStringMediaType || hasStringMediaTypePrefix || hasStringMediaTypeSuffix || isJsonMediaType || isXmlMediaType;
            var isBinaryMediaType = IsBinaryMediaType(mediaType);

            var result = isStringMediaType && !isBinaryMediaType;
            return result;
        }
    }
}
