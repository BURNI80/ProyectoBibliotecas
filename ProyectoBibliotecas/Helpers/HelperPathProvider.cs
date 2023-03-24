namespace ProyectoBibliotecas.Helpers
{
    public enum Folders { Libros = 0, Bibliotecas = 1,Autores = 2, Temporal = 3}

    public class HelperPathProvider
    {
        private IWebHostEnvironment hostEnvironment;

        public HelperPathProvider(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Libros)
            {
                carpeta = Path.Combine("imgs", "libros");
            }
            else if (folder == Folders.Bibliotecas)
            {
                carpeta = Path.Combine("imgs", "bibliotecas");
            }
            else if (folder == Folders.Autores)
            {
                carpeta = Path.Combine("imgs", "autores");
            }
            else if (folder == Folders.Temporal)
            {
                carpeta = "temp";
            }
            string rootPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, carpeta, fileName);
            return path;
        }
    }
}
