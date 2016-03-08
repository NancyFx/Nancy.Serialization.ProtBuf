namespace Nancy.Serialization.ProtoBuf.Demo
{
    using Nancy.ModelBinding;
    using Nancy.Serialization.ProtoBuf.Demo.Model;

    public class MainModule : LegacyNancyModule
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