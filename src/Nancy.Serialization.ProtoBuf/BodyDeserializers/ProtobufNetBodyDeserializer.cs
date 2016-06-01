namespace Nancy.Serialization.ProtoBuf.BodyDeserializers
{
    using System;
    using System.IO;
    using ModelBinding;
    using Responses.Negotiation;
    using global::ProtoBuf.Meta;

    /// <summary>
    /// Deserializes request bodies in ProtoBuffer format
    /// </summary>
    public sealed class ProtobufNetBodyDeserializer : IBodyDeserializer
    {
        /// <summary>
        /// Whether the deserializer can deserialize the content type
        /// </summary>
        /// <param name="mediaRange">Content type to deserialize</param>
        /// <param name="context">Current <see cref="BindingContext"/>.</param>
        /// <returns>True if supported, false otherwise</returns>
        public bool CanDeserialize(MediaRange mediaRange, BindingContext context)
        {
            return IsProtoBufType(mediaRange);
        }

        /// <summary>
        /// Deserialize the request body to a model
        /// </summary>
        /// <param name="mediaRange">Content type to deserialize</param>
        /// <param name="bodyStream">Request body stream</param>
        /// <param name="context">Current context</param>
        /// <returns>Model instance</returns>
        public object Deserialize(MediaRange mediaRange, Stream bodyStream, BindingContext context)
        {
            if (bodyStream.CanSeek)
            {
                bodyStream.Position = 0;
            }

            return RuntimeTypeModel.Default.Deserialize(bodyStream, null, context.DestinationType);
        }

        /// <summary>
        /// Attempts to detect if the content type is ProtoBuffer.
        /// Supports:
        ///   application/x-protobuf
        /// Matches are case insentitive to try and be as "accepting" as possible.
        /// </summary>
        /// <param name="mediaRange">Request content type</param>
        /// <returns>True if content type is JSON, false otherwise</returns>
        private static bool IsProtoBufType(MediaRange mediaRange)
        {
            if (string.IsNullOrWhiteSpace(mediaRange))
            {
                return false;
            }

	        try
	        {
		        if (mediaRange.Type.Matches(Constants.ProtoBufContentType))
		        {
		            return true;
		        }

	            var subType =
	                mediaRange.Subtype.ToString();

                return (subType.StartsWith("vnd", StringComparison.OrdinalIgnoreCase)
                    && subType.EndsWith("+x-protobuf", StringComparison.OrdinalIgnoreCase));
	        }
	        catch (Exception)
	        {
		        return false;
	        }
        }
    }
}
