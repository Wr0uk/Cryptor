using System.Text;

namespace Cryptor;

public class DeCrypt
{
    private static byte[]? _msg, _key;
    
    public static string Translate(string message,string key)
    {
        ToByte(message,key);
        return StringRepresentation();
    }

    private static void ToByte(string msg, string key)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding enc = Encoding.GetEncoding(1251);
        _key = enc.GetBytes(key);
        ClearMsgToByte(msg);
    }

    private static void ClearMsgToByte(string msg)
    {
        List<int> msgIntList = new List<int>();
        long kollatz = long.Parse(msg[..19]);
        long nextKollatz = NextKollatz(kollatz);
        while (msg.Contains(kollatz.ToString()))
        {
            msg = msg.Remove(0,kollatz.ToString().Length);
            var indexOf = msg.IndexOf(nextKollatz.ToString());
            string clearedInt = indexOf >0 ? msg[..indexOf]: msg;
            msg = msg.Remove(0, clearedInt.Length);
            msgIntList.Add( int.Parse(clearedInt));
            kollatz = nextKollatz;
            nextKollatz = NextKollatz(kollatz);
        }

        _msg = new byte[msgIntList.Count];
        int indexKey = 0;
        bool backward = false;
        
        for (int i = 0; i < msgIntList.Count; i++)
        {
            _msg[i] = (byte)(msgIntList[i] - _key![indexKey]);
            if (indexKey == _key.Length - 1 && !backward) 
                backward = true;
            else 
            if (indexKey == 0 && backward)
                backward = false;
            
            indexKey = backward ? indexKey-1 : indexKey+1;
        }
    }

    private static long NextKollatz(long kollatz)
    {
        if (kollatz % 2 == 0)
            return kollatz / 2;
        
        return kollatz * 3 + 1;
    }

    private static string StringRepresentation()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding enc = Encoding.GetEncoding(1251);
        return enc.GetString(_msg!);
    }
}