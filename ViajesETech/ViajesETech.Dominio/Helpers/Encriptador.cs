using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ViajesETech.Dominio.Helpers
{
    public static class Encriptador
    {
        /// <summary>
        /// Recibe la contraseña para encriptarla.
        /// </summary>
        /// <param name="Contraseña">Recibe la contraseña a encriptar.</param>
        /// <returns>retorna un string con la contraseña encriptada.</returns>
        public static string Cifrar(string Contraseña)
        {
            string resultado = string.Empty;
            SHA256Managed sha256 = new SHA256Managed();
            byte[] contraCifrada = sha256.ComputeHash(Encoding.Default.GetBytes(Contraseña));
            return BitConverter.ToString(contraCifrada).Replace("-", "");
        }
    }
}
