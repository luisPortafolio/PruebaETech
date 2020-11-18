namespace ViajesETech.Dominio.Data
{
    using System;
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
    }

    public class Viajeros
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CI { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
    public class Viajes
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Place { get; set; }                     
        public int IDestination { get; set; }
        public int IdOrigen { get; set; }
        public decimal Price { get; set; }
    }
    public class Destinos
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Rol { get; set; }
    }
}