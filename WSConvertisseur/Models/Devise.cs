using System.ComponentModel.DataAnnotations;

namespace WSConvertisseur.Models
{
    public class Devise
    {
        private int id;
        private string? nomDevise;
        private double taux;

        public Devise()
        {
                
        }

        public Devise(int id, string? nomDevise, double taux)
        {
            this.Id = id;
            this.NomDevise = nomDevise;
            this.Taux = taux;
        }

        [Required]
        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        [Required]
        public string? NomDevise
        {
            get
            {
                return this.nomDevise;
            }

            set
            {
                this.nomDevise = value;
            }
        }

        [Required]
        public double Taux
        {
            get
            {
                return this.taux;
            }

            set
            {
                this.taux = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Devise devise &&
                   this.Id == devise.Id &&
                   this.NomDevise == devise.NomDevise &&
                   this.Taux == devise.Taux;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.NomDevise, this.Taux);
        }
    }
}
