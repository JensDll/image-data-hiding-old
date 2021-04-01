using Application.API.Interfaces;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.IntegrationTest
{
    public class EncodeDecodeTest
    {
        public IEncodeService EncodeService { get; }

        public IDecodeService DecodeService { get; }

        public static EncodeDecodeTheoryData Data = new();

        public EncodeDecodeTest(IEncodeService encodeService, IDecodeService decodeService)
        {
            EncodeService = encodeService;
            DecodeService = decodeService;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncodeDecode_MessagesShouldBeTheSame(byte[] message)
        {
            var image = new Bitmap(100, 100);

            EncodeService.EnocodeMessage(image, message);

            var decodedMessage = DecodeService.DecodeMessage(image);

            Assert.Equal(message, decodedMessage);
        }

        [Fact]
        public void EncodeDecode_ShouldThrowMessageToLongExceptionWhenMessageDoesNotFit()
        {
            var rand = new Random();
            var image = new Bitmap(100, 100);

            var message = new byte[30000];
            rand.NextBytes(message);

            Assert.Throws<MessageToLongException>(() => EncodeService.EnocodeMessage(image, message));
        }
    }

    public class EncodeDecodeTheoryData : TheoryData<byte[]>
    {
        public EncodeDecodeTheoryData()
        {
            var rand = new Random(420);

            for (int i = 1000; i <= 29000; i += 7000)
            {
                var message = new byte[i];
                rand.NextBytes(message);
                AddRow(message);
            }
        }
    }
}
