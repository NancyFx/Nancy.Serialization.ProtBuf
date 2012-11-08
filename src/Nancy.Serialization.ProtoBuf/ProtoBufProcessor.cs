namespace Nancy.Serialization.ProtoBuf
{
    using System;
    using System.Collections.Generic;
    using Nancy.Responses.Negotiation;

    public class ProtoBufProcessor : IResponseProcessor
    {
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

        private static bool IsWildcard(MediaRange requestedMediaRange)
        {
            return requestedMediaRange.Type.IsWildcard && requestedMediaRange.Subtype.IsWildcard;
        }

        public Response Process(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return new ProtoBufResponse(model);
        }

        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings
        {
            get
            {
                return new[]
                {
                    Tuple.Create("protobuf", MediaRange.FromString(Constants.ProtoBufContentType))
                };
            }
        }
    }
}