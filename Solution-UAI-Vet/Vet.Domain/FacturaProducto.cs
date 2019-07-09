﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vet.Domain
{
   public partial class FacturaProducto : Factura
    {


    }

    [MetadataType(typeof(FacturaProductoMetadata))]

    public partial class FacturaProducto
    {
        public class FacturaProductoMetadata
        {
            [Key]
            [Column(Order = 1)]
            public int Id { get; set; }
            [ForeignKey("Cliente")]
            [Column(Order = 2)]
            [Required]
            public int IdCliente { get; set; }
            [Required]
            public DateTime Fecha { get; set; }
            [Required]
            public double Monto { get; set; }
        }
    }

}