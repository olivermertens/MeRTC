using Org.BouncyCastle.Crypto.Tls;

namespace MeRtc.Base
{
    public interface IDtlsRole
    {
        DtlsTransport Start();
    }
}