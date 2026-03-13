using System;

namespace Magazyn_WPF.Models
{
    public class Produkt
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Kategoria { get; set; }
        public int Ilość { get; set; }
        public string Jednostka { get; set; }
        public string Lokalizacja { get; set; }
        public DateTime DataDodania { get; set; }
    }
}
