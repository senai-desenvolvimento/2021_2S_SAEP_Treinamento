using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Treinamento_Tarde.Models
{
    public class Perfis
    {
        [Key]
        public int Id { get; set; }
        public string Perfil { get; set; }

        public ICollection<Usuarios> Usuarios { get; set; }
        public ICollection<Comentarios> Comentarios { get; set; }
    }
}
