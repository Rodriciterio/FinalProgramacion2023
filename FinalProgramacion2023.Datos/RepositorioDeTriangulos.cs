using FinalProgramacion2023.Entidades;

namespace FinalProgramacion2023.Datos
{
    public class RepositorioDeTriangulos
    {
        private List<Triangulo> listaTriangulos;
        private readonly string _archivo = Environment.CurrentDirectory + "\\Triangulos.txt";
        private readonly string _archivoCopia = Environment.CurrentDirectory + "\\Triangulos.bak";

        public RepositorioDeTriangulos()
        {
            listaTriangulos = new List<Triangulo>();
            LeerDatos();
        }

        public bool Existe(Triangulo triangulo)
        {
            listaTriangulos.Clear();
            LeerDatos();
            foreach (var itemTriangulo in listaTriangulos)
            {
                if (itemTriangulo.LadoA == triangulo.LadoA &&
                    itemTriangulo.LadoB == triangulo.LadoB &&
                    itemTriangulo.LadoC == triangulo.LadoC &&
                    itemTriangulo.TipoDeBorde == triangulo.TipoDeBorde &&
                    itemTriangulo.TipoDeRelleno == triangulo.TipoDeRelleno)
                {
                    return true;
                }
            }
            return false;
        }

        private void LeerDatos()
        {
            if (File.Exists(_archivo))
            {
                var lector = new StreamReader(_archivo);
                while (!lector.EndOfStream)
                {
                    string lineaLeida = lector.ReadLine();
                    Triangulo triangulo = ConstruirTriangulo(lineaLeida);
                    listaTriangulos.Add(triangulo);
                }
                lector.Close();
            }
        }

        private Triangulo ConstruirTriangulo(string? lineaLeida)
        {
            var campos = lineaLeida.Split('|');
            var ladoA = int.Parse(campos[0]);
            var ladoB = int.Parse(campos[1]);
            var ladoC = int.Parse(campos[2]);
            TipoDeBorde borde = (TipoDeBorde)int.Parse(campos[3]);
            TipoDeRelleno relleno = (TipoDeRelleno)int.Parse(campos[4]);
            Triangulo t = new Triangulo(ladoA, ladoB, ladoC, borde, relleno);
            return t;
        }

        public void Agregar(Triangulo triangulo)
        {
            var escritor = new StreamWriter(_archivo, true);
            string lineaEscribir = ConstruirLinea(triangulo);
            escritor.WriteLine(lineaEscribir);
            escritor.Close();
            listaTriangulos.Add(triangulo);
        }

        private string ConstruirLinea(Triangulo triangulo)
        {
            return $"{triangulo.LadoA} | {triangulo.LadoB} | {triangulo.LadoC} | " +
                $"{triangulo.TipoDeBorde.GetHashCode()} | {triangulo.TipoDeRelleno.GetHashCode()}";
        }

        public void Borrar(Triangulo trianguloBorrar)
        {
            using (var lector = new StreamReader(_archivo))
            {
                using (var escritor = new StreamWriter(_archivoCopia))
                {
                    while (!lector.EndOfStream)
                    {
                        string lineaLeida = lector.ReadLine();
                        Triangulo trianguloLeido = ConstruirTriangulo(lineaLeida);
                        if (trianguloBorrar.LadoA != trianguloLeido.LadoA &&
                            trianguloBorrar.LadoB != trianguloLeido.LadoB &&
                            trianguloBorrar.LadoC != trianguloLeido.LadoC)
                        {
                            escritor.WriteLine(lineaLeida);
                        }
                    }
                }
            }
            File.Delete(_archivo);
            File.Move(_archivoCopia, _archivo);
            listaTriangulos.Remove(trianguloBorrar);
        }

        public void Editar(Triangulo trianguloEnArchivo, Triangulo trianguloEditar)
        {
            using (var lector = new StreamReader(_archivo))
            {
                using (var escritor = new StreamWriter(_archivoCopia))
                {
                    while (!lector.EndOfStream)
                    {
                        string lineaLeida = lector.ReadLine();
                        Triangulo triangulo = ConstruirTriangulo(lineaLeida);
                        if (trianguloEnArchivo.LadoA != triangulo.LadoA &&
                            trianguloEnArchivo.LadoB != triangulo.LadoB &&
                            trianguloEnArchivo.LadoC != triangulo.LadoC)
                        {
                            escritor.WriteLine(lineaLeida);
                        }
                        else
                        {
                            lineaLeida = ConstruirLinea(trianguloEditar);
                            escritor.WriteLine(lineaLeida);
                        }
                    }
                }
            }
            File.Delete(_archivo);
            File.Move(_archivoCopia, _archivo);
            //Actualización de la lista con el valor editado para que permanezca.
            int indiceTriangulo = listaTriangulos.FindIndex(t =>
                t.LadoA == trianguloEnArchivo.LadoA &&
                t.LadoB == trianguloEnArchivo.LadoB &&
                t.LadoC == trianguloEnArchivo.LadoC);

            if (indiceTriangulo != -1)
            {
                listaTriangulos[indiceTriangulo] = trianguloEditar;
            }
        }

        public int GetCantidad(int valorFiltro = 0)
        {
            if (valorFiltro > 0)
            {
                return listaTriangulos.Count(t => t.LadoA >= valorFiltro &&
                t.LadoB >= valorFiltro && t.LadoC >= valorFiltro);
            }
            return listaTriangulos.Count;
        }

        public List<Triangulo> GetLista()
        {
            return listaTriangulos;
        }

        public List<Triangulo> Filtrar(TipoDeRelleno rellenoFiltro)
        {
            if (rellenoFiltro != TipoDeRelleno.Seleccione)
            {
                return listaTriangulos.Where(t => t.TipoDeRelleno == rellenoFiltro).ToList();

            }
            return listaTriangulos;
        }

        public List<Triangulo> Filtrar(int intValor)
        {
            return listaTriangulos.Where(t => t.GetArea() >= intValor).ToList();
        }

        public List<Triangulo> OrdenarAsc()
        {
            return listaTriangulos.OrderBy(t => t.GetPerimetro()).ToList();
        }

        public List<Triangulo> OrdenarDesc()
        {
            return listaTriangulos.OrderByDescending(t => t.GetPerimetro()).ToList();
        }
    }
}
