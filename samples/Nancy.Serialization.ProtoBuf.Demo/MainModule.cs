namespace Nancy.Serialization.ProtoBuf.Demo
{
    using Nancy.ModelBinding;
    using Nancy.Serialization.ProtoBuf.Demo.Model;

    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Post("/postProtoBuf", BuildManagerHostUnloadEventArgs =>
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