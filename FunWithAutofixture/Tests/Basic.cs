using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace FunWithAutofixture.Tests
{
    public class Basic
    {
        private readonly Mimicker _sut = new Mimicker();
        private IFixture _fixture = new Fixture();
        private readonly ITestOutputHelper _output;

        public Basic(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestString()
        {
            //Arrange
            var s = _fixture.Create<string>();
            _output.WriteLine(s);
            //Act
            var result = _sut.Echo(s);
            //Assert
            result.Should().Be(s);
        }

        [Fact]
        public void TestInt()
        {
            //Arrange
            var i = _fixture.Create<int>();
            _output.WriteLine(i.ToString());
            //Act
            var result = _sut.Echo(i);
            //Assert
            result.Should().Be(i);
        }

        [Fact]
        public void TestCopyCat()
        {
            //Arrange
            var value = _fixture.Create<DummyClass>();
            //Act
            var result = _sut.CopyCat(value);
            //Assert
            value.Should().NotBeSameAs(result);
            value.Should().BeEquivalentTo(result);
        }

        [Fact]
        public void SeededText()
        {
            //Arrange
            var seededText = _fixture.Create("kunduz");
            _output.WriteLine(seededText);
        }

        [Fact]
        public void ClientWithFixedName()
        {
            var client = _fixture.Build<Client>().With(c => c.Name, "Necati").Create();
            _output.WriteLine(JsonConvert.SerializeObject(client));

            Assert.True(true);
        }

        [Fact]
        public void RegisterExample()
        {
            _fixture.Register(() => new Client { Name = "Necati" });

            var c1 = _fixture.Create<Client>();
            var c2 = _fixture.Create<Client>();

            _output.WriteLine(JsonConvert.SerializeObject(c1));
            _output.WriteLine(JsonConvert.SerializeObject(c2));

            Assert.True(true);
        }

        [Fact]
        public void SpecimenBuilderExample()
        {
            _fixture = new Fixture();

            _fixture.Customizations.Add(new ErgenBuilder());
            var e1 = _fixture.Create<Client>();

            _output.WriteLine(JsonConvert.SerializeObject(e1));

            Assert.True(true);
        }

        [Fact]
        public void CustomiziationExample()
        {
            _fixture = new Fixture().Customize(new ErgenCustomization());

            var e2 = _fixture.Create<Client>();

            _output.WriteLine(JsonConvert.SerializeObject(e2));
            Assert.True(true);
        }







        public class DummyClass
        {
            public string S1 { get; set; }
            public string S2 { get; set; }
            public string S3 { get; set; }
            public string S4 { get; set; }
            public string S5 { get; set; }
            public string S6 { get; set; }
            public string S7 { get; set; }

        }

        public class Client
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class ErgenBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                if (!(request is Type type && type == typeof(Client))) return new NoSpecimen();

                var rnd = new Random();

                return new Client
                {
                    Age = rnd.Next(14, 20),
                    Name = context.Create<string>()
                };
            }
        }

        public class ErgenCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customizations.Add(new ErgenBuilder());
            }
        }
    }
}
