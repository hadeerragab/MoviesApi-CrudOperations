using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApi.Models
{
    public class Genre
    {
        //السطر دا عشان احنا عملنا ال id byte مش int  ف بنعرفه انه هيخليه بيدأ من 1 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Byte Id { get; set; }

        [MaxLength(100)]
        public  string Name { get; set; }

    }
}
