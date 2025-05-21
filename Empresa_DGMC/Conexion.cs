using MongoDB.Driver;
using System;
using System.Windows.Forms;

namespace Empresa_DGMC
{
    public class MongoDBContext : IDisposable
    {
        private IMongoDatabase _database;
        private readonly string _connectionString = "mongodb://localhost:27017";
        private readonly string _databaseName = "EmpresaDGMC";
        private readonly string _collectionName = "Empleados";
        private IMongoClient _client;
        
        // Indica si la conexión a MongoDB es válida
        public bool ConexionExitosa { get; private set; } = false;

        public MongoDBContext()
        {
            try
            {
                _client = new MongoClient(_connectionString);
                
                // Verificar la conexión intentando obtener la lista de bases de datos
                // Esto lanzará una excepción si no puede conectarse
                _client.ListDatabases();
                
                _database = _client.GetDatabase(_databaseName);
                ConexionExitosa = true;
                
                // Intentar crear índices si la conexión tuvo éxito
                try
                {
                    CrearIndices();
                }
                catch (MongoCommandException ex)
                {
                    // Si el error es por espacio en disco, mostrar un mensaje menos alarmante
                    if (ex.Message.Contains("available disk space"))
                    {
                        MessageBox.Show(
                            "No hay suficiente espacio en disco para crear índices. La aplicación funcionará pero las búsquedas pueden ser más lentas.\n\n" +
                            "Libere espacio en su disco duro para mejorar el rendimiento.",
                            "Advertencia - Espacio en disco insuficiente",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Error al crear los índices: {ex.Message}",
                            "Advertencia",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error inesperado: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (TimeoutException)
            {
                MessageBox.Show(
                    "No se pudo conectar al servidor MongoDB. Verifique que el servicio MongoDB esté iniciado y funcionando en su equipo.\n\n" +
                    "1. Asegúrese de que MongoDB está instalado.\n" +
                    "2. Compruebe que el servicio MongoDB está en ejecución.\n" +
                    "3. Verifique que el puerto 27017 no esté bloqueado por el firewall.\n\n" +
                    "La aplicación seguirá funcionando, pero no podrá guardar ni recuperar datos.",
                    "Error de conexión a MongoDB",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (MongoConnectionException ex)
            {
                MessageBox.Show(
                    $"Error de conexión a MongoDB: {ex.Message}\n\n" +
                    "1. Asegúrese de que MongoDB está instalado.\n" +
                    "2. Compruebe que el servicio MongoDB está en ejecución.\n" +
                    "3. Verifique que el puerto 27017 no esté bloqueado por el firewall.",
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error inesperado: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CrearIndices()
        {
            var collection = Empleados;
            
            // Crear índice por nombre
            var indexKeysDefNombre = Builders<EmpleadosG>.IndexKeys.Ascending(e => e.Nombre);
            collection.Indexes.CreateOne(new CreateIndexModel<EmpleadosG>(indexKeysDefNombre));
            
            // Crear índice por edad
            var indexKeysDefEdad = Builders<EmpleadosG>.IndexKeys.Ascending(e => e.Edad);
            collection.Indexes.CreateOne(new CreateIndexModel<EmpleadosG>(indexKeysDefEdad));
            
            // Crear índice por ciudad
            var indexKeysDefCiudad = Builders<EmpleadosG>.IndexKeys.Ascending(e => e.Ciudad);
            collection.Indexes.CreateOne(new CreateIndexModel<EmpleadosG>(indexKeysDefCiudad));
            
            // Crear índice compuesto por nombre y ciudad
            var indexKeysDefNombreCiudad = Builders<EmpleadosG>.IndexKeys
                .Ascending(e => e.Nombre)
                .Ascending(e => e.Ciudad);
            collection.Indexes.CreateOne(new CreateIndexModel<EmpleadosG>(indexKeysDefNombreCiudad));
        }

        public IMongoCollection<EmpleadosG> Empleados
        {
            get 
            {
                if (!ConexionExitosa)
                {
                    MessageBox.Show(
                        "No hay conexión a MongoDB. La operación no puede completarse.",
                        "Error de conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return null;
                }
                return _database.GetCollection<EmpleadosG>(_collectionName); 
            }
        }
        
        // Métodos para consultas con índices
        public IFindFluent<EmpleadosG, EmpleadosG> BuscarPorNombre(string nombre)
        {
            if (!ConexionExitosa) return null;
            return Empleados.Find(e => e.Nombre.ToLower().Contains(nombre.ToLower()));
        }
        
        public IFindFluent<EmpleadosG, EmpleadosG> BuscarPorEdad(int edad)
        {
            if (!ConexionExitosa) return null;
            return Empleados.Find(e => e.Edad == edad);
        }
        
        public IFindFluent<EmpleadosG, EmpleadosG> BuscarPorEdadRango(int edadMinima, int edadMaxima)
        {
            if (!ConexionExitosa) return null;
            return Empleados.Find(e => e.Edad >= edadMinima && e.Edad <= edadMaxima);
        }
        
        public IFindFluent<EmpleadosG, EmpleadosG> BuscarPorCiudad(string ciudad)
        {
            if (!ConexionExitosa) return null;
            return Empleados.Find(e => e.Ciudad.ToLower().Contains(ciudad.ToLower()));
        }
        
        public IFindFluent<EmpleadosG, EmpleadosG> BuscarPorNombreYCiudad(string nombre, string ciudad)
        {
            if (!ConexionExitosa) return null;
            return Empleados.Find(e => 
                e.Nombre.ToLower().Contains(nombre.ToLower()) && 
                e.Ciudad.ToLower().Contains(ciudad.ToLower()));
        }
        
        // Implementación de IDisposable
        public void Dispose()
        {
            // MongoDB Driver no proporciona un método Dispose explícito para IMongoClient 
            // o IMongoDatabase, pero implementamos IDisposable para cumplir con el patrón.
            // Liberamos las referencias para ayudar al recolector de basura
            _database = null;
            _client = null;
            
            // Forzar recolección de basura
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}