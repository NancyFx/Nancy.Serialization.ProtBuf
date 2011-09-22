namespace Nancy.Serialization.ProtoBuf
{
    /// <summary>
    /// Extension methods for IResponseFormatter
    /// </summary>
    public static class FormatterExtensions
    {
        /// <summary>
        /// Format the response as a <see cref="ProtoBufResponse"/>.
        /// </summary>
        /// <typeparam name="TModel">Type of the model instance</typeparam>
        /// <param name="formatter">Current response formatter</param>
        /// <param name="model">Model instance</param>
        /// <returns>A new <see cref="ProtoBufResponse"/> instance</returns>
        public static Response AsProtoBuf<TModel>(this IResponseFormatter formatter, TModel model)
        {
            return new ProtoBufResponse(model);
        }
    }
}
