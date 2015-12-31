//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

public class Utility
{
	public static float clampAngle(float angle, float leftClamp, float rightClamp) {
		if (angle < leftClamp) return leftClamp;
		if (angle > rightClamp) return rightClamp;
		return angle;
	}

    public static Color BinaryStringToColor(string b)
    {
        Color cl = Color.black;
        char ch;
        int len = b.Length - 1;
        float ic = 0.0f;
        int ii = 0;
        int ib = 1;

        if (len != 31)
        {
            return cl;
        }
        for (int i = len; 0 <= i; i--)
        {
            ch = b[i];
            if (ch == '1')
            {
                ic += Mathf.Pow(2.0f, ii);
            }
            ii++;
            if ((ib == 1) && (ii == 8))
            {
                // alpha
                cl.a = Mathf.Floor(ic) / 255;
                ii = 0;
                ic = 0.0f;
                ib++;
            }
            else
            if ((ib == 2) && (ii == 8))
            {
                // blue
                cl.b = Mathf.Floor(ic) / 255;
                ii = 0;
                ic = 0.0f;
                ib++;
            }
            else
            if ((ib == 3) && (ii == 8))
            {
                // green
                cl.g = Mathf.Floor(ic) / 255;
                ii = 0;
                ic = 0.0f;
                ib++;
            }
            else
            if ((ib == 4) && (ii == 8))
            {
                // red
                cl.r = Mathf.Floor(ic) / 255;
                ii = 0;
                ic = 0.0f;
                ib++;
            }
        }
        return cl;
    }

    public static string FloatToBinaryString(float f)
    {
        string s;
        char c;
        char[] ca;
        int i;
        string si;
        string sf;
        string ss;

        if (f == 0.0f)
        {
            return "00000000000000000000000000000000";
        }
        if (f == -0.0f)
        {
            return "10000000000000000000000000000000";
        }
        if (float.IsPositiveInfinity(f))
        {
            return "01111111100000000000000000000000";
        }
        if (float.IsNegativeInfinity(f))
        {
            return "11111111100000000000000000000000";
        }
        if (float.IsNaN(f))
        {
            return "11111111110000000000000000000000";
        }

        bool is_significant;

        int exponent = 0;

        if (Mathf.Sign(f) >= 0)
        {
            ss = "0";
        }
        else
        {
            ss = "1";
        }

        si = "";
        sf = "0.";
        s = f.ToString("0.#############################");
        int l = s.Length;
        is_significant = true;
        exponent = 0;
        ca = s.ToCharArray();
        int izero = -1;

        for (i = 0; i < l; i++)
        {
            c = ca[i];

            if (c == '.')
            {
                izero = i;
                si += ".0";
            }
            if ((c == '0') || (c == '1') || (c == '2') || (c == '3') || (c == '4') || (c == '5') || (c == '6') || (c == '7') || (c == '8') || (c == '9'))
            {
                is_significant = (izero < 0);
                if (is_significant)
                {
                    si += c;
                }
                else
                {
                    sf += c;
                }
            }
        }
        int biased_exponent;
        if (si == "")
        {
            si = "0.0";
        }
        string si2bin = IntToBinaryString(float.Parse(si), false, 0);
        string sf2bin = IntToBinaryString(float.Parse(sf), true, 0);

        if (si2bin == "")
        {
            si2bin = "0.0";
        }
        if (si2bin == "0.0")
        {
            exponent = -(sf2bin.IndexOf("1") + 1);
        }
        else
        {
            exponent = si2bin.Length - 1;
        }

        int last_index = 0;
        string sbe2bin;
        biased_exponent = 127 + exponent;
        sbe2bin = IntToBinaryString(biased_exponent, false, 0);
        sbe2bin = "00000000" + sbe2bin;
        sbe2bin = sbe2bin.Substring(sbe2bin.Length - 8, 8);
        string stotal = ss + sbe2bin;
        if (si2bin == "0.0")
        {
            last_index = sf2bin.LastIndexOf('1');
            stotal += sf2bin.Substring(-exponent);
            stotal += "0000000000000000000000000000000000000000000000";
            if (last_index > 0)
            {
                stotal = stotal.Substring(0, 31) + '1';
            }
            else
            {
                stotal = stotal.Substring(0, 32);
            }
        }
        else
        {
            stotal += si2bin.Substring(1) + sf2bin;
            if (stotal.Length > 32)
            {
                stotal = stotal.Substring(0, 31) + '1';
            }
            else
            {
                stotal += "0000000000000000000000000000000000000000000000";
                stotal = stotal.Substring(0, 32);
            }
        }
        //Debug.Log("INPUT=" + f.ToString() + " SignBin=" + ss + " sbe2bin=" + sbe2bin + " Exponent=" + exponent + " Biased exponent=" + biased_exponent + " si=" + si + " si2bin=" + si2bin + " sf=" + sf + " sf2bin=" + sf2bin + " stotal=" + stotal);		
        return stotal;
    }

