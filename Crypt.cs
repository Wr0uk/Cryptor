using System.Text;

namespace Cryptor;

public static class Crypt
{
    private static byte[]? _msg, _key;
    private static int[]? _codedMsg;
    private static long[]? _kollatz;
    
    public static string Translate(string message,string key)
    {
        ToByte(Attraction(message,key), key);
        CryptrMsg();
        return StringRepresantation();
    }

    private static void ToByte( string msg, string key)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding enc = Encoding.GetEncoding(1251);
        _msg = enc.GetBytes(msg);
        _key = enc.GetBytes(key);
    }

    private static void CryptrMsg()
    {
        _codedMsg = new int[_msg!.Length];
        bool backward = false;
        int index = 0;
        for (int i = 0; i < _msg.Length; i++)
        {
            _codedMsg[i] =_msg[i] + _key![index];
            
            if (index == _key.Length - 1 && !backward) 
                backward = true;
            else 
                if (index == 0 && backward)
                    backward = false;
            
            index = backward ? index-1 : index+1;
        }
    }

    private static string StringRepresantation()
    {
        CreateKolattzSet();
        string represantation = "";
        for (int i = 0; i < _codedMsg!.Length; i++)
        {
            represantation+=$"{_kollatz![i].ToString()}{_codedMsg[i].ToString()}";
        }
        return represantation;
    }

    private static void CreateKolattzSet()
    {
        Random random = new Random();
        _kollatz = new long[_codedMsg!.Length];
        while (_kollatz[0].ToString().Length!=19)
        {
            _kollatz[0] = random.NextInt64();
        }
        for (int i = 1; i < _kollatz.Length; i++)
        {
            _kollatz[i] = NextKollatz(_kollatz[i - 1]);
        }
    }

    private static long NextKollatz(long previousKollatz)
    {
        if (previousKollatz % 2 == 0)
            return previousKollatz / 2;

        return previousKollatz * 3 + 1;
    }

    private static string Attraction( string msg,  string key)
    {
        string result = "";
        int upperBound = key.Length-2;
        for (int i = 0; i < msg.Length; i++)
        {
            int index = key.IndexOf(msg[i]);
            if (index < 0 || index > upperBound)
                result += msg[i];
            else
                result += key[index+1];
        }
        return result;
    }

}