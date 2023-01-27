using System;
using System.Net;
class ShowIP{
    public static void Main(string[] args){
        string name = (args.Length < 1) ? Dns.GetHostName() : args[0];
        try{
            IPAddress[] addrs = Dns.Resolve(name).AddressList;
            foreach(IPAddress addr in addrs) 
                Console.WriteLine("{0}/{1}",name,addr);
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }
    }
}