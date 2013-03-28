namespace Nancy.Serialization.ProtoBuf.Demo.Model
{
    using System.Runtime.Serialization;
    
    [DataContract]
    public class User
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public int Age { get; set; }
    }
}