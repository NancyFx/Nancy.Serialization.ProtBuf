namespace Nancy.Serialization.ProtoBuf
{
    public static class FormatterExtensions
    {
        public static Response AsProtoBuf<TModel>(this IResponseFormatter formatter, TModel model)
        {
            return new ProtoBufResponse(model);
        }
    }
}
