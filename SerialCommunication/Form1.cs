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
                    }
                    else
                    {
                        serialPort1Arduino.Close();
                        labelStatus.Text = "Error: verkeerd antwoord";
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

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerOefening5.Enabled = tabControl.SelectedIndex == 5;
        }
        private void timerOefening5_Tick(object sender, EventArgs e)
        {
            double rico = 0.039100684;
            int offset = 5;
            double rico1 = 0.488758553;
            try
            {
                if (serialPort1Arduino.IsOpen)
                {

                    serialPort1Arduino.ReadExisting();
                    string commando = "get a0";
                    serialPort1Arduino.WriteLine(commando);
                    string antwoord = serialPort1Arduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);

                    double value = double.Parse(antwoord);


                    double temperatuur = (rico * value) + offset;


                    labelGewensteTemp.Text = temperatuur.ToString("0.0°C");

                    serialPort1Arduino.ReadExisting();
                    string commando1 = "get a1";
                    serialPort1Arduino.WriteLine(commando1);
                    string antwoord1 = serialPort1Arduino.ReadLine();
                    antwoord1 = antwoord1.TrimEnd();
                    antwoord1 = antwoord1.Substring(4);

                    double value1 = double.Parse(antwoord1);


                    double temperatuur1 = rico1 * value1;

                    labelHuidigeTemp.Text = temperatuur1.ToString("0.0°C");
                    string commando2;
                    if (temperatuur1 < temperatuur) commando2 = "set d2 high";
                    else commando2 = "set d2 low";
                    serialPort1Arduino.WriteLine(commando2);




                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPort1Arduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

    }
}
