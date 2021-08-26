using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ApiGame.InputModel
{
    public class GameInputModel
    {
        [Required]
        [StringLength(100,MinimumLength =3,ErrorMessage ="The name of the game have to be between 3 and 100 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(100,MinimumLength =1,ErrorMessage ="The name of the Producer have to be between 1 and 100 charecters")]
        public string Producer { get; set; }
        [Required]
        [Range(1,1000,ErrorMessage ="The price has to be between 1 and 1000")]
        public double Price { get; set; }
    }
}
