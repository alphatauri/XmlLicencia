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
using LhotseBI.Core;

namespace AplicaciónLicenciada
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string FileToString(string infile)
        {
            return File.ReadAllText(infile);
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            const string publicKeyFileName = @".\public.key";
            const string licenseFileName = @".\license.lic";
            var publicKeyExists = File.Exists(publicKeyFileName);
            var licenseFileExists = File.Exists(licenseFileName);

            if (!publicKeyExists || !licenseFileExists)
            {
                MessageBox.Show(string.Format("{0}\n{1}", publicKeyExists ? "" : "No existe el archivo de clave", licenseFileExists ? "" : "El archivo de licencia no existe"));
                return;
            }

            var license = new LhotseBILicense();
            var licenseContent = FileToString(licenseFileName);
            var publicKey = FileToString(publicKeyFileName);

            var valid = license.VerifyXmlDocument(publicKey, licenseContent);
            if (!valid)
            {
                MessageBox.Show("Archivo de licencia alterado.");
                return;
            }

            if (!license.Habilitado)
            {
                MessageBox.Show("No está habilitado para utilizar esta aplicación");
                return;
            }

            button1.Enabled = license.Propiedad1;
            button2.Enabled = license.Propiedad2;
            button3.Enabled = license.Propiedad3;
            button4.Enabled = license.Propiedad4;
            button5.Enabled = license.Propiedad5;
            button6.Enabled = license.Propiedad6;
            button7.Enabled = license.Propiedad7;
            button8.Enabled = license.Propiedad8;
            button9.Enabled = license.Propiedad9;
        }
    }
}
