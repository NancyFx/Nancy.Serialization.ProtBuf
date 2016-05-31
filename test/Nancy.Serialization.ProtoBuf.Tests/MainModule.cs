namespace Nancy.Serialization.ProtoBuf.Tests
{
    using Nancy.ModelBinding;

    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Post("/postProtoBuf", args =>
            {
                User data = this.Bind();
                return Negotiate.WithModel(data);
            });

            Get("/getProtoBuf/{name}/{age}", args =>
            {
                var data = new User { Name = args.name, Age = args.age };
                return Negotiate.WithModel(data);
            });
        }
    }
}
