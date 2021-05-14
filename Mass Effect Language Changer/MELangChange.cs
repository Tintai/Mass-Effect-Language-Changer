using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
using System.IO;
using System.Xml;
namespace Mass_Effect_Language_Changer
{
    public partial class MELangChange : Form
    {
        #region Main
        int OSArchitecture()
        {
            string pa = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            return ((String.IsNullOrEmpty(pa) || String.Compare(pa, 0, "x86", 0, 3, true) == 0) ? 32 : 64);
        }
        string ME1path = null;
        string ME2path = null;
        bool ME1click = true; bool ME1open = false;
        bool ME2click = true; bool ME2open = false;
        public MELangChange()
        {
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(304, 194);
        }
        #endregion

        #region SelectGame
        private void button_ME1_Click(object sender, EventArgs e)
        {
            ME1checkregistry();
            pictureBox_ME1gamefullPL.Parent = button_ME1gamefullPL; pictureBox_ME1gamefullPL.Location = new Point(0, 0);
            pictureBox_ME1gamesubtitlesPL.Parent = button_ME1gamesubtitlesPL; pictureBox_ME1gamesubtitlesPL.Location = new Point(0, 0);
        }
        private void button_ME2_Click(object sender, EventArgs e)
        {
            ME2checkregistry();
            pictureBox_ME2gamefullPL.Parent = button_ME2gamefullPL; pictureBox_ME2gamefullPL.Location = new Point(0, 0);
            pictureBox_ME2gamesubtitlesPL.Parent = button_ME2gamesubtitlesPL; pictureBox_ME2gamesubtitlesPL.Location = new Point(0, 0);
            pictureBox_registryPL.Parent = button_ME2registryPL; pictureBox_registryPL.Location = new Point(0, 0);
            pictureBox_registryEN.Parent = button_ME2registryEN; pictureBox_registryEN.Location = new Point(0, 0);
        }
        #endregion

        #region Mass Effect 1

