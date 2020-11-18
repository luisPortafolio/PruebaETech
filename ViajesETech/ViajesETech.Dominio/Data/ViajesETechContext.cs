namespace ViajesETech.Dominio.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class ViajesETechContext : DbContext
    {
        // El contexto se ha configurado para usar una cadena de conexión 'ViajesETechContext' del archivo 
        // de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
        // esta cadena de conexión tiene como destino la base de datos 'ViajesETech.Dominio.Data.ViajesETechContext' de la instancia LocalDb. 
        // 
        // Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
        // modifique la cadena de conexión 'ViajesETechContext'  en el archivo de configuración de la aplicación.
        public ViajesETechContext()
            : base("name=ViajesETechContext")
        {
        }

        // Agregue un DbSet para cada tipo de entidad que desee incluir en el modelo. Para obtener más información 
        // sobre cómo configurar y usar un modelo Code First, vea http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Viajeros> Viajeros { get; set; }
        public virtual DbSet<Viajes> Viajes { get; set; }
        public virtual DbSet<Destinos> Destinos { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ViajesViajeros> ViajesViajeros { get; set; }
    }
    [Table("Viajeros")]
    public class Viajeros
    {
        [Key]
        public int Id { get; set; } 
        [Index("CI_Index",IsUnique =true)]
        public int CI { get; set; }
        [MaxLength(100, ErrorMessage = "El campo Address debe contener solo 100 caracteres")]
        public string Address { get; set; }
        [MaxLength(12,ErrorMessage ="El número solo puede contener 12 caracteres.")]
        public string Phone { get; set; }
        [ForeignKey("User")]
        public int IdUser { get; set; }
    }
    [Table("Viajes")]
    public class Viajes
    {
        [Key]               
        public int Id { get; set; }
        [Index("Code_Index")]
        public string Code { get; set; }
        public int Place { get; set; }
        [ForeignKey("Destinos")]
        public int DestinosFin { get; set; }
        [ForeignKey("Destinos")]
        public int DestinosOrigen { get; set; }
        public decimal Price { get; set; }
    }
    [Table("Destinos")]
    public class Destinos
    {
        [Key]
        public int Id { get; set; }
        [Index("IX_Name",IsUnique =true)]
        public string Name { get; set; }
    }
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "El Name solo puede contener 50 caracteres.")]
        public string Name { get; set; }
        [Index("IX_User",IsUnique =true)]
        [MaxLength(20,ErrorMessage ="El userName solo puede contener 20 caracteres.")]
        public string UserName { get; set; }
        [MaxLength(50, ErrorMessage = "El Email solo puede contener 50 caracteres.")]
        public string Email { get; set; }     
        public string Password { get; set; }
        [Required]
        public bool Rol { get; set; }
    }
    [Table("ViejesViajeros")]
    public class ViajesViajeros
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Viajes")]
        public int Viajes { get; set; }
        [ForeignKey("Viajeros")]
        public int Viajeros { get; set; }
    }
}