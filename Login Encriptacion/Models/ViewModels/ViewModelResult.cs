namespace Login_Encriptacion.Models.ViewModels
{
    public class ViewModelResult
    {
        public string Original { get; set; } // El texto que escribió el usuario.
        public string SimetricoCifrado { get; set; } // Resultado de AES.
        public string AsimetricoCifrado { get; set; } // Resultado de RSA.
    }
}
