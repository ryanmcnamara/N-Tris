using N_Tris.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace N_Tris
{
    class PolyominoColorChooser
    {
        public static void assignColors( List<Polyomino> polys )
        {
            if ( polys.Count == 0)
            {
                return;
            }

            if (polys[0].Minos.Count == 4)
            {
                foreach (Polyomino p in polys)
                {
                    p.Color = assignColor(p);
                }
            }
            else
            {
                for (int i = 0; i < polys.Count; i++)
                {
                    double hue = ((double)i) / polys.Count;
                    hue *= 360;

                    int r;
                    int g;
                    int b;

                    HsvToRgb(hue, 1, .7, out r, out g, out b);
                    Brush c =  new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
                    polys[i].Color = c;
                }

            }

        }

        public static Brush assignColor( Polyomino p_ )
        {
            Polyomino p = p_.Clone();
            
            HashBitArray h = p.toBitArray();

            if (p.Minos.Count == 4)
            {
                // i
                bool[] x = { true, true, true, true, false, false, false, false, false, false, false, false, false, false, false, false };
                HashBitArray b = new HashBitArray(x);
                if (h.Equals(b)) return Brushes.Cyan;

                // o
                bool[] x1 = { true, true, false, false, true, true, false, false, false, false, false, false, false, false, false, false };
                HashBitArray b1 = new HashBitArray(x1);
                if (h.Equals(b1)) return Brushes.Yellow;

                // t
                bool[] x2 = { true, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false };
                HashBitArray b2 = new HashBitArray(x2);
                if (h.Equals(b2)) return Brushes.Purple;

                // s
                bool[] x3 = { true, true, false, false, false, true, true, false, false, false, false, false, false, false, false, false };
                HashBitArray b3 = new HashBitArray(x3);
                if (h.Equals(b3)) return Brushes.Green;

                // z
                bool[] x4 = { false, true, true, false, true, true, false, false, false, false, false, false, false, false, false, false };
                HashBitArray b4 = new HashBitArray(x4);
                if (h.Equals(b4)) return Brushes.Red;

                // j
                bool[] x5 = { true, true, true, false, true, false, false, false, false, false, false, false, false, false, false, false };
                HashBitArray b5 = new HashBitArray(x5);
                if (h.Equals(b5)) return Brushes.Blue;

                // l
                bool[] x6 = { true, true, true, false, false, false, true, false, false, false, false, false, false, false, false, false };
                HashBitArray b6 = new HashBitArray(x6);
                if (h.Equals(b6)) return Brushes.Orange;
            }

            int hash = h.GetHashCode();

            if (hash < 0)
            {
                hash = -(hash + 1);
            }
            double hue = (hash + 0.0) / int.MaxValue;
            hue *= 360;

            int red;
            int green;
            int blue;

            HsvToRgb(hue, .5, .5, out red, out green, out blue);

            Color c = new Color();
            c.R = (byte)red;
            c.G = (byte)green;
            c.B = (byte)blue;

            return Brushes.Black;
        }

        static void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
        {
            // ######################################################################
            // T. Nathan Mundhenk
            // mundhenk@usc.edu
            // C/C++ Macro HSV to RGB

            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = ((int)(R * 255.0)) % 255;
            g = ((int)(G * 255.0)) % 255;
            b = ((int)(B * 255.0)) % 255;
        }

    }
}
