using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LhotseBI.Core
{
    public class LhotseBILicense
    {
        public bool VerifyXmlDocument(string publicKey, string licenseContent)
        {
            RSA key = RSA.Create();
            key.FromXmlString(publicKey);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(licenseContent);
            SignedXml sxml = new SignedXml(doc);
            try
            {
                // Find signature node
                XmlNode sig = doc.GetElementsByTagName("Signature")[0];
                sxml.LoadXml((XmlElement)sig);

                Habilitado = GetPropertyValue(doc, "habilitado");
                Propiedad1 = GetPropertyValue(doc, "propiedad1");
                Propiedad2 = GetPropertyValue(doc, "propiedad2");
                Propiedad3 = GetPropertyValue(doc, "propiedad3");
                Propiedad4 = GetPropertyValue(doc, "propiedad4");
                Propiedad5 = GetPropertyValue(doc, "propiedad5");
                Propiedad6 = GetPropertyValue(doc, "propiedad6");
                Propiedad7 = GetPropertyValue(doc, "propiedad7");
                Propiedad8 = GetPropertyValue(doc, "propiedad8");
                Propiedad9 = GetPropertyValue(doc, "propiedad9");
            }
            catch (Exception)
            {
                // Not signed!
                return false;
            }
            return sxml.CheckSignature(key);
        }

        private bool GetPropertyValue(XmlDocument xml, string propertyName)
        {
            var node = xml.GetElementsByTagName(propertyName);
            var text = node.Count != 0 && !string.IsNullOrEmpty(node[0].InnerText) ? node[0].InnerText : "False";
            return bool.Parse(text);
        }

        public bool Habilitado { get; private set; }
        public bool Propiedad1 { get; private set; }
        public bool Propiedad2 { get; private set; }
        public bool Propiedad3 { get; private set; }
        public bool Propiedad4 { get; private set; }
        public bool Propiedad5 { get; private set; }
        public bool Propiedad6 { get; private set; }
        public bool Propiedad7 { get; private set; }
        public bool Propiedad8 { get; private set; }
        public bool Propiedad9 { get; private set; }

        public XmlDocument SignXmlDocument(string licenseContent, string privateKey)
        {
            RSA key = RSA.Create();
            key.FromXmlString(privateKey);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(licenseContent);

            SignedXml sxml = new SignedXml(doc);
            sxml.SigningKey = key;
            sxml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigCanonicalizationUrl;

            // Add reference to XML data
            Reference r = new Reference("");
            r.AddTransform(new XmlDsigEnvelopedSignatureTransform(false));
            sxml.AddReference(r);

            // Build signature
            sxml.ComputeSignature();

            // Attach signature to XML Document
            XmlElement sig = sxml.GetXml();
            doc.DocumentElement.AppendChild(sig);

            return doc;
        }
    }
}
