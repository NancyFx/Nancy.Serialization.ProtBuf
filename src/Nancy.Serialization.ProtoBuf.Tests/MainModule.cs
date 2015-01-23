
namespace Nancy.Serialization.ProtoBuf.Tests
{
    using Nancy.ModelBinding;

    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Post["/postProtoBuf"] = parameters =>
            {
                User data = this.Bind();
                return Negotiate.WithModel(data);
            };

            Get["/getProtoBuf/{name}/{age}"] = parameters =>
            {
                var data = new User { Name = parameters.name, Age = parameters.age };
                return Negotiate.WithModel(data);
            };
        }
    }
}