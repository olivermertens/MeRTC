using System;
using Org.BouncyCastle.Crypto.Tls;

namespace MeRtc.Base
{
    class TlsAuthenticationImplementor : TlsAuthentication
        {
            private Func<CertificateRequest, TlsCredentials> getClientCredentialsFunc;
            private Action<Certificate> notifyServerCertificateAction;
            public TlsCredentials GetClientCredentials(CertificateRequest certificateRequest) => getClientCredentialsFunc(certificateRequest);
            public void NotifyServerCertificate(Certificate serverCertificate) => notifyServerCertificateAction(serverCertificate);

            public TlsAuthenticationImplementor(Func<CertificateRequest, TlsCredentials> getClientCredentials, Action<Certificate> notifyServerCertificate)
            {
                this.getClientCredentialsFunc = getClientCredentials;
                this.notifyServerCertificateAction = notifyServerCertificate;
            }
        }
}