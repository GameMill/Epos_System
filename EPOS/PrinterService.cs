using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPOS
{
    class PrinterService
    {
        const string V = "\u0056";
        const string B = "\u0042";
        const string t = "\u0074";
        const string p = "\u0070";
        const string e = "\u0065";
        const string c = "\u0063";
        const string d = "\u0064";
        const string a = "\u0061";
        const string M = "\u004D";
        const string G = "\u0047";


        const string ESC = "\u001B";
        const string GS = "\u001D";
        const string NUL = "\0";
        
        const string InitializePrinter = ESC + "@";
        const string BoldOn = ESC + "E" + "\u0001";
        const string BoldOff = ESC + "E" + NUL;
        const string DoubleOn = GS + "!" + "\u0011";  // 2x sized text (double-high + double-wide)
        const string QuadOn = GS + "!" + "\u0022";  // 4x sized text (double-high + double-wide)
        const string eightOn = GS + "!" + "\u0033";  // 4x sized text (double-high + double-wide)
        const string sixteenOn = GS + "!" + "\u0044";  // 4x sized text (double-high + double-wide)
        const string DoubleOff = GS + "!" + NUL;
        const string DoubleStrikeOn = ESC + "G" + "\u0001";
        const string DoubleStrikeOff = ESC + "G" + NUL;
        const string UnderlineOff = ESC + "-" + "\0";
        const string UnderlineSmall = ESC + "-" + "\u0001";
        const string UnderlineBig = ESC + "-" + "\u0002";

        const string UKFont = ESC + "R" + "\u0003";

        const string AlignLeft = ESC + "a" + "\0";
        const string AlignCenter = ESC + "a" + "\u0001";
        const string AlignRight = ESC + "a" + "\u0002";

        const string ReverseModeOn = GS + B + "\u0001";
        const string ReverseModeOff = GS + B + "\0";

        bool NeedBiggerMargen = false;
        /*
         * Font 1
         * 1x 42
         * 2x 21
         * 3x 14
         * 4x 10
         * 5x 8
         * 6x 7
         * 7x 6
         * 8x 5
         */

        /*
         * Font 2
         * 1x 56
         * 2x 28
         * 3x 18
         * 4x 14
         * 5x 11
         * 6x 9
         * 7x 8
         * 8x 7
         */

        System.IO.Ports.SerialPort Port;

        public bool DisableSmooth { get; set; } = false;
        bool Available = false;

        public PrinterService(string ComPort,System.IO.Ports.Handshake Handshake = System.IO.Ports.Handshake.XOnXOff, int BandRate = 19200, System.IO.Ports.Parity Parity = System.IO.Ports.Parity.None, int Databit = 8, System.IO.Ports.StopBits Stopbit = System.IO.Ports.StopBits.One)
        {
            try
            {
                Port = new System.IO.Ports.SerialPort(ComPort, BandRate, Parity, Databit, Stopbit);
            }
            catch 
            {
                return;
            }
            Available = true;
            Port.Handshake = Handshake;
        }
        public void Open()
        {
            if (!Available)
                return;
            try
            {
                Port.Open();
                Port.Write(InitializePrinter);
                Port.Write(UKFont);
            }
            catch
            {
                
            }
        }

        public void ChangeFontSize(int Width,int Height)
        {
            if (!Available)
                return;
            if (!DisableSmooth)
                Port.Write(GS + "\u0062" + "\u0001");
            var c = Math.Pow(2, 4) * (Width - 1) + (Height - 1);
            Port.Write(GS + "!" + (char)c);
            if (Width > 3)
                NeedBiggerMargen = true;
        }

        public void Write(string Line, int Align = 0, bool DoubleStrike = false, int Size = 1, bool Bold = false, int Underline = 0, bool ReverseMode = false)
        {
            if (!Available)
                return;
            if (Port.IsOpen)
            {
                if (Align > 0)
                    if (Align == 1)
                        Port.Write(AlignCenter);
                    if (Align == 2)
                        Port.Write(AlignRight);
                   
                if (DoubleStrike)
                    Port.Write(DoubleStrikeOn);
                if (Size > 1)
                {
                    ChangeFontSize(Size, Size);
                    /*Port.Write(GS + "b" + "\u0001");
                    if (Size == 1)
                        Port.Write(DoubleOn);
                    else if (Size == 2)
                        Port.Write(QuadOn);
                    else if (Size == 3)
                        Port.Write(eightOn);
                    else
                        Port.Write(sixteenOn);*/
                }
                else
                    NeedBiggerMargen = false;
               
                if (Bold)
                    Port.Write(BoldOn);
                if (Underline > 0)
                    if (Underline == 1)
                        Port.Write(UnderlineSmall);
                    else
                        Port.Write(UnderlineBig);
                if (ReverseMode)
                    Port.Write(ReverseModeOn);


                Port.Write(Line.Replace("£","\u0023"));

                if (ReverseMode)
                    Port.Write(ReverseModeOff);
                if (Underline > 0)
                    Port.Write(UnderlineOff);
                if (Bold)
                    Port.Write(BoldOff);
                if (Size > 1)
                {
                    if(!DisableSmooth)
                        Port.Write(GS + "b" + "\0");
                    Port.Write(DoubleOff);
                }
                if (DoubleStrike)
                    Port.Write(DoubleStrikeOff);
                if (Align > 0)
                    Port.Write(AlignLeft);
            }
            else
            {
                System.Windows.MessageBox.Show("Error Serial Port Not Open");
            }
        }

        public void WriteLine(string Line="", int Align = 0, bool DoubleStrike = false, int Size = 0, bool Bold = false, int Underline = 0, bool ReverseMode = false)
        {
            if (!Available || !Port.IsOpen)
                return;
            Write(Line + Environment.NewLine, Align, DoubleStrike, Size, Bold, Underline, ReverseMode);                
        }

        public void BlankLine(int NumberOfLines)
        {
            if (!Available ||!Port.IsOpen)
                return;
            for (int i = 0; i < NumberOfLines; i++)
            {
                Port.WriteLine("");
            }
        }

        public void ChangeFont(int type)
        {
            if (!Available)
                return;
            int b = 48 + type;
            if (type == 0)
                Port.Write(ESC + "\u004D" + "0");
            else if (type == 1)
                Port.Write(ESC + "\u004D" + "1");
            else if (type == 1)
                Port.Write(ESC + "\u004D" + "2");
        }

        public void Barcode(string Code, int Align = 0, string Height="\u0120")
        {
            if (!Available || !Port.IsOpen)
                return;
            if (Align > 0)
                if (Align == 1)
                    Port.Write(AlignCenter);
                else
                    Port.Write(AlignRight);

            Port.Write(GS + "\u0068" + Height);
            Port.WriteLine(GS + "k" + "\u0004" + Code + "\u0000\u0000");

            if (Align > 0)
                Port.Write(AlignLeft);
        }

        public void Barcode128(string Code, int Align = 0, string Height = "\u0120")
        {
            if (!Available || !Port.IsOpen)
                return;

            if (Align > 0)
                if (Align == 1)
                    Port.Write(AlignCenter);
                else
                    Port.Write(AlignRight);

            Port.Write(GS + "\u0068" + Height);
            Port.WriteLine(GS + "k" + "\u0073" + "\u0010" + Code + "\u0000\u0000");

            if (Align > 0)
                Port.Write(AlignLeft);
        }

        public void Close()
        {
            if (!Available || !Port.IsOpen)
                return;
            if(NeedBiggerMargen)
                BlankLine(6);
            else
                BlankLine(4);
            NeedBiggerMargen = false;
            Port.Write(GS+"V"+"0, 48");
            Port.Close();
        }

        public void BarcodeTest()
        {
            var a = "{A012323392982";
            var b = "{B012323392982";
            var c = "{C"+((char)01)+((char)23)+((char)23)+((char)39)+((char)29)+((char)82);


            PrintTest(a);
            PrintTest(b);
            PrintTest(c);
        }
        private void PrintTest(string C)
        {
            if (!Available)
                return;
            //Port.Write(GS + "k" + ((char)(73 - 65)));
            Port.Write(GS + "k" + ((char)73) + ((char)c.Length) + c);   
        }
    }

}
