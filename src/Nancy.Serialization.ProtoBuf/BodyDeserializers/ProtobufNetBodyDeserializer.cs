﻿using ProtoBuf.Meta;

namespace Nancy.Serialization.ProtoBuf.BodyDeserializers
{
    using System;
    using System.IO;
    using System.Linq;
    using ModelBinding;

    /// <summary>
    /// Deserializes request bodies in ProtoBuffer format
    /// </summary>
    public sealed class ProtobufNetBodyDeserializer : IBodyDeserializer
    {
        /// <summary>
        /// Whether the deserializer can deserialize the content type
        /// </summary>
        /// <param name="contentType">Content type to deserialize</param>
        /// <param name="context">Current <see cref="BindingContext"/>.</param>
        /// <returns>True if supported, false otherwise</returns>
        public bool CanDeserialize(string contentType, BindingContext context)
        {
            return IsProtoBufType(contentType);
        }

        /// <summary>
        /// Deserialize the request body to a model
        /// </summary>
        /// <param name="contentType">Content type to deserialize</param>
        /// <param name="bodyStream">Request body stream</param>
        /// <param name="context">Current context</param>
        /// <returns>Model instance</returns>
        public object Deserialize(string contentType, Stream bodyStream, BindingContext context)
        {
            // deserialize the body stream into the destination type
            return RuntimeTypeModel.Default.Deserialize(bodyStream, null, context.DestinationType);
        }

        /// <summary>
        /// Attempts to detect if the content type is ProtoBuffer.
        /// Supports:
        ///   application/x-protobuf
        /// Matches are case insentitive to try and be as "accepting" as possible.
        /// </summary>
        /// <param name="contentType">Request content type</param>
        /// <returns>True if content type is JSON, false otherwise</returns>
        private static bool IsProtoBufType(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return false;
            }

            var contentMimeType = contentType.Split(';').First();

		// We probably have something like application/x-protobuf OR
		// application/vnd.whatever+x-protobuf

		// accept them all!
	        try
	        {
			// Are we just application/x-protobuf?
		        if (contentMimeType.Equals(Constants.ProtoBufContentType, StringComparison.InvariantCultureIgnoreCase))
			        return true;
				
			// Ok, get a little more creative and look for the specific vendor flavor:
			string fSubType = contentMimeType.Split('/').Skip(1).First();

			return (fSubType.StartsWith("vnd", StringComparison.InvariantCultureIgnoreCase) &&
				fSubType.EndsWith("+x-protobuf", StringComparison.InvariantCultureIgnoreCase));
	        }
	        catch (Exception)
	        {
		        return false;
	        }
        }
    }
}
