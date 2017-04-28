using System;
using System.Text;

namespace ESBackupServer.App.Objects.Factories.Registration
{
    internal class PasswordFactory
    {
        internal string Generate(int length)
        {
            string chars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!#$%&'()*+,-./0123456789:;<=>?@[]^_{}|";
            StringBuilder sb = new StringBuilder(length);
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[r.Next(chars.Length)]);
            }
            return sb.ToString();
        }
    }
}