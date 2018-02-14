using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smart_Toll_System
{
    public partial class MailFrom : Form
    {

        FilterInfoCollection webcam;
        VideoCaptureDevice cam;
        Image img=null;
        List<string> lst;
        string[] ss = { "yes$0.5$1$Regnum$132424244$A$4$", "yes$0.7$1$Regnum$1424244$A$5$" };

        public MailFrom()
        {
            InitializeComponent();
            cmbCam.Visible = false;
            cmbPort.Visible = false;
            btnSet.Visible = false;
            string[] ports = SerialPort.GetPortNames();
            cmbPort.Items.AddRange(ports);
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
            
            
        }
        DataGridViewRow row1;
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            try
            {


                string res = serialPort1.ReadLine();
                string[] temp = res.Split('$');


                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = DateTime.Now;
                row.Cells[1].Value = img;
                row1 = row;
                int s = 0;
                if (temp.Length <= 7) s = temp.Length;
                else s = 8;
                row.Cells[2].Value = "Lane 1";
                for (int y = 1; y < s; y++)
                    row.Cells[y + 2].Value = temp[y];
                //label1.Text = res;

                Invoke(new MethodInvoker(InnerLoadData));


                Thread.Sleep(100);

                sendMail("Date : " + row.Cells[0].Value + "\n" + "Track Number : " + row.Cells[7].Value + "\n" + "Weight : " + row.Cells[3].Value + "\n\n" + "Reported Bye Regnum Group");

                Print();
            }catch(Exception ee)
            {
                MessageBox.Show("Problem In Port :"+ee.Message.ToString());
            }

            
        }
        void InnerLoadData()
        {
            dataGridView1.Rows.Add(row1);
        }




        public void Print()
        {
            var doc = new PrintDocument();
            doc.PrintPage += new PrintPageEventHandler(ProvideContent);
            doc.Print();
        }
        //string[] ss = { "yes$0.5$1$Regnum$132424244$A$4$", "yes$0.7$1$Regnum$1424244$A$5$" };

        public void ProvideContent(object sender, PrintPageEventArgs e)
        {

            Graphics graphics = e.Graphics;
            Font font = new Font("Courier New", 10);

            float fontHeight = font.GetHeight();

            int startX = 0;
            int startY = 0;
            int Offset = 20;

            e.PageSettings.PaperSize.Width = 50;

            graphics.DrawString("Regnum Resource Ltd.\nAxle Load Control\n   (RFID Demo)", new Font("Courier New", 16),
                                new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 100;
            graphics.DrawString("Lane : 1", new Font("Courier New", 14),
                                new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Track Width : 0.10 Inch", new Font("Courier New", 14),
                                new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Track Weight : 0.5 KG", new Font("Courier New", 14),
                                new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Track Height : 0.2 Inch", new Font("Courier New", 14),
                                new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("Number Plate  : 132424244", new Font("Courier New", 14),
                                new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Vehicle Class  : A", new Font("Courier New", 14),
                                new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Wheel  : 4", new Font("Courier New", 14),
                                new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Fine  :" + "2000 TK",
                     new Font("Courier New", 14),
                     new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 20;


            graphics.DrawString(" Date :" + "27/09/2016",
                     new Font("Courier New", 14),
                     new SolidBrush(Color.Black), startX, startY + Offset);

            Offset = Offset + 20;
            String underLine = "------------------------------------------";

            graphics.DrawString(underLine, new Font("Courier New", 14),
                     new SolidBrush(Color.Black), startX, startY + Offset);

            Offset = Offset + 20;


            Offset = Offset + 20;
            underLine = "------------------------------------------";



            Offset = Offset + 40;

            graphics.DrawString("Signature ............",
                     new Font("Courier New", 14),
                     new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 60;





        }

       

        void sendMail(String s)
        {
            try
            {
                var fromAddress = new MailAddress("uiualllab@gmail.com", "Amit");
                var toAddress = new MailAddress("nazibahmad10@gmail.com", "Mr Nazib");
                const string fromPassword = "amit132134";
                const string subject = "Smart Tolling Report";
                string body = s;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
                
            }
            catch
            {
                MessageBox.Show("not send Sent");
            }


        }
        private void MailFrom_Load(object sender, EventArgs e)
        {
            

            
            dataGridView1.Rows[0].Height = 150;
            
            //dataGridView1.AutoSize = true;
            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            
           
            foreach (FilterInfo VideocaptureDevice in webcam)
            {
                cmbCam.Items.Add(VideocaptureDevice.Name);
                
            }

            
           

           
           
            
        }

        private void ca_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bit = (Bitmap)eventArgs.Frame.Clone();
            img = bit;
            pictureBox1.Image = img;
            
        }

/*
        public static void PrintReceiptForTransaction()
        {

            PrintDocument recordDoc = new PrintDocument();

            recordDoc.DocumentName = "Customer Receipt";
            recordDoc.PrintPage += new PrintPageEventHandler(ReceiptPrinter.PrintReceiptPage); // function below
            recordDoc.PrintController = new StandardPrintController(); // hides status dialog popup
            // Comment if debugging 
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = "EPSON TM-T20II Receipt";
            recordDoc.PrinterSettings = ps;
            recordDoc.Print();
            // --------------------------------------

            // Uncomment if debugging - shows dialog instead
            //PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();
            //printPrvDlg.Document = recordDoc;
            //printPrvDlg.Width = 1200;
            //printPrvDlg.Height = 800;
            //printPrvDlg.ShowDialog();
            // --------------------------------------

            recordDoc.Dispose();

        }

        private static void PrintReceiptPage(object sender, PrintPageEventArgs e)
        {
            float x = 10;
            float y = 5;
            float width = 270.0F; // max width I found through trial and error
            float height = 0F;

            Font drawFontArial12Bold = new Font("Arial", 12, FontStyle.Bold);
            Font drawFontArial10Regular = new Font("Arial", 10, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Set format of string.
            StringFormat drawFormatCenter = new StringFormat();
            drawFormatCenter.Alignment = StringAlignment.Center;
            StringFormat drawFormatLeft = new StringFormat();
            drawFormatLeft.Alignment = StringAlignment.Near;
            StringFormat drawFormatRight = new StringFormat();
            drawFormatRight.Alignment = StringAlignment.Far;

            // Draw string to screen.
            string text = "Regnum";
            e.Graphics.DrawString(text, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;

            text = "500 BDT";
            e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;

            // ... and so on

        }
        public static void PrintReceiptForTransaction()
        {

            PrintDocument recordDoc = new PrintDocument();

            recordDoc.DocumentName = "Customer Receipt";
            recordDoc.PrintPage += new PrintPageEventHandler(ReceiptPrinter.PrintReceiptPage); // function below
            recordDoc.PrintController = new StandardPrintController(); // hides status dialog popup
            // Comment if debugging 
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = "EPSON TM-T20II Receipt";
            recordDoc.PrinterSettings = ps;
            recordDoc.Print();
            // --------------------------------------

            // Uncomment if debugging - shows dialog instead
            //PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();
            //printPrvDlg.Document = recordDoc;
            //printPrvDlg.Width = 1200;
            //printPrvDlg.Height = 800;
            //printPrvDlg.ShowDialog();
            // --------------------------------------

            recordDoc.Dispose();

        }


            
      */

        private void MailFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {             
                    cam.Stop();
               
                    serialPort1.Close();
                    serialPort1.Dispose();

            }
            catch { }
           
        }

        private void MailFrom_FormClosed(object sender, FormClosedEventArgs e)
        {

           
        }

        private void setPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(mstShow.Text=="Show")
            {

                cmbPort.Visible = true;
                cmbCam.Visible = true;
                btnSet.Visible = true;
                mstShow.Text = "Hide";
            }
            else if (mstShow.Text == "Hide")
            {
                cmbPort.Visible = false;
                cmbCam.Visible = false;
                btnSet.Visible = false;
                mstShow.Text="Show";
            }

        }

        private void btnSet_Click(object sender, EventArgs e)
        {


            try
            {

                cam = new VideoCaptureDevice(webcam[cmbCam.SelectedIndex].MonikerString);
                cam.NewFrame += new AForge.Video.NewFrameEventHandler(ca_NewFrame);
                cam.Start();

            }
            catch
            {
                MessageBox.Show("Camera  Problem");
            }
           
            try
            {
                
                serialPort1.PortName = cmbPort.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.Open();
                

                


            }
            catch (Exception ex)
            {

               
            }
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
        }

        private void oFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mstCam.Text == "OFF")
            {
                pictureBox1.Visible = false;
                mstCam.Text="ON";

            }
            else if (mstCam.Text=="ON")
            {
                pictureBox1.Visible = true;
                mstCam.Text = "OFF";
            }

                
        }

        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutUS().Show();
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("a");
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("b");
        }
    }
}
