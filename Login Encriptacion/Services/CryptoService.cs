using Microsoft.EntityFrameworkCore.Update;
using System.Security.Cryptography;
using System.Text;

namespace Login_Encriptacion.Services
{
    public class CryptoService
    {
        //Login_Encriptacion simetrica (AES)
        private readonly byte[] _keySim; //llave secreta AES
        private readonly byte[] _ivSim; //VECTOR DE INICIO AES
        private static readonly RSAParameters _privateKey; //llave privada RSA
        private static readonly RSAParameters _publicKey; //llave publlica RSA

        public CryptoService(IConfiguration config)
        {
            _keySim = Convert.FromBase64String(config["CriptoConfig:KeySimetrica"]);
            _ivSim = Encoding.UTF8.GetBytes(config["CriptoConfig:IVSimetrica"]);
        }
      
        //Inicializador estático para RSA(Crea el par de llaves asimétricas una sola vez)
        static CryptoService()
        {
            using var rsa = new RSACryptoServiceProvider(2048);
            _privateKey = rsa.ExportParameters(true);
            _publicKey = rsa.ExportParameters(false);
        }

        //HASHIN PARA LA CONTRASEÑA
        public  string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
        //ENCRIPTACION AES
        public string EncriptarSimetrico(string text)
        {
            using var aes = Aes.Create();
            using var enc = aes.CreateEncryptor(_keySim, _ivSim);
            var buffer = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(enc.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public string DesencriptarSimetrico(string cifrado)
        {
            using var aes= Aes.Create();
            using var dec= aes.CreateDecryptor(_keySim, _ivSim);
            var buffer=Convert.FromBase64String(cifrado);
            return Encoding.UTF8.GetString(dec.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        //ENCRIPTAR RSA
        public string EncriptarAsimetrico(string text)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(_publicKey);
            return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(text), false));
        }
        public string DesencriptarAsimetrico(string cifrado)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(_privateKey);
            return Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(cifrado), false));
        }
    }
}
