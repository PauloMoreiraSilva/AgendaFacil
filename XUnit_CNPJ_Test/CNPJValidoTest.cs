using PI4Sem.Infra;
using System;
using Xunit;

namespace XUnit_CNPJ_Test
{
    public class CNPJValidoTest
    {
        [Fact]
        public void ValidarCNPJ()
        {
            Assert.Equal(true, Formats.ValidaCNPJ("12171198000155"));
            Assert.Equal(true, Formats.ValidaCNPJ("50788305000188"));
            Assert.Equal(false, Formats.ValidaCNPJ("abcde"));
            Assert.Equal(false, Formats.ValidaCNPJ(""));
            Assert.Equal(false, Formats.ValidaCNPJ("11111111111111"));
        }

        [Fact]
        public void ValidarCPF()
        {
            Assert.Equal(true, Formats.ValidaCPF("98455250089"));
            Assert.Equal(true, Formats.ValidaCPF("21182805051"));
            Assert.Equal(false, Formats.ValidaCPF("abcde"));
            Assert.Equal(false, Formats.ValidaCPF(""));
            Assert.Equal(false, Formats.ValidaCPF("11111111111"));
        }
    }
}
