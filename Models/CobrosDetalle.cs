using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuisR_P2_AP2.Models
{
    public class CobrosDetalle
    {
        [Key]
        public int CobroDetalleId { get; set; }
        public int CobroId { get; set; }
        public int VentaId { get; set; }
        public DateTime Fecha { get; set; }
        public double Monto { get; set; }
        public double Balance { get; set; }
        public double Cobrado { get; set; }
       

        public CobrosDetalle()
        {
            CobroDetalleId = 0;
            CobroId = 0;
            VentaId = 0;
            Fecha = DateTime.Now;
            Monto = 0;
            Balance = 0;
            Cobrado =0;
        }
    }
}