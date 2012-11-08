namespace Nancy.Serialization.ProtoBuf
{
    using global::ProtoBuf;

    /// <summary>
    /// Represents a ProtoBuffer response.
    /// </summary>
    public class ProtoBufResponse : Response
    {
        /// <summary>
        /// Initializes a new instance ofthe <see cref="ProtoBufResponse"/> class.
        /// </summary>
        /// <param name="body">Model instance to be serialized as the body.</param>
        public ProtoBufResponse(object body)
        {
            Contents = stream => Serializer.Serialize(stream, body);
            ContentType = Constants.ProtoBufContentType;
            StatusCode = HttpStatusCode.OK;
        }

        /// <summary>
        /// Sets the HttpStatusCode property of the current <see cref="ProtoBufResponse"/> instance
        /// to the specified value.
        /// </summary>
        /// <param name="httpStatusCode">The new http status code</param>
        /// <returns>The modified <see cref="ProtoBufResponse"/> instance</returns>
        public ProtoBufResponse WithStatusCode(HttpStatusCode httpStatusCode)
        {
            StatusCode = httpStatusCode;
            return this;
        }
    }
}