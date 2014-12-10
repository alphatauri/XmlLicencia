using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LhotseBI.Core;

namespace AdministradorDeLicencias
{
    public partial class Form1 : Form
    {
        private string _privateKey;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var admin = new LhotseBILicenseProvider();
            admin.CreateKeyPairs(@".\");
            LoadPrivateKey();
        }

        private void StringToFile(string outfile, string data)
        {
            var outStream = File.CreateText(outfile);
            outStream.Write(data);
            outStream.Close();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_privateKey))
            {
                MessageBox.Show("No existe la clave privada para firmar el contenido");
                return;
            }
            var licenseContent = new StringBuilder();
            licenseContent.Append("<license>");
            licenseContent.AppendFormat("<habilitado>{0}</habilitado>", checkBox1.Checked);
            licenseContent.AppendFormat("<propiedad1>{0}</propiedad1>", chP1.Checked);
            licenseContent.AppendFormat("<propiedad2>{0}</propiedad2>", chP2.Checked);
            licenseContent.AppendFormat("<propiedad3>{0}</propiedad3>", chP3.Checked);
            licenseContent.AppendFormat("<propiedad4>{0}</propiedad4>", chP4.Checked);
            licenseContent.AppendFormat("<propiedad5>{0}</propiedad5>", chP5.Checked);
            licenseContent.AppendFormat("<propiedad6>{0}</propiedad6>", chP6.Checked);
            licenseContent.AppendFormat("<propiedad7>{0}</propiedad7>", chP7.Checked);
            licenseContent.AppendFormat("<propiedad8>{0}</propiedad8>", chP8.Checked);
            licenseContent.AppendFormat("<propiedad9>{0}</propiedad9>", chP9.Checked);
            licenseContent.Append("</license>");

            var license = new LhotseBILicense();

            var fileContent = license.SignXmlDocument(licenseContent.ToString(), _privateKey);

            StringToFile(@".\license.lic", fileContent.OuterXml);
        }

        private void LoadPrivateKey()
        {
            const string privateKeyFileName = @".\private.key";
            if (File.Exists(privateKeyFileName))
                _privateKey = File.ReadAllText(privateKeyFileName);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadPrivateKey();
        }
    }
}
