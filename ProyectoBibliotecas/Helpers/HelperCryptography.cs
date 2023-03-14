using System.Security.Cryptography;
using System.Text;

namespace ProyectoBibliotecas.Helpers
{
    public class HelperCryptography
    {

        public static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";
            for(int i = 0; i < 50; i++)
            {
                int aleat = random.Next(0, 255);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }
            return salt;
        }

        public static bool ComapreArrays(byte[] a, byte[] b)
        {
            bool iguales = true;
            if(a.Length != b.Length)
            {
                iguales = false;
            }
            else
            {
                for(int i = 0; i<a.Length; i++)
                {
                    if (a[i].Equals(b[i]) == false)
                    {
                        iguales = false;
                        break;
                    }
                }
            }
            return iguales;
        }

        public static byte[] EncryptPassword(string pass, string salt)
        {
            string contenido = pass + salt;
            SHA512 sha = SHA512.Create();
            byte[] salida = Encoding.UTF8.GetBytes(contenido);
            for(int i = 0; i< 24; i++)
            {
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();
            return salida;
        }

    }
}
