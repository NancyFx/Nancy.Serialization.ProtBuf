using System;
using System.IO;
using System.Linq;
using Nancy.ModelBinding;
using ProtoBuf.Meta;

namespace Nancy.Serialization.ProtoBuf.BodyDeserializers
{
    public sealed class ProtobufNetBodyDeserializer : IBodyDeserializer
    {
        public bool CanDeserialize(string contentType)
        {
            return IsProtoBufType(contentType);
        }

        public object Deserialize(string contentType, Stream bodyStream, BindingContext context)
        {
            // deserialize the body stream into the destination type
            return RuntimeTypeModel.Default.Deserialize(bodyStream, null, context.DestinationType);
        }

        private static bool IsProtoBufType(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return false;
            }

            var contentMimeType = contentType.Split(';').First();

            return contentMimeType.Equals(
                Constants.ProtoBufContentType, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}