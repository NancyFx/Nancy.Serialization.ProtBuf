using Nancy.ModelBinding;
using Nancy.Serialization.ProtoBuf.Demo.Model;

namespace Nancy.Serialization.ProtoBuf.Demo
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Post["/postProtoBuf"] =
                x =>
                    {
                        User data = this.Bind();
                        return Negotiate.WithModel(data);
                    };

            Get["/getProtoBuf/{name}/{age}"] =
                parameters =>
                    {
                        var data = new User { Name = parameters.name, Age = parameters.age };
                        return Negotiate.WithModel(data);
                    };

        }
    }
}