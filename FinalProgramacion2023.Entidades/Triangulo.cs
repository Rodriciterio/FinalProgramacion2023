namespace FinalProgramacion2023.Entidades
{
    public class Triangulo : ICloneable
    {
        public int LadoA { get; set; }
        public int LadoB { get; set; }
        public int LadoC { get; set; }

        private TipoDeBorde tipoDeBorde;

        public TipoDeBorde TipoDeBorde
        {
            get { return tipoDeBorde; }
            set { tipoDeBorde = value; }
        }

        private TipoDeRelleno tipoDeRelleno;

        public TipoDeRelleno TipoDeRelleno
        {
            get { return tipoDeRelleno; }
            set { tipoDeRelleno = value; }
        }

        public Triangulo()
        {

        }

        public Triangulo(int ladoA, int ladoB, int ladoC, TipoDeBorde borde, TipoDeRelleno relleno)
        {
            LadoA = ladoA;
            LadoB = ladoB;
            LadoC = ladoC;
            TipoDeBorde = borde;
            TipoDeRelleno = relleno;
        }

        public bool ValidarLadoA()
        {
            return LadoA > 0;
        }

        public bool ValidarLadoB()
        {
            return LadoB > 0;
        }

        public bool ValidarLadoC()
        {
            return LadoC > 0;
        }

        public string ObtenerTipoTriangulo()
        {
            if (LadoA == LadoB && LadoB == LadoC)
            {
                return "Equilátero";
            }
            else if (LadoA == LadoB || LadoB == LadoC || LadoA == LadoC)
            {
                return "Isósceles";
            }
            else
            {
                return "Escaleno";
            }
        }

        public bool EsTriangulo()
        {
            return (LadoA + LadoB > LadoC) && (LadoA + LadoC > LadoB) && (LadoB + LadoC > LadoA);
        }

        public string ValidarTriangulo()
        {
            if (EsTriangulo())
            {
                return "Es triangulo";
            }
            else
            {
                return "No es un triagulo";
            }
        }

        public double GetPerimetro() => (LadoA + LadoB + LadoC);
        public double GetArea()
        {
            double p = GetPerimetro();
            return Math.Round(Math.Sqrt(p * (p - LadoA) * (p - LadoB) * (p - LadoC)), 2);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
