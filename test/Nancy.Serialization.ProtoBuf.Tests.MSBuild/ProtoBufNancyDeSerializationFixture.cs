namespace Nancy.Serialization.ProtoBuf.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Nancy.ModelBinding;
    using Nancy.Serialization.ProtoBuf.BodyDeserializers;
    using global::ProtoBuf;
    using global::ProtoBuf.Meta;
    using Xunit;


    public class ProtoBufNancyDeSerializationFixture
    {
        private readonly BindingContext bindingContext = new BindingContext
        {
            DestinationType = typeof(User),
            ValidModelBindingMembers =
                typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => new BindingMemberInfo(p)),
        };

        static ProtoBufNancyDeSerializationFixture()
        {
            RuntimeTypeModel.Default.Add(typeof(User), false).Add("Name", "Age");
        }

        [Fact]
        public void when_deserializing()
        {
            // Given
            var bodyStream = new MemoryStream();
            using (bodyStream)
            {
                var sourceUser = GetRandomUser();
                Serializer.Serialize(bodyStream, sourceUser);
                bodyStream.Position = 0;

                // When
                IBodyDeserializer sut = new ProtobufNetBodyDeserializer();
                var actual = sut.Deserialize(Constants.ProtoBufContentType, bodyStream, bindingContext);

                // Then
                var actualData = Assert.IsType<User>(actual);
                Assert.Equal(sourceUser.Age, actualData.Age);
                Assert.Equal(sourceUser.Name, actualData.Name);
            }
        }

        [Fact]
        public void when_deserializing_while_the_body_stream_was_not_at_position_zero()
        {
            // Given
            var bodyStream = new MemoryStream();
            using (bodyStream)
            {
                var sourceUser = GetRandomUser();
                Serializer.Serialize(bodyStream, sourceUser);
                bodyStream.Position = 1;

                // When
                IBodyDeserializer sut = new ProtobufNetBodyDeserializer();
                var actual = sut.Deserialize(Constants.ProtoBufContentType, bodyStream, bindingContext);

                // Then
                var actualData = Assert.IsType<User>(actual);
                Assert.Equal(sourceUser.Age, actualData.Age);
                Assert.Equal(sourceUser.Name, actualData.Name);
            }
        }

        private static User GetRandomUser()
        {
            return new User
            {
                Age = new Random().Next(),
                Name = Guid.NewGuid().ToString()
            };
        }
    }
}