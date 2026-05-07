using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1Arduino.IsOpen)
                {
                    //wilt verbreken
                    serialPort1Arduino.Close();
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                    labelStatus.Text = "Status: Disconected";
                }
                else
                {
                    //wilt verbinden
                    serialPort1Arduino.PortName = (string)comboBoxPoort.SelectedItem;
                    serialPort1Arduino.BaudRate = Int32.Parse((string)comboBoxBaudrate.SelectedItem);
                    serialPort1Arduino.DataBits = (int)numericUpDownDatabits.Value;

                    if (radioButtonParityEven.Checked) serialPort1Arduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked) serialPort1Arduino.Parity = Parity.Odd;
                    else if (radioButtonParityNone.Checked) serialPort1Arduino.Parity = Parity.None;
                    else if (radioButtonParityMark.Checked) serialPort1Arduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked) serialPort1Arduino.Parity = Parity.Space;

                    if (radioButtonStopbitsNone.Checked) serialPort1Arduino.StopBits = StopBits.None;
                    else if (radioButtonStopbitsOne.Checked) serialPort1Arduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsOnePointFive.Checked) serialPort1Arduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsTwo.Checked) serialPort1Arduino.StopBits = StopBits.Two;

                    if (radioButtonHandshakeNone.Checked) serialPort1Arduino.Handshake = Handshake.None;
                    else if (radioButtonHandshakeRTS.Checked) serialPort1Arduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeRTSXonXoff.Checked) serialPort1Arduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeXonXoff.Checked) serialPort1Arduino.Handshake = Handshake.XOnXOff;

                    serialPort1Arduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPort1Arduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPort1Arduino.Open();
                    string commando = "ping";
                    serialPort1Arduino.WriteLine(commando);
                    string antwoord = serialPort1Arduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    if (antwoord == "pong")
                    {
                        radioButtonVerbonden.Checked = true;
                        buttonConnect.Text = "Disconnect";
                        labelStatus.Text = "Status: Connected";
                    }
                    else
                    {
                        serialPort1Arduino.Close();
                        labelStatus.Text = "Error: verkeerd antwoord";
                        radioButtonVerbonden.Checked = false;
                    }
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error:" + exception.Message;
                serialPort1Arduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1Arduino.IsOpen)
                {
                    string commando; //set d2 high/low
                    if (checkBoxDigital2.Checked) commando = "set d2 high";
                    else commando = "set d2 low";
                    serialPort1Arduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPort1Arduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "connect";
            }

        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1Arduino.IsOpen)
                {
                    string commando; //set d3 high/low
                    if (checkBoxDigital3.Checked) commando = "set d3 high";
                    else commando = "set d3 low";
                    serialPort1Arduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPort1Arduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "connect";
            }
        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1Arduino.IsOpen)
                {
                    string commando; //set d4 high/low
                    if (checkBoxDigital4.Checked) commando = "set d4 high";
                    else commando = "set d4 low";
                    serialPort1Arduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPort1Arduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "connect";
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check of de geselecteerde tab degene is van Oefening 5
            if (tabControl.SelectedTab == tabPageOefening5)
            {
                // Hier komt de code die moet starten voor de oefening
                timerOefening5.Start();
            }
        }

        private void timerOefening5_Tick(object sender, EventArgs e)
        {
            timerOefening5.Start();
            try
            {
                if (serialPort1Arduino.IsOpen)
                {

                    // === GEWENSTE TEMPERATUUR (analoge pin 0) ===
                    double richtingscoefficientGewenst = 40.0 / 1023.0;
                    double offsetGewenst = 5.0;

                    // Lees analoge pin 0 uit via seriële communicatie
                    serialPort1Arduino.ReadExisting();
                    serialPort1Arduino.WriteLine("get a0");
                    string antwoordA0 = serialPort1Arduino.ReadLine();

                    int waardeA0 = int.Parse(antwoordA0.Substring(4));
                    double gewensteTemp = (richtingscoefficientGewenst * waardeA0) + offsetGewenst;

                    labelGewensteTemp.Text = gewensteTemp.ToString("F1") + " °C";


                    // === HUIDIGE TEMPERATUUR (analoge pin 1) ===
                    double richtingscoefficientHuidig = 500.0 / 1023.0;

                    // Lees analoge pin 1 uit via seriële communicatie
                    serialPort1Arduino.ReadExisting();
                    serialPort1Arduino.WriteLine("get a1");
                    string antwoordA1 = serialPort1Arduino.ReadLine();


                    int waardeA1 = int.Parse(antwoordA1.Substring(4));
                    double huidigeTemp = (richtingscoefficientHuidig * waardeA1);

                    labelHuidigeTemp.Text = huidigeTemp.ToString("F1") + " °C";

                    if (huidigeTemp < gewensteTemp)
                    {
                        // LED AAN
                        serialPort1Arduino.WriteLine("set d2 high");

                    }
                    else
                    {
                        // LED UIT
                        serialPort1Arduino.WriteLine("set d2 low");
                    }

                }
            }
            catch (Exception exception)
            {
                timerOefening5.Stop();
                labelStatus.Text = "Error: " + exception.Message;
                serialPort1Arduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "connect";
            }



        }
    }
}
