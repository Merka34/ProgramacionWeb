using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProWebU1Act2.Models
{
    public class IndexViewModel
    {
        public int Calificacion1 { get; set; }
        public int Calificacion2 { get; set; }
        public int Calificacion3 { get; set; }
        public int CalificacionFinal { get { return (Calificacion1 + Calificacion2 + Calificacion3) / 3; } }

        public string Estado
        {
            get
            {
                if (CalificacionFinal >= 70)
                    return "Aprobado";
                else
                    return "Reprobado";
            }
        }
    }
}
