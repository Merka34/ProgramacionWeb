using System;
using System.Collections.Generic;

#nullable disable

namespace ProWebU2Zoo.Models
{
    public partial class Especy
    {
        public int Id { get; set; }
        public string Especie { get; set; }
        public int? IdClase { get; set; }
        public string Habitat { get; set; }
        public double? Peso { get; set; }
        public int? Tamaño { get; set; }
        public string Observaciones { get; set; }

        public virtual Clase IdClaseNavigation { get; set; }
    }
}
