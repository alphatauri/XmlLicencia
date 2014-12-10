using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LhotseBI.Core
{
    public class LhotseBILicenseProvider
    {
        public string LoadKeyPairs(string file)
        {
            var stream = File.OpenText(file);
            return stream.ReadToEnd();
        }
        public void CreateKeyPairs(string directory)
        {
            var key = new RSACryptoServiceProvider(2048);

            var publicPrivateKeyXml = key.ToXmlString(true);
            var publicOnlyKeyXml = key.ToXmlString(false);

            StringToFile(@".\public.key", publicOnlyKeyXml);
            StringToFile(@".\private.key", publicPrivateKeyXml);
        }
        private static void StringToFile(string outfile, string data)
        {
            StreamWriter outStream = File.CreateText(outfile);
            outStream.Write(data);
            outStream.Close();
        }
    }
}