    public static float BinaryStringToFloat(string b)
    {
        if (b == "00000000000000000000000000000000")
        {
            return 0.0f;
        }
        if (b == "10000000000000000000000000000000")
        {
            return -0.0f;
        }
        if (b == "01111111100000000000000000000000")
        {
            return float.PositiveInfinity;
        }
        if (b == "11111111100000000000000000000000")
        {
            return float.NegativeInfinity;
        }
        if ((b.Substring(1, 8) == "11111111") && (b.Substring(9, 23).Contains("1")))
        {
            return float.NaN;
        }

        double fl = 0.0d;
        float sign = 1.0f;
        float exponent = 0.0f;
        double mantisa = 0.0d;

        char ch;
        int len = b.Length - 1;
        float ic = 0.0f;
        double im = 0.0d;
        int ii = 0;
        int ib = 1;

        if (len != 31)
        {
            return (float)fl;
        }
        for (int i = len; 0 <= i; i--)
        {
            ch = b[i];
            if (ch == '1')
            {
                ic += Mathf.Pow(2.0f, ii);
                im += (1.0d / (double)Mathf.Pow(2.0f, (float)i - 8.0f));
            }
            ii++;
            if ((ib == 1) && (ii == 23))
            {
                // mantisa
                mantisa = 1.0d + im;
                ii = 0;
                ic = 0.0f;
                ib++;
            }
            else
            if ((ib == 2) && (ii == 8))
            {
                //exponent
                exponent = ic;
                ii = 0;
                ic = 0.0f;
                ib++;
            }
            else
            if ((ib == 3) && (ii == 1))
            {
                // sign
                if (ch == '1')
                {
                    sign = -1.0f;
                }
                ii = 0;
                ic = 0.0f;
                ib++;
            }
        }
        exponent = exponent - 127.0f;
        fl = (double)sign * (float)mantisa * (double)Mathf.Pow(2.0f, exponent);
        //Debug.Log ("BinaryStringToFloat=" + fl.ToString() + " SIGN=" + sign.ToString() + " E=" + exponent.ToString() + " M=" + mantisa.ToString());
        return (float)fl;
    }

    public static string IntToBinaryString(float number, bool fraction, int padding)
    {
        string s;
        float number_float;
        float quotient_float;
        int number_int;
        int quotient;
        int reminder;
        s = "";

        if (fraction)
        {
            int j = 1;
            number_float = number;
            while (j < 30)
            {
                quotient_float = number_float * 2.0f;
                if (quotient_float >= 1.0f)
                {
                    s += '1';
                    quotient_float -= 1.0f;
                }
                else
                {
                    s += '0';
                }
                number_float = quotient_float;
                j++;
                if (quotient_float == 0.0f)
                {
                    j = 100;
                    s += "0";
                }
            }
        }
        else
        {
            number_int = (int)number;
            while (0 < number_int)
            {
                quotient = (int)Mathf.Floor(number_int / 2);
                reminder = (number_int - (quotient * 2));
                number_int = quotient;
                if (reminder == 0)
                {
                    s = '0' + s;
                }
                else
                {
                    s = '1' + s;
                }
                if (number_int == 0)
                {
                    number_int = -100;
                }
            }
            if (padding > 0)
            {
                s = "0000000000000000000000000000000000000000000000" + s;
                s = s.Substring(s.Length - padding);
            }
        }
        return s;
    }

    public static string ColorToBinaryString(Color c)
    {
        string s;
        s = IntToBinaryString(Mathf.Floor(c.r * 255.0f), false, 8);
        s += IntToBinaryString(Mathf.Floor(c.g * 255.0f), false, 8);
        s += IntToBinaryString(Mathf.Floor(c.b * 255.0f), false, 8);
        s += IntToBinaryString(Mathf.Floor(c.a * 255.0f), false, 8);
        return s;
    }

    public static Color FloatToColor(float f)
    {
        return BinaryStringToColor(FloatToBinaryString(f));
    }

    public static float ColorToFloat(Color c)
    {
        return BinaryStringToFloat(ColorToBinaryString(c));
    }
}