        #region Game Files
        private void button_ME1gamefullPL_Click(object sender, EventArgs e)
        {
            string filetowrite = ME1path + @"\BioGame\Config\DefaultEngine.ini";
            try
            {
                string configfile = filetowrite;
                string tempfile = filetowrite + "_tmp";
                int line_number = 1;
                string line = null;
                using (StreamReader reader = new StreamReader(configfile))
                using (StreamWriter writer = new StreamWriter(tempfile))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line_number == 10)
                            writer.WriteLine("Language=PLPC");
                        else
                            writer.WriteLine(line);
                        line_number++;
                    }
                }
                File.Copy(filetowrite + "_tmp", filetowrite, true);
                File.Delete(filetowrite + "_tmp");
                string xmlfile = ME1path + @"\data\MassEffectLauncher.xml";
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlfile);
                XmlNode plstringNode = xmlDocument.SelectSingleNode("BioWareApp/application/languagetable/strings[@language='pl']/string[@name='play']");
                plstringNode.InnerText = "Graj";
                XmlNode conditionsNode = xmlDocument.SelectSingleNode("BioWareApp/conditions/condition[@name='LanguageSet']/case[@casevalue='INT']");
                conditionsNode.Attributes["value"].Value = "en";
                xmlDocument.Save(xmlfile);
                button_ME1gamefullPL.Enabled = false; pictureBox_ME1gamefullPL.Visible = true; button_ME1gamesubtitlesPL.Enabled = true; pictureBox_ME1gamesubtitlesPL.Visible = false;
            }
            catch { MessageBox.Show("Nie można odczytać pliku \"DefaultEngine.ini\"", "Mass Effect Language Changer", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void button_ME1gamesubtitlesPL_Click(object sender, EventArgs e)
        {
            string filetowrite = ME1path + @"\BioGame\Config\DefaultEngine.ini";
            try
            {
                string configfile = filetowrite;
                string tempfile = filetowrite + "_tmp";
                int line_number = 1;
                string line = null;
                using (StreamReader reader = new StreamReader(configfile))
                using (StreamWriter writer = new StreamWriter(tempfile))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line_number == 10)
                            writer.WriteLine("Language=PL");
                        else
                            writer.WriteLine(line);
                        line_number++;
                    }
                }
                File.Copy(filetowrite + "_tmp", filetowrite, true);
                File.Delete(filetowrite + "_tmp");
                string xmlfile = ME1path + @"\data\MassEffectLauncher.xml";
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlfile);
                XmlNode plstringNode = xmlDocument.SelectSingleNode("BioWareApp/application/languagetable/strings[@language='pl']/string[@name='play']");
                plstringNode.InnerText = "Graj (Dubbing ENG)";
                XmlNode conditionsNode = xmlDocument.SelectSingleNode("BioWareApp/conditions/condition[@name='LanguageSet']/case[@casevalue='INT']");
                conditionsNode.Attributes["value"].Value = "pl";
                xmlDocument.Save(xmlfile);
                button_ME1gamefullPL.Enabled = true; pictureBox_ME1gamefullPL.Visible = false; button_ME1gamesubtitlesPL.Enabled = false; pictureBox_ME1gamesubtitlesPL.Visible = true;
            }
            catch { MessageBox.Show("Nie można odczytać pliku \"DefaultEngine.ini\"", "Mass Effect Language Changer", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        #endregion

        #region References
        private void ME1checkregistry()
        {
            if (OSArchitecture() == 32)
            {
                RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\BioWare\Mass Effect\");
                if (Key == null)
                {
                    MessageBox.Show("Nie wykryto instalacji Mass Effect", "Mass Effect Language Changer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    ME1path = Key.GetValue("Path").ToString();
                    ME1checkgamefiles();
                }
            }
            else if (OSArchitecture() == 64)
            {
                RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\BioWare\Mass Effect\");
                if (Key == null)
                {
                    MessageBox.Show("Nie wykryto instalacji Mass Effect", "Mass Effect Language Changer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    ME1path = Key.GetValue("Path").ToString();
                    ME1checkgamefiles();
                }
            }
        }
        private void ME1checkgamefiles()
        {
            if (File.Exists(ME1path + @"\BioGame\Config\DefaultEngine.ini") == true)
            {
                StreamReader configfile = new StreamReader(ME1path + @"\BioGame\Config\DefaultEngine.ini");
                bool error = false;
                try
                {
                    string lang = null;
                    for (int i = 1; i < 10; i++) { configfile.ReadLine(); }
                    lang = configfile.ReadLine().Substring(Math.Max(0, 9));
                    if (lang == "PLPC") { button_ME1gamefullPL.Enabled = false; pictureBox_ME1gamefullPL.Visible = true; button_ME1gamesubtitlesPL.Enabled = true; pictureBox_ME1gamesubtitlesPL.Visible = false; }
                    else if (lang == "PL") { button_ME1gamefullPL.Enabled = true; pictureBox_ME1gamefullPL.Visible = false; button_ME1gamesubtitlesPL.Enabled = false; pictureBox_ME1gamesubtitlesPL.Visible = true; }
                    else { button_ME1gamefullPL.Enabled = true; pictureBox_ME1gamefullPL.Visible = true; button_ME1gamesubtitlesPL.Enabled = true; pictureBox_ME1gamesubtitlesPL.Visible = true; }
                    ME1buttonclick();
                }
                catch { error = true; }
                finally { configfile.Close(); }
                if (error == true)
                {
                    MessageBox.Show("Nie można odczytać pliku \"DefaultEngine.ini\"", "Mass Effect Language Changer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nie można odnaleźć pliku \"DefaultEngine.ini\"", "Mass Effect Language Changer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ME1buttonclick()
        {
            if (ME1click == true)
            {
                ME1click = false; ME1open = true;
                button_ME1.Image = Properties.Resources.ME1select;
                if (ME2open == false)
                    this.ClientSize = new System.Drawing.Size(304, 379);
                groupBox_ME1gamefile.Visible = true;
                pictureBox_logo.Visible = false;
            }
            else if (ME1click == false)
            {
                ME1click = true; ME1open = false;
                button_ME1.Image = Properties.Resources.ME1;
                if (ME2open == false)
                    this.ClientSize = new System.Drawing.Size(304, 194);
                groupBox_ME1gamefile.Visible = false;
                pictureBox_logo.Visible = true;
            }
        }
        #endregion

        #endregion

        #region Mass Effect 2

        #region Game Files
        private void button_ME2gamefullPL_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter configfile = new StreamWriter(ME2path + @"\data\sku.ini");
                configfile.WriteLine("[SKU]"); configfile.WriteLine("SKU=");
                configfile.WriteLine("VOLanguage=POL");
                configfile.WriteLine("TextLanguage=POL");
                configfile.Close();
                string filepath = ME2path + @"\BioGame\CookedPC\";
                FileInfo infofile = new FileInfo(filepath + "BIOGame_POL.tlk");
                if (infofile.Length.ToString() != "1942212")
                {
                    File.Move(filepath + "BIOGame_POL.tlk", filepath + "BIOGame_POL.tlk_tmp");
                    File.Move(filepath + "BIOGame_INT.tlk", filepath + "BIOGame_POL.tlk");
                    File.Move(filepath + "BIOGame_POL.tlk_tmp", filepath + "BIOGame_INT.tlk");
                }
                string xmlfile = ME2path + @"\data\MassEffect2Launcher.xml";
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlfile);
                XmlNode plstringNode = xmlDocument.SelectSingleNode("BioWareApp/application/languagetable/strings[@language='pl']/string[@name='play']");
                plstringNode.InnerText = "Graj";
                XmlNode conditionsNode = xmlDocument.SelectSingleNode("BioWareApp/conditions/condition[@name='LanguageSet']/case[@casevalue='INT']");
                conditionsNode.Attributes["value"].Value = "en";
                xmlDocument.Save(xmlfile);
                button_ME2gamefullPL.Enabled = false; pictureBox_ME2gamefullPL.Visible = true; button_ME2gamesubtitlesPL.Enabled = true; pictureBox_ME2gamesubtitlesPL.Visible = false;
            }
            catch
            {
                MessageBox.Show("Nie można zmienić i zapisać plików gry", "Mass Effect 2 Language Changer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void button_ME2gamesubtitlesPL_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter configfile = new StreamWriter(ME2path + @"\data\sku.ini");
                configfile.WriteLine("[SKU]"); configfile.WriteLine("SKU=");
                configfile.WriteLine("VOLanguage=INT");
                configfile.WriteLine("TextLanguage=INT");
                configfile.Close();
                string filepath = ME2path + @"\BioGame\CookedPC\";
                File.Move(filepath + "BIOGame_POL.tlk", filepath + "BIOGame_POL.tlk_tmp");
                File.Move(filepath + "BIOGame_INT.tlk", filepath + "BIOGame_POL.tlk");
                File.Move(filepath + "BIOGame_POL.tlk_tmp", filepath + "BIOGame_INT.tlk");
                string xmlfile = ME2path + @"\data\MassEffect2Launcher.xml";
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlfile);
                XmlNode plstringNode = xmlDocument.SelectSingleNode("BioWareApp/application/languagetable/strings[@language='pl']/string[@name='play']");
                plstringNode.InnerText = "Graj (Dubbing ENG)";
                XmlNode conditionsNode = xmlDocument.SelectSingleNode("BioWareApp/conditions/condition[@name='LanguageSet']/case[@casevalue='INT']");
                conditionsNode.Attributes["value"].Value = "pl";
                xmlDocument.Save(xmlfile);
                button_ME2gamefullPL.Enabled = true; pictureBox_ME2gamefullPL.Visible = false; button_ME2gamesubtitlesPL.Enabled = false; pictureBox_ME2gamesubtitlesPL.Visible = true;
            }
            catch
            {
                MessageBox.Show("Nie można zmienić i zapisać plików gry", "Mass Effect 2 Language Changer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Registry
        private void button_ME2registryPL_Click(object sender, EventArgs e)
        {
            try
            {
                if (OSArchitecture() == 32)
                {
                    RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\BioWare\Mass Effect 2\", true);
                    Key.SetValue("Language", "pl");
                }
                if (OSArchitecture() == 64)
                {
                    RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\BioWare\Mass Effect 2\", true);
                    Key.SetValue("Language", "pl");
                }
                button_ME2registryPL.Enabled = false; pictureBox_registryPL.Visible = true; button_ME2registryEN.Enabled = true; pictureBox_registryEN.Visible = false;
            }
            catch
            {
                MessageBox.Show("Nie posiadasz uprawnień do zapisu wartości", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button_ME2registryEN_Click(object sender, EventArgs e)
        {
            try
            {
                if (OSArchitecture() == 32)
                {
                    RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\BioWare\Mass Effect 2\", true);
                    Key.SetValue("Language", "en");
                }
                if (OSArchitecture() == 64)
                {
                    RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\BioWare\Mass Effect 2\", true);
                    Key.SetValue("Language", "en");
                }
                button_ME2registryPL.Enabled = true; pictureBox_registryPL.Visible = false; button_ME2registryEN.Enabled = false; pictureBox_registryEN.Visible = true;
            }
            catch
            {
                MessageBox.Show("Nie posiadasz uprawnień do zapisu wartości", "Brak uprawnień", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region References
        private void ME2checkregistry()
        {
            if (OSArchitecture() == 32)
            {
                RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\BioWare\Mass Effect 2\");
                if (Key == null)
                {
                    MessageBox.Show("Nie wykryto instalacji Mass Effect 2", "Mass Effect 2 Language Changer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string languagevalue = Key.GetValue("Language").ToString();
                    if (languagevalue == "pl") { button_ME2registryPL.Enabled = false; pictureBox_registryPL.Visible = true; button_ME2registryEN.Enabled = true; pictureBox_registryEN.Visible = false; }
                    else if (languagevalue == "en") { button_ME2registryPL.Enabled = true; pictureBox_registryPL.Visible = false; button_ME2registryEN.Enabled = false; pictureBox_registryEN.Visible = true; }
                    else { button_ME2registryPL.Enabled = true; pictureBox_registryPL.Visible = true; button_ME2registryEN.Enabled = true; pictureBox_registryEN.Visible = true; }
                    ME2path = Key.GetValue("Path").ToString();
                    ME2checkgamefiles();
                }
            }
            else if (OSArchitecture() == 64)
            {
                RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\BioWare\Mass Effect 2\");
                if (Key == null)
                {
                    MessageBox.Show("Nie wykryto instalacji Mass Effect 2", "Mass Effect 2 Language Changer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string languagevalue = Key.GetValue("Language").ToString();
                    if (languagevalue == "pl") { button_ME2registryPL.Enabled = false; pictureBox_registryPL.Visible = true; button_ME2registryEN.Enabled = true; pictureBox_registryEN.Visible = false; }
                    else if (languagevalue == "en") { button_ME2registryPL.Enabled = true; pictureBox_registryPL.Visible = false; button_ME2registryEN.Enabled = false; pictureBox_registryEN.Visible = true; }
                    else { button_ME2registryPL.Enabled = true; pictureBox_registryPL.Visible = true; button_ME2registryEN.Enabled = true; pictureBox_registryEN.Visible = true; }
                    ME2path = Key.GetValue("Path").ToString();
                    ME2checkgamefiles();
                }
            }
        }
        private void ME2checkgamefiles()
        {
            if (File.Exists(ME2path + @"\data\sku.ini") == false)
            {
                using (File.Create(ME2path + @"\data\sku.ini")) { }
            }
            StreamReader configfile = new StreamReader(ME2path + @"\data\sku.ini");
            bool skuerror = false;
            string filepath = ME2path + @"\BioGame\CookedPC\";
            if (!File.Exists(filepath + "BIOGame_INT.tlk"))
            {
                label_warning.Visible = true;
                button_ME2gamefullPL.Visible = false; pictureBox_ME2gamefullPL.Visible = false; button_ME2gamesubtitlesPL.Visible = false; pictureBox_ME2gamesubtitlesPL.Visible = false;
            }
            else
            {
                label_warning.Visible = false; button_ME2gamefullPL.Visible = true; button_ME2gamesubtitlesPL.Visible = true; 
                try
                {
                    string vol; string text;
                    configfile.ReadLine(); configfile.ReadLine();
                    vol = configfile.ReadLine().Substring(Math.Max(0, 11));
                    text = configfile.ReadLine().Substring(Math.Max(0, 13));
                    FileInfo infofile = new FileInfo(filepath + "BIOGame_POL.tlk");
                    if (vol == "POL" && text == "POL" && infofile.Length > 1900000) { button_ME2gamefullPL.Enabled = false; pictureBox_ME2gamefullPL.Visible = true; button_ME2gamesubtitlesPL.Enabled = true; pictureBox_ME2gamesubtitlesPL.Visible = false; }
                    else if (vol == "INT" && text == "INT" && infofile.Length < 1900000) { button_ME2gamefullPL.Enabled = true; pictureBox_ME2gamefullPL.Visible = false; button_ME2gamesubtitlesPL.Enabled = false; pictureBox_ME2gamesubtitlesPL.Visible = true; }
                    else { button_ME2gamefullPL.Enabled = true; pictureBox_ME2gamefullPL.Visible = false; button_ME2gamesubtitlesPL.Enabled = true; pictureBox_ME2gamesubtitlesPL.Visible = false; }
                }
                catch { skuerror = true; }
                finally { configfile.Close(); }
            }
            if (skuerror == true)
            {
                MessageBox.Show("Nie można odczytać pliku \"sku.ini\"\nPrzywrócona zostanie domyślna zawartość pliku", "Mass Effect 2 Language Changer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StreamWriter repairconfigfile = new StreamWriter(ME2path + @"\data\sku.ini");
                repairconfigfile.WriteLine("[SKU]"); repairconfigfile.WriteLine("SKU=");
                repairconfigfile.WriteLine("VOLanguage=POL");
                repairconfigfile.WriteLine("TextLanguage=POL");
                repairconfigfile.Close();
                button_ME2gamefullPL.Enabled = false; pictureBox_ME2gamesubtitlesPL.Visible = false;
            }
            ME2buttonclick();
        }
        private void ME2buttonclick()
        {
            if (ME2click == true)
            {
                ME2click = false; ME2open = true;
                button_ME2.Image = Properties.Resources.ME2select;
                this.ClientSize = new System.Drawing.Size(595, 379);
                groupBox_ME2gamefile.Visible = true;
                groupBox_ME2registry.Visible = true;
                pictureBox_logo.Visible = true;
            }
            else if (ME2click == false)
            {
                ME2click = true; ME2open = false;
                button_ME2.Image = Properties.Resources.ME2;
                if (ME1open == true)
                    this.ClientSize = new System.Drawing.Size(304, 379);
                else
                    this.ClientSize = new System.Drawing.Size(304, 194);
                groupBox_ME2gamefile.Visible = false;
                groupBox_ME2registry.Visible = false;
                pictureBox_logo.Visible = false;
            }
        }
        #endregion 

        #endregion

    }
}