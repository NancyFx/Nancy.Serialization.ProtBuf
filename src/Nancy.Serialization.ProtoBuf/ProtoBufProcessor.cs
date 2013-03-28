namespace Nancy.Serialization.ProtoBuf
{
    using System;
    using System.Collections.Generic;
    using Nancy.Responses.Negotiation;

    /// <summary>
    /// An <see cref="IResponseProcessor"/> implementation for ProtoBuffer.
    /// </summary>
    public class ProtoBufProcessor : IResponseProcessor
    {
        /// <summary>
        /// Gets a set of mappings that map a given extension (such as .json)
        /// to a media range that can be sent to the client in a vary header.
        /// </summary>
        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings
        {
            get
            {
                return new[] { Tuple.Create("protobuf", MediaRange.FromString(Constants.ProtoBufContentType)) };
            }
        }

        /// <summary>
        /// Determines whether the the processor can handle a given content type and model.
        /// </summary>
        /// <param name="requestedMediaRange">Content type requested by the client.</param>
        /// <param name="model">The model for the given media range.</param>
        /// <param name="context">The nancy context.</param>
        /// <returns>A <see cref="ProcessorMatch"/> result that determines the priority of the processor.</returns>
        public ProcessorMatch CanProcess(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            if (IsWildcard(requestedMediaRange) || requestedMediaRange.Matches(Constants.ProtoBufContentType))
            {
                return new ProcessorMatch
                {
                    ModelResult = MatchResult.DontCare,
                    RequestedContentTypeResult = MatchResult.ExactMatch
                };
            }

            return new ProcessorMatch
            {
                ModelResult = MatchResult.DontCare,
                RequestedContentTypeResult = MatchResult.NoMatch
            };
        }
        
        /// <summary>
        /// Process the response.
        /// </summary>
        /// <param name="requestedMediaRange">Content type requested by the client.</param>
        /// <param name="model">The model for the given media range.</param>
        /// <param name="context">The nancy context.</param>
        /// <returns>A <see cref="Response"/> instance.</returns>
        public Response Process(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return new ProtoBufResponse(model);
        }
        
        private static bool IsWildcard(MediaRange requestedMediaRange)
        {
            return requestedMediaRange.Type.IsWildcard && requestedMediaRange.Subtype.IsWildcard;
        }
    }
}