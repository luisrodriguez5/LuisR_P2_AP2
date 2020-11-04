using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cobros_P2.Models
{
    public class Cobros
    {
        [Key]
        public int CobroId { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public int Totales { get; set; }
        public double TotalCobrado { get; set; }
        public string Observaciones { get; set; }
        
        [ForeignKey("CobroId")]
        public virtual List<CobrosDetalle> Detalle { get; set; } = new List<CobrosDetalle>();
    }

    public class CobrosDetalle
    {
        [Key]
        public int Id { get; set; }
        public int CobroId { get; set; }
        public virtual Cobros Cobro { get; set; }
        public int VentaId { get; set; }
        public virtual Ventas Venta { get; set; }
        public double Cobrado { get; set; }    
    }
}