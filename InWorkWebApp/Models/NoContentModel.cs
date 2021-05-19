namespace InWorkWebApp.Models
{
    public class NoContentModel
    {
        /// <summary>
        /// Crea una instancia del modelo que representa la falta de contenido en la página pasándole parámetros específicos de cada vista
        /// </summary>
        /// <param name="privateHeader"></param>
        /// <param name="privateDetail"></param>
        /// <param name="header"></param>
        /// <param name="detail"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        public NoContentModel(string privateHeader, string privateDetail, string header, string detail, string action, string controller)
        {
            PrivateHeader = privateHeader;
            PrivateDetail = privateDetail;
            PublicHeader = header;
            PublicDetail = detail;
            ActionName = action;
            ControllerName = controller;
        }

        /// <summary>
        /// Crea una instancia del modelo que representa la falta de contenido en la página con valores predeterminados
        /// </summary>
        public NoContentModel()
        {
            PrivateHeader = "No existe información que mostrar.";
            PrivateDetail = "Haz clic en el botón para agregar contenido.";
            PublicHeader = "No existe información que mostrar.";
            PublicDetail = "El contenido que estás buscando no existe o no ha sido dado de alta por el administrador.";
            ActionName = "Dashboard";
            ControllerName = "Manage";
        }

        public string PrivateHeader { get; set; }

        public string PrivateDetail { get; set; }

        public string PublicHeader { get; set; }

        public string PublicDetail { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }
    }
}