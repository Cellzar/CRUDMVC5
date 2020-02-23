using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectCRUD.Models.Clases
{
    public class Usuario
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string usuarioEntrar { get; set; }
        public string password { get; set; }
        public string Identificacion { get; set; }
        public int Edad { get; set; }
        public string Barrio { get; set; }
    }
}