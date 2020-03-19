using System;
using System.Security.Cryptography;

namespace Cryptography
{
    class Program
    {
        static void Main(string[] args)
        {
            var myString = "A complete set of graphical user interface (GUI) to build 2D video games. With outer space theme and vibrant colors.";
            var myString2 = "The data protection system is built upon two core concepts - a data protection provider (represented by the IDataProtectionProvider interface), which is used to create a data protector (represented by the IDataProtector interface). The data protector is used to encrypt and decrypt data. Because the data protection system has been added to the application's services collection by default, it can be made available via dependency injection. Here's how you can inject the IDataProtectionProvider into a controller and then use it to create an instance of an IDataProtector in the controller's constructor:";
            var myString3 = "2347587";

            var encryptText = EncryptionHelper.Encrypt(myString);
            Console.WriteLine(encryptText);
            Console.WriteLine(EncryptionHelper.Decrypt(encryptText));
            var encryptText2 = EncryptionHelper.Encrypt(myString2);
            Console.WriteLine(encryptText2);
            Console.WriteLine(EncryptionHelper.Decrypt(encryptText2));
            var encryptText3 = EncryptionHelper.Encrypt(myString3);
            Console.WriteLine(encryptText3);
            Console.WriteLine(EncryptionHelper.Decrypt(encryptText3));
            Console.ReadLine();

        }
    }
}
