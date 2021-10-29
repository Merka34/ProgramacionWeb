using System;
using System.Collections.Generic;

#nullable disable

namespace ProWebU2Zoo.Models
{
    public partial class Clase
    {
        public Clase()
        {
            Especies = new HashSet<Especy>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Especy> Especies { get; set; }
    }
}
