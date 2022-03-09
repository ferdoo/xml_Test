using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using System.Globalization;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Collections.Generic;

namespace xml_Test
{
    public partial class Form1 : Form
    {

        private XmlNamespaceManager _ns;

        private XmlDocument _document;
        public XmlDocument Document
        {
            get { return _document; }
            set { _document = value; }
        }

        private XmlNode _rootNode;
        public XmlNode RootNode
        {
            get { return _rootNode; }
            set { _rootNode = value; }
        }

        private XmlNode _node;

        public int keyvalue; // TEST

        public XmlNode Node
        {
            get { return _node; }
            set { _node = value; }
        }


        //private IDictionary<string, string> instancneDictionary = new Dictionary<string, string>();


        private List<string> names = new List<string>();



        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            names.Clear();

            richTextBox1.Clear();

            var openFileDialog = new OpenFileDialog();

            //openFileDialog.InitialDirectory = @"D:\Excel code generator for TIA Portal Openness\xml Export ProjectCheck debug";
            //openFileDialog.InitialDirectory = @"D:\Excel code generator for TIA Portal Openness\xml Export Openes\TEMP\26-E10-90C-367604-001-1330-SPS.ap15_1\20_Station_1320";
            openFileDialog.Filter = "TIA XML (*.xml)|*.xml";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {


                foreach (var file in openFileDialog.FileNames)
                {
                    
                    //richTextBox1.AppendText(file + "\n");
                    //richTextBox1.AppendText(Environment.NewLine);
                    ProcessFile(file);
                }


                //var colisions = names.GroupBy(x => new {x}).Where(w => w.Count() > 1).Select(s=>s);

                var colisions = names.GroupBy(x => x)
                    .Where(group => group.Count() > 1)
                    .Select(group => group.Key);

                foreach (var colision in colisions)
                {
                    richTextBox1.AppendText(colision);
                    richTextBox1.AppendText(Environment.NewLine);
                }

                richTextBox1.AppendText("Hotovo");
            }

        }



        void ProcessFile(string Filename)
        {
            Document = new XmlDocument();

            _ns = new XmlNamespaceManager(Document.NameTable);
            _ns.AddNamespace("SI", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
            _ns.AddNamespace("siemensNetworks", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v3");

            //Load Xml File with fileName into memory
            Document.Load(Filename);
            //get root node of xml file
            RootNode = Document.DocumentElement;


            var listOfNetworks = RootNode.SelectNodes("//SW.Blocks.CompileUnit");


            if (listOfNetworks != null)
            {
                foreach (XmlNode network in listOfNetworks)
                {
                    var listOfCallRef = network.SelectNodes(".//siemensNetworks:Call", _ns);

                    foreach (XmlNode nodeCallref in listOfCallRef)
                    {

                        var nodeCallInfo = nodeCallref.SelectSingleNode(".//siemensNetworks:CallInfo", _ns);
                        var nodeComponent = nodeCallref.SelectSingleNode(".//siemensNetworks:Component", _ns);


                        var Name = "";
                        var BlockType = "";
                        var Component = "";


                        Name = nodeCallInfo.Attributes["Name"].Value;
                        BlockType = nodeCallInfo.Attributes["BlockType"].Value;

                        if (nodeComponent != null)
                            Component = nodeComponent.Attributes["Name"].Value;


                        //richTextBox1.AppendText($"BlockType: {BlockType}   Name: {Name}   Component: {Component} \n");
                        //richTextBox1.AppendText(Environment.NewLine);


                        if (Component != "")
                        {
                            //instancneDictionary.Add(Component, Component);

                            names.Add(Component);

                        }
                        

                    }
                }
            }
        }

    }
}
