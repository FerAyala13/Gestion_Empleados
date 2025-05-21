using System;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Diagnostics;

namespace Empresa_DGMC
{
    public partial class EmpleadosForm : Form
    {
        private MongoDBContext _context;
        private string _selectedId;

        public EmpleadosForm()
        {
            InitializeComponent();
            
            try
            {
                // Configurar el DataGridView
                ConfigurarDataGridView();
                
                // Inicializar la conexión a MongoDB
                _context = new MongoDBContext();
                
                // Verificar si se pudo establecer conexión con MongoDB
                if (!_context.ConexionExitosa)
                {
                    // Deshabilitar controles si no hay conexión
                    DeshabilitarControles();
                    return;
                }
                
                // Cargar datos iniciales
                CargarEmpleadosDirecto();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la aplicación: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void DeshabilitarControles()
        {
            // Deshabilitar los controles de entrada
            txtNombre.Enabled = false;
            txtEdad.Enabled = false;
            txtCiudad.Enabled = false;
            txtCorreo.Enabled = false;
            
            // Deshabilitar los botones
            btnAgregar.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            
            // Deshabilitar los controles de búsqueda
            txtBuscarNombre.Enabled = false;
            txtBuscarCiudad.Enabled = false;
            txtBuscarEdadMin.Enabled = false;
            txtBuscarEdadMax.Enabled = false;
            btnBuscarNombre.Enabled = false;
            btnBuscarCiudad.Enabled = false;
            btnBuscarEdad.Enabled = false;
            btnBuscarNombreCiudad.Enabled = false;
            btnMostrarTodos.Enabled = false;
            
            // Mostrar mensaje en el DataGridView
            dataGridEmpleados.DataSource = new List<string> { "No hay conexión a MongoDB" };
        }

        private void ConfigurarDataGridView()
        {
            // Configurar aspecto y comportamiento del DataGridView
            dataGridEmpleados.AutoGenerateColumns = true;
            dataGridEmpleados.AllowUserToAddRows = false;
            dataGridEmpleados.AllowUserToDeleteRows = false;
            dataGridEmpleados.ReadOnly = true;
            dataGridEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridEmpleados.MultiSelect = false;
            dataGridEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarEmpleados()
        {
            try
            {
                if (!_context.ConexionExitosa)
                {
                    DeshabilitarControles();
                    return;
                }
                
                // Obtener la lista de empleados
                var empleados = _context.Empleados.Find(e => true).ToList();
                
                // Si tenemos datos válidos, asignarlos al DataGridView
                if (empleados != null && empleados.Count > 0)
                {
                    dataGridEmpleados.DataSource = null;
                    dataGridEmpleados.DataSource = empleados;
                }
                else
                {
                    // No hay datos, limpiamos el DataGridView
                    dataGridEmpleados.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar empleados: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Mensaje para verificar que el evento se está activando
                MessageBox.Show("Iniciando proceso de agregar empleado", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Validar campos
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtEdad.Text))
                {
                    MessageBox.Show("La edad es obligatoria", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEdad.Focus();
                    return;
                }
                
                if (!int.TryParse(txtEdad.Text, out int edad))
                {
                    MessageBox.Show("La edad debe ser un número", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEdad.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtCiudad.Text))
                {
                    MessageBox.Show("La ciudad es obligatoria", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCiudad.Focus();
                    return;
                }
                
                MessageBox.Show("Validación de campos completada", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                try
                {
                    // Conexión directa a MongoDB
                    MessageBox.Show("Iniciando conexión a MongoDB", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var connectionString = "mongodb://localhost:27017";
                    var client = new MongoClient(connectionString);
                    
                    // Probar la conexión
                    var databaseList = client.ListDatabases().ToList();
                    MessageBox.Show($"Conexión exitosa. Bases de datos disponibles: {databaseList.Count}", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    var database = client.GetDatabase("EmpresaDGMC");
                    var collection = database.GetCollection<EmpleadosG>("Empleados");
                    
                    // Contar documentos antes de la inserción
                    var countAntes = collection.CountDocuments(new BsonDocument());
                    MessageBox.Show($"Documentos antes de insertar: {countAntes}", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Crear un nuevo empleado
                    var empleado = new EmpleadosG
                    {
                        Nombre = txtNombre.Text,
                        Edad = edad,
                        Ciudad = txtCiudad.Text,
                        Correo = txtCorreo.Text ?? string.Empty // Evitar nulls
                    };
                    
                    // Verificar el ID generado
                    MessageBox.Show($"ID generado para el empleado: {empleado.Id}", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Insertarlo directamente
                    MessageBox.Show("Intentando insertar empleado...", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    collection.InsertOne(empleado);
                    
                    // Contar documentos después de la inserción
                    var countDespues = collection.CountDocuments(new BsonDocument());
                    MessageBox.Show($"Documentos después de insertar: {countDespues}", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Verificar que el documento se insertó realmente
                    var filtro = Builders<EmpleadosG>.Filter.Eq(emp => emp.Id, empleado.Id);
                    var empleadoInsertado = collection.Find(filtro).FirstOrDefault();
                    
                    if (empleadoInsertado != null)
                    {
                        MessageBox.Show($"Empleado verificado en la base de datos: {empleadoInsertado.Nombre}", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("ERROR: El empleado no se encontró después de la inserción", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    // Limpiar los campos del formulario
                    LimpiarCampos();
                    
                    // Cargar los datos actualizados - usar método explícito
                    MessageBox.Show("Actualizando DataGridView...", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Limpiar el DataGridView
                    dataGridEmpleados.DataSource = null;
                    dataGridEmpleados.Refresh();
                    
                    // Cargar los datos frescos
                    var todosEmpleados = collection.Find(new BsonDocument()).ToList();
                    dataGridEmpleados.DataSource = todosEmpleados;
                    
                    MessageBox.Show($"Empleados cargados en el DataGridView: {todosEmpleados.Count}", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Mensaje final
                    MessageBox.Show("Empleado agregado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MongoException mongoEx)
                {
                    MessageBox.Show($"Error específico de MongoDB: {mongoEx.Message}\n\n" +
                        $"Tipo: {mongoEx.GetType().Name}\n" +
                        $"Stack: {mongoEx.StackTrace}", 
                        "Error de MongoDB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar empleado: {ex.Message}\n\n" +
                    $"Tipo: {ex.GetType().Name}\n" +
                    $"Detalles: {ex.StackTrace}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método simplificado para agregar empleados
        private void btnAgregarEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtEdad.Text))
                {
                    MessageBox.Show("La edad es obligatoria", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEdad.Focus();
                    return;
                }
                
                if (!int.TryParse(txtEdad.Text, out int edad))
                {
                    MessageBox.Show("La edad debe ser un número", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEdad.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtCiudad.Text))
                {
                    MessageBox.Show("La ciudad es obligatoria", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCiudad.Focus();
                    return;
                }
                
                // Crear un nuevo documento BSON directamente (en vez del modelo EmpleadosG)
                var document = new BsonDocument
                {
                    { "Nombre", txtNombre.Text },
                    { "Edad", edad },
                    { "Ciudad", txtCiudad.Text },
                    { "Correo", !string.IsNullOrWhiteSpace(txtCorreo.Text) ? txtCorreo.Text : string.Empty }
                };
                
                // Conexión directa a MongoDB
                var connectionString = "mongodb://localhost:27017";
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("EmpresaDGMC");
                
                // IMPORTANTE: Usar "Empleados" (con mayúscula) como nombre de colección
                var collection = database.GetCollection<BsonDocument>("Empleados");
                
                // Insertar el documento
                collection.InsertOne(document);
                
                // Notificar inserción exitosa
                MessageBox.Show($"Documento insertado con éxito en la colección 'Empleados'.", 
                    "Inserción exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Limpiar los campos
                LimpiarCampos();
                
                // Actualizar el DataGridView
                var empleados = collection.Find(new BsonDocument()).ToList();
                
                // Convertir los documentos BSON a objetos EmpleadosG para el DataGridView
                List<EmpleadosG> listaEmpleados = new List<EmpleadosG>();
                
                foreach (var doc in empleados)
                {
                    try
                    {
                        var emp = new EmpleadosG
                        {
                            Id = doc.Contains("_id") ? doc["_id"].ToString() : ObjectId.GenerateNewId().ToString(),
                            Nombre = doc.Contains("Nombre") ? doc["Nombre"].AsString : "",
                            Edad = doc.Contains("Edad") ? doc["Edad"].AsInt32 : 0,
                            Ciudad = doc.Contains("Ciudad") ? doc["Ciudad"].AsString : "",
                            Correo = doc.Contains("Correo") ? doc["Correo"].AsString : ""
                        };
                        
                        listaEmpleados.Add(emp);
                    }
                    catch (Exception docEx)
                    {
                        // Ignorar documentos problemáticos, pero seguir procesando el resto
                    }
                }
                
                dataGridEmpleados.DataSource = null;
                dataGridEmpleados.DataSource = listaEmpleados;
                
                MessageBox.Show($"Se han cargado {listaEmpleados.Count} empleados en el DataGridView", 
                    "Actualización completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar empleado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método simplificado para cargar empleados
        private void CargarEmpleadosSimple()
        {
            try
            {
                // Conexión directa a MongoDB
                var connectionString = "mongodb://localhost:27017";
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("EmpresaDGMC");
                var collection = database.GetCollection<BsonDocument>("Empleados");
                
                // Desconectar el DataGridView antes de cargar nuevos datos
                dataGridEmpleados.DataSource = null;
                
                // Obtener todos los documentos BSON
                var documentos = collection.Find(new BsonDocument()).ToList();
                
                // Convertir los documentos BSON a objetos EmpleadosG para el DataGridView
                List<EmpleadosG> empleados = new List<EmpleadosG>();
                
                foreach (var doc in documentos)
                {
                    try
                    {
                        var emp = new EmpleadosG
                        {
                            Id = doc.Contains("_id") ? doc["_id"].ToString() : ObjectId.GenerateNewId().ToString(),
                            Nombre = doc.Contains("Nombre") ? doc["Nombre"].AsString : "",
                            Edad = doc.Contains("Edad") ? doc["Edad"].AsInt32 : 0,
                            Ciudad = doc.Contains("Ciudad") ? doc["Ciudad"].AsString : "",
                            Correo = doc.Contains("Correo") ? doc["Correo"].AsString : ""
                        };
                        
                        empleados.Add(emp);
                    }
                    catch (Exception docEx)
                    {
                        // Solo registrar el error pero continuar con los demás documentos
                        System.Diagnostics.Debug.WriteLine($"Error al procesar documento: {docEx.Message}");
                    }
                }
                
                // Asignar los datos al DataGridView
                dataGridEmpleados.DataSource = empleados;
                
                // Actualizar la vista
                dataGridEmpleados.Refresh();
                
                // Mostrar mensaje si no hay datos
                if (empleados.Count == 0)
                {
                    MessageBox.Show("No se encontraron empleados en la base de datos.", 
                        "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar empleados: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método específico para cargar empleados directamente sin cachés
        private void CargarEmpleadosDirecto()
        {
            try
            {
                if (!_context.ConexionExitosa)
                {
                    DeshabilitarControles();
                    return;
                }
                
                // Limpiar la selección actual
                _selectedId = null;
                LimpiarCampos();
                
                try
                {
                    // Limpiar el DataGridView antes de cargar nuevos datos
                    dataGridEmpleados.DataSource = null;
                    dataGridEmpleados.Refresh();
                    
                    // Conexión directa a MongoDB
                    var connectionString = "mongodb://localhost:27017";
                    var client = new MongoClient(connectionString);
                    var database = client.GetDatabase("EmpresaDGMC");
                    var collection = database.GetCollection<BsonDocument>("Empleados");
                    
                    // Obtener todos los documentos
                    var documentos = collection.Find(new BsonDocument()).ToList();
                    
                    // Convertir a objetos EmpleadosG
                    var listaEmpleados = new List<EmpleadosG>();
                    
                    foreach (var doc in documentos)
                    {
                        try
                        {
                            // Crear un objeto EmpleadosG a partir del documento BsonDocument
                            var emp = new EmpleadosG
                            {
                                Id = doc["_id"].AsObjectId.ToString(),
                                Nombre = doc.Contains("Nombre") ? doc["Nombre"].AsString : "",
                                Edad = doc.Contains("Edad") ? doc["Edad"].AsInt32 : 0,
                                Ciudad = doc.Contains("Ciudad") ? doc["Ciudad"].AsString : "",
                                Correo = doc.Contains("Correo") ? doc["Correo"].AsString : ""
                            };
                            
                            listaEmpleados.Add(emp);
                        }
                        catch (Exception ex)
                        {
                            // Solo registrar el error pero continuar con el resto
                            Debug.WriteLine($"Error al procesar documento: {ex.Message}");
                        }
                    }
                    
                    // Asignar los datos al DataGridView
                    dataGridEmpleados.DataSource = listaEmpleados;
                    
                    // Actualizar la vista
                    dataGridEmpleados.Refresh();
                    
                    // Configurar apariencia del DataGridView después de asignar datos
                    ConfigurarColumnasDataGrid();
                    
                    // Mostrar mensaje solo si no hay datos
                    if (listaEmpleados.Count == 0)
                    {
                        MessageBox.Show("No se encontraron empleados en la base de datos.", 
                            "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar los empleados: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico al cargar empleados: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnasDataGrid()
        {
            // Solo proceder si hay columnas
            if (dataGridEmpleados.Columns.Count > 0)
            {
                try
                {
                    // Configurar visibilidad y títulos de columnas
                    foreach (DataGridViewColumn column in dataGridEmpleados.Columns)
                    {
                        switch (column.Name)
                        {
                            case "Id":
                                column.HeaderText = "ID";
                                column.Visible = true;
                                break;
                            case "Nombre":
                                column.HeaderText = "Nombre";
                                column.Visible = true;
                                break;
                            case "Edad":
                                column.HeaderText = "Edad";
                                column.Visible = true;
                                break;
                            case "Ciudad":
                                column.HeaderText = "Ciudad";
                                column.Visible = true;
                                break;
                            case "Correo":
                                column.HeaderText = "Correo";
                                column.Visible = true;
                                break;
                            case "FechaPrueba":
                                column.HeaderText = "Fecha de Prueba";
                                column.Visible = false; // Ocultamos esta columna
                                break;
                            default:
                                // Otras columnas no reconocidas
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ignorar errores en la configuración de columnas
                    Debug.WriteLine($"Error al configurar columnas: {ex.Message}");
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar que se haya seleccionado un registro
                if (string.IsNullOrEmpty(_selectedId))
                {
                    MessageBox.Show("Por favor, seleccione un empleado para modificar", 
                        "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar campos
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtEdad.Text))
                {
                    MessageBox.Show("La edad es obligatoria", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEdad.Focus();
                    return;
                }
                
                if (!int.TryParse(txtEdad.Text, out int edad))
                {
                    MessageBox.Show("La edad debe ser un número", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEdad.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtCiudad.Text))
                {
                    MessageBox.Show("La ciudad es obligatoria", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCiudad.Focus();
                    return;
                }
                
                // Confirmar modificación
                var confirmacion = MessageBox.Show(
                    $"¿Está seguro que desea modificar el registro de '{txtNombre.Text}'?", 
                    "Confirmar modificación", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);
                    
                if (confirmacion != DialogResult.Yes)
                {
                    return;
                }
                
                // Crear conexión a MongoDB
                var connectionString = "mongodb://localhost:27017";
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("EmpresaDGMC");
                var collection = database.GetCollection<BsonDocument>("Empleados");
                
                // Convertir el _selectedId a ObjectId para buscar el documento
                ObjectId objectId;
                if (!ObjectId.TryParse(_selectedId, out objectId))
                {
                    MessageBox.Show("El ID seleccionado no es válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Crear el filtro para encontrar el documento por ID
                var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
                
                // Crear el documento actualizado
                var update = Builders<BsonDocument>.Update
                    .Set("Nombre", txtNombre.Text)
                    .Set("Edad", edad)
                    .Set("Ciudad", txtCiudad.Text)
                    .Set("Correo", !string.IsNullOrWhiteSpace(txtCorreo.Text) ? txtCorreo.Text : string.Empty);
                
                // Ejecutar la actualización
                var result = collection.UpdateOne(filter, update);
                
                // Verificar si se actualizó el registro
                if (result.ModifiedCount > 0)
                {
                    // Limpiar los campos y el ID seleccionado
                    LimpiarCampos();
                    
                    // Actualizar el DataGridView con los datos actualizados
                    CargarEmpleadosDirecto();
                    
                    MessageBox.Show("Empleado modificado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result.MatchedCount > 0)
                {
                    MessageBox.Show("No se realizaron cambios en el empleado.", 
                        "Sin cambios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo encontrar el empleado a modificar.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar empleado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedId))
            {
                MessageBox.Show("Por favor, seleccione un empleado para eliminar");
                return;
            }

            try
            {
                if (!_context.ConexionExitosa)
                {
                    MessageBox.Show("No hay conexión a MongoDB. La operación no puede completarse.", 
                        "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Pedir confirmación antes de eliminar
                DialogResult confirmacion = MessageBox.Show(
                    $"¿Está seguro de eliminar al empleado {txtNombre.Text}?",
                    "Confirmar eliminación", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);
                    
                if (confirmacion == DialogResult.No)
                {
                    return;
                }
                
                var filter = Builders<EmpleadosG>.Filter.Eq(emp => emp.Id, _selectedId);
                
                // Eliminar el empleado
                _context.Empleados.DeleteOne(filter);
                
                LimpiarCampos();
                // Forzar la recarga de datos desde la base de datos
                CargarEmpleadosDirecto();
                MessageBox.Show("Empleado eliminado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar empleado: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridEmpleados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Solo procesamos si hay un clic en una fila válida (no en el encabezado)
                if (e.RowIndex >= 0 && e.RowIndex < dataGridEmpleados.Rows.Count)
                {
                    // Obtener la fila seleccionada
                    DataGridViewRow row = dataGridEmpleados.Rows[e.RowIndex];
                    
                    // Para hacer diagnóstico, veamos qué tipo de objeto tenemos
                    if (row.DataBoundItem != null)
                    {
                        // Estrategia 1: Usar casting seguro para EmpleadosG
                        if (row.DataBoundItem is EmpleadosG empleado)
                        {
                            // Guardar el ID seleccionado
                            _selectedId = empleado.Id;
                            
                            // Llenar los campos del formulario
                            txtNombre.Text = empleado.Nombre;
                            txtEdad.Text = empleado.Edad.ToString();
                            txtCiudad.Text = empleado.Ciudad;
                            txtCorreo.Text = empleado.Correo ?? string.Empty;
                            
                            // Solo mostrar un mensaje simple de confirmación
                            MessageBox.Show($"Empleado {empleado.Nombre} cargado para edición.", 
                                "Registro seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return; // Salir ya que hemos procesado todo
                        }
                        
                        // Estrategia 2: Intentar recuperar los valores directamente de las celdas
                        try
                        {
                            // Buscar las columnas por índice
                            _selectedId = null;
                            
                            // Intentar encontrar columna de ID
                            if (dataGridEmpleados.Columns.Contains("Id"))
                                _selectedId = row.Cells["Id"].Value?.ToString();
                            else if (dataGridEmpleados.Columns.Contains("_id"))
                                _selectedId = row.Cells["_id"].Value?.ToString();
                            
                            // Obtener el resto de valores por nombre de columna verificando si existen
                            if (dataGridEmpleados.Columns.Contains("Nombre"))
                                txtNombre.Text = row.Cells["Nombre"].Value?.ToString() ?? string.Empty;
                                
                            if (dataGridEmpleados.Columns.Contains("Edad"))
                                txtEdad.Text = row.Cells["Edad"].Value?.ToString() ?? string.Empty;
                                
                            if (dataGridEmpleados.Columns.Contains("Ciudad"))
                                txtCiudad.Text = row.Cells["Ciudad"].Value?.ToString() ?? string.Empty;
                                
                            if (dataGridEmpleados.Columns.Contains("Correo"))
                                txtCorreo.Text = row.Cells["Correo"].Value?.ToString() ?? string.Empty;
                            
                            MessageBox.Show("Datos cargados desde el grid. Puede modificarlos y guardar los cambios.", 
                                "Registro cargado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception cellEx)
                        {
                            MessageBox.Show($"Error al procesar celdas: {cellEx.Message}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener datos del registro seleccionado.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el registro: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método auxiliar para obtener valores de celdas de manera segura
        private string GetCellValue(DataGridViewRow row, string columnName)
        {
            try
            {
                // Buscar la columna por nombre
                if (dataGridEmpleados.Columns.Contains(columnName))
                {
                    var cell = row.Cells[columnName];
                    if (cell != null && cell.Value != null)
                        return cell.Value.ToString();
                }
            }
            catch (Exception)
            {
                // Ignorar errores y devolver cadena vacía
            }
            
            return string.Empty;
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtEdad.Text = string.Empty;
            txtCiudad.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            _selectedId = null;
        }
        
        // Implementación de los eventos de botón para búsquedas con índices
        private void btnBuscarNombre_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_context.ConexionExitosa)
                {
                    MessageBox.Show("No hay conexión a MongoDB. La operación no puede completarse.", 
                        "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtBuscarNombre.Text))
                {
                    MessageBox.Show("Ingrese un nombre para buscar");
                    return;
                }
                
                // Usar la búsqueda por índice de nombre
                var resultados = _context.BuscarPorNombre(txtBuscarNombre.Text).ToList();
                dataGridEmpleados.DataSource = resultados;
                
                MessageBox.Show($"Se encontraron {resultados.Count} resultados");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar por nombre: {ex.Message}");
            }
        }
        
        private void btnBuscarCiudad_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_context.ConexionExitosa)
                {
                    MessageBox.Show("No hay conexión a MongoDB. La operación no puede completarse.", 
                        "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtBuscarCiudad.Text))
                {
                    MessageBox.Show("Ingrese una ciudad para buscar");
                    return;
                }
                
                // Usar la búsqueda por índice de ciudad
                var resultados = _context.BuscarPorCiudad(txtBuscarCiudad.Text).ToList();
                dataGridEmpleados.DataSource = resultados;
                
                MessageBox.Show($"Se encontraron {resultados.Count} resultados");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar por ciudad: {ex.Message}");
            }
        }
        
        private void btnBuscarEdad_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_context.ConexionExitosa)
                {
                    MessageBox.Show("No hay conexión a MongoDB. La operación no puede completarse.", 
                        "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Verificar si se busca por un rango o por una edad específica
                if (!string.IsNullOrWhiteSpace(txtBuscarEdadMin.Text) && 
                    !string.IsNullOrWhiteSpace(txtBuscarEdadMax.Text))
                {
                    // Buscar por rango
                    int edadMin = int.Parse(txtBuscarEdadMin.Text);
                    int edadMax = int.Parse(txtBuscarEdadMax.Text);
                    
                    var resultados = _context.BuscarPorEdadRango(edadMin, edadMax).ToList();
                    dataGridEmpleados.DataSource = resultados;
                    
                    MessageBox.Show($"Se encontraron {resultados.Count} resultados entre {edadMin} y {edadMax} años");
                }
                else if (!string.IsNullOrWhiteSpace(txtBuscarEdadMin.Text))
                {
                    // Buscar por edad específica
                    int edad = int.Parse(txtBuscarEdadMin.Text);
                    
                    var resultados = _context.BuscarPorEdad(edad).ToList();
                    dataGridEmpleados.DataSource = resultados;
                    
                    MessageBox.Show($"Se encontraron {resultados.Count} resultados con edad {edad}");
                }
                else
                {
                    MessageBox.Show("Ingrese al menos la edad mínima");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingrese números válidos en los campos de edad");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar por edad: {ex.Message}");
            }
        }
        
        private void btnBuscarNombreCiudad_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_context.ConexionExitosa)
                {
                    MessageBox.Show("No hay conexión a MongoDB. La operación no puede completarse.", 
                        "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtBuscarNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtBuscarCiudad.Text))
                {
                    MessageBox.Show("Ingrese un nombre y una ciudad para buscar");
                    return;
                }
                
                // Usar la búsqueda compuesta por nombre y ciudad (índice compuesto)
                var resultados = _context.BuscarPorNombreYCiudad(
                    txtBuscarNombre.Text, 
                    txtBuscarCiudad.Text).ToList();
                    
                dataGridEmpleados.DataSource = resultados;
                
                MessageBox.Show($"Se encontraron {resultados.Count} resultados");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar por nombre y ciudad: {ex.Message}");
            }
        }
        
        private void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            if (!_context.ConexionExitosa)
            {
                MessageBox.Show("No hay conexión a MongoDB. La operación no puede completarse.", 
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Limpiar los campos de búsqueda
            txtBuscarNombre.Text = string.Empty;
            txtBuscarCiudad.Text = string.Empty;
            txtBuscarEdadMin.Text = string.Empty;
            txtBuscarEdadMax.Text = string.Empty;
            
            // Cargar todos los empleados
            CargarEmpleados();
        }

        // Método de prueba directo para MongoDB
        private void btnPruebaMongoDB_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Iniciando prueba directa de MongoDB...", "Prueba", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Limpiar la consola
                Debug.WriteLine("\n\n==== PRUEBA DIRECTA DE MONGODB ====");
                
                // Crear conexión directa sin usar nuestra clase MongoDBContext
                var connectionString = "mongodb://localhost:27017";
                var client = new MongoClient(connectionString);
                
                Debug.WriteLine("Conexión creada.");
                
                // Listar las bases de datos para verificar la conexión
                var databases = client.ListDatabases().ToList();
                string dbList = "Bases de datos en el servidor: ";
                foreach (var db in databases)
                {
                    dbList += db["name"].ToString() + ", ";
                    Debug.WriteLine($"Base de datos: {db["name"]}");
                }
                
                MessageBox.Show(dbList, "Bases de datos disponibles", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Obtener o crear la base de datos
                var database = client.GetDatabase("EmpresaDGMC");
                Debug.WriteLine("Base de datos seleccionada: EmpresaDGMC");
                
                // Usar una colección diferente para pruebas
                var testCollection = database.GetCollection<BsonDocument>("PruebasMongoDB");
                Debug.WriteLine("Colección de prueba seleccionada: PruebasMongoDB");
                
                // Crear un documento de prueba sencillo
                var document = new BsonDocument
                {
                    { "Nombre", "Empleado de Prueba" },
                    { "Edad", 30 },
                    { "Ciudad", "Ciudad de Prueba" },
                    { "Correo", "prueba@ejemplo.com" },
                    { "FechaPrueba", DateTime.Now }
                };
                
                // Insertar el documento directamente en la colección de prueba
                Debug.WriteLine("Intentando insertar documento de prueba...");
                testCollection.InsertOne(document);
                Debug.WriteLine("Documento insertado correctamente en colección de prueba");
                
                MessageBox.Show("Se ha insertado un documento de prueba en la colección 'PruebasMongoDB'", 
                    "Inserción exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Contar documentos en la colección
                var count = testCollection.CountDocuments(new BsonDocument());
                Debug.WriteLine($"Total de documentos en la colección de prueba: {count}");
                
                MessageBox.Show($"Total de documentos en la colección de prueba: {count}", 
                    "Conteo de documentos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Recuperar el documento recién insertado
                var filter = Builders<BsonDocument>.Filter.Eq("Nombre", "Empleado de Prueba");
                var result = testCollection.Find(filter).FirstOrDefault();
                
                if (result != null)
                {
                    Debug.WriteLine($"Documento recuperado: {result}");
                    MessageBox.Show($"Documento recuperado: {result["Nombre"]} - {result["Edad"]} - {result["Ciudad"]}",
                        "Recuperación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Debug.WriteLine("No se pudo recuperar el documento");
                    MessageBox.Show("No se pudo recuperar el documento insertado",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                // Ahora insertar un empleado de prueba en la colección real
                var empleadosCollection = database.GetCollection<EmpleadosG>("Empleados");
                var nuevoEmpleado = new EmpleadosG
                {
                    Nombre = "Prueba Empleado",
                    Edad = 25,
                    Ciudad = "Ciudad Empleado",
                    Correo = "empleado@ejemplo.com",
                    // Nota: No establecemos FechaPrueba para empleados normales
                };
                
                Debug.WriteLine("Insertando en colección de empleados reales...");
                empleadosCollection.InsertOne(nuevoEmpleado);
                Debug.WriteLine("Inserción en colección real completada");
                
                MessageBox.Show("Se ha insertado un empleado en la colección real 'Empleados'", 
                    "Segunda prueba exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Actualizar el DataGridView con datos de la colección real
                CargarEmpleadosSimple();
                
                MessageBox.Show("Prueba completada con éxito. Vista actualizada.", 
                    "Prueba finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                MessageBox.Show($"Error en la prueba de MongoDB: {ex.Message}\n\n" +
                    $"StackTrace: {ex.StackTrace}", 
                    "Error de prueba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para limpiar la colección de empleados
        private void btnLimpiarColeccion_Click(object sender, EventArgs e)
        {
            try
            {
                // Confirmar con el usuario
                DialogResult resultado = MessageBox.Show(
                    "¿Está seguro que desea eliminar TODOS los registros de la colección?\n\n" +
                    "Esta acción no se puede deshacer.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                    
                if (resultado != DialogResult.Yes)
                {
                    return;
                }
                
                // Conexión directa a MongoDB
                var connectionString = "mongodb://localhost:27017";
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("EmpresaDGMC");
                var collection = database.GetCollection<EmpleadosG>("Empleados");
                
                // Eliminar todos los documentos
                var deleteResult = collection.DeleteMany(Builders<EmpleadosG>.Filter.Empty);
                
                // Actualizar el DataGridView
                CargarEmpleadosSimple();
                
                MessageBox.Show($"Se han eliminado {deleteResult.DeletedCount} documentos de la colección.",
                    "Limpieza completada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al limpiar la colección: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Método de prueba muy simple
        private void btnPruebaSimple_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear un documento sencillo de prueba
                var empleadoPrueba = new EmpleadosG
                {
                    Nombre = "Empleado Test",
                    Edad = 25,
                    Ciudad = "Ciudad Test",
                    Correo = "test@ejemplo.com"
                };
                
                // Mostrar el ID generado
                MessageBox.Show($"ID generado para el empleado de prueba: {empleadoPrueba.Id}", 
                    "ID Generado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Intentar la conexión más básica
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("EmpresaDGMC");
                
                // Crear o usar una colección de prueba específica
                var collection = database.GetCollection<BsonDocument>("PruebaSimple");
                
                // Crear un documento muy simple
                var document = new BsonDocument
                {
                    { "test", "valor" },
                    { "fecha", DateTime.Now }
                };
                
                // Insertar el documento
                collection.InsertOne(document);
                
                // Contar documentos
                var count = collection.CountDocuments(new BsonDocument());
                
                // Mostrar resultado
                MessageBox.Show($"Prueba exitosa. Documento insertado. Total documentos en la colección de prueba: {count}", 
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en prueba simple: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para verificar directamente la colección MongoDB
        private void btnVerificarMongo_Click(object sender, EventArgs e)
        {
            try
            {
                // Conexión directa a MongoDB
                var connectionString = "mongodb://localhost:27017";
                var client = new MongoClient(connectionString);
                
                // Listar todas las bases de datos
                var dbList = client.ListDatabases().ToList();
                string dbNames = "Bases de datos disponibles:\n";
                foreach (var db in dbList)
                {
                    dbNames += $"- {db["name"]}\n";
                }
                
                MessageBox.Show(dbNames, "Bases de datos en MongoDB", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Verificar la base de datos específica
                var database = client.GetDatabase("EmpresaDGMC");
                
                // Listar todas las colecciones en la base de datos
                var collections = database.ListCollectionNames().ToList();
                string collectionNames = "Colecciones en EmpresaDGMC:\n";
                foreach (var coll in collections)
                {
                    collectionNames += $"- {coll}\n";
                }
                
                MessageBox.Show(collectionNames, "Colecciones en la BD", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Verificar específicamente la colección Empleados
                if (collections.Contains("Empleados"))
                {
                    var empleadosCollection = database.GetCollection<BsonDocument>("Empleados");
                    var empleados = empleadosCollection.Find(new BsonDocument()).ToList();
                    
                    string empleadosInfo = $"Total de documentos en colección 'Empleados': {empleados.Count}\n\n";
                    
                    if (empleados.Count > 0)
                    {
                        empleadosInfo += "Primeros 5 documentos:\n";
                        int count = 0;
                        foreach (var doc in empleados)
                        {
                            if (count >= 5) break;
                            empleadosInfo += $"Documento {count + 1}: {doc.ToJson().Substring(0, Math.Min(doc.ToJson().Length, 100))}...\n";
                            count++;
                        }
                    }
                    else
                    {
                        empleadosInfo += "No hay documentos en la colección.";
                    }
                    
                    MessageBox.Show(empleadosInfo, "Contenido de la colección 'Empleados'", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("La colección 'Empleados' no existe en la base de datos.\n" + 
                        "Esto explica por qué no ves registros guardados.", 
                        "Colección no encontrada", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar MongoDB: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}