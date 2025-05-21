using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Empresa_DGMC
{
    partial class EmpleadosForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblEdad = new System.Windows.Forms.Label();
            this.lblCiudad = new System.Windows.Forms.Label();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtEdad = new System.Windows.Forms.TextBox();
            this.txtCiudad = new System.Windows.Forms.TextBox();
            this.txtCorreo = new System.Windows.Forms.TextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.dataGridEmpleados = new System.Windows.Forms.DataGridView();
            this.groupBoxBusqueda = new System.Windows.Forms.GroupBox();
            this.lblBuscarNombre = new System.Windows.Forms.Label();
            this.txtBuscarNombre = new System.Windows.Forms.TextBox();
            this.btnBuscarNombre = new System.Windows.Forms.Button();
            this.lblBuscarCiudad = new System.Windows.Forms.Label();
            this.txtBuscarCiudad = new System.Windows.Forms.TextBox();
            this.btnBuscarCiudad = new System.Windows.Forms.Button();
            this.lblBuscarEdadMin = new System.Windows.Forms.Label();
            this.txtBuscarEdadMin = new System.Windows.Forms.TextBox();
            this.lblBuscarEdadMax = new System.Windows.Forms.Label();
            this.txtBuscarEdadMax = new System.Windows.Forms.TextBox();
            this.btnBuscarEdad = new System.Windows.Forms.Button();
            this.btnBuscarNombreCiudad = new System.Windows.Forms.Button();
            this.btnMostrarTodos = new System.Windows.Forms.Button();
            this.btnPruebaMongoDB = new System.Windows.Forms.Button();
            this.btnLimpiarColeccion = new System.Windows.Forms.Button();
            this.btnPruebaSimple = new System.Windows.Forms.Button();
            this.btnVerificarMongo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridEmpleados)).BeginInit();
            this.groupBoxBusqueda.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(9, 7);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(243, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Gestión de Empleados";
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(9, 45);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(47, 13);
            this.lblNombre.TabIndex = 1;
            this.lblNombre.Text = "Nombre:";
            // 
            // lblEdad
            // 
            this.lblEdad.AutoSize = true;
            this.lblEdad.Location = new System.Drawing.Point(9, 67);
            this.lblEdad.Name = "lblEdad";
            this.lblEdad.Size = new System.Drawing.Size(35, 13);
            this.lblEdad.TabIndex = 2;
            this.lblEdad.Text = "Edad:";
            // 
            // lblCiudad
            // 
            this.lblCiudad.AutoSize = true;
            this.lblCiudad.Location = new System.Drawing.Point(9, 88);
            this.lblCiudad.Name = "lblCiudad";
            this.lblCiudad.Size = new System.Drawing.Size(43, 13);
            this.lblCiudad.TabIndex = 3;
            this.lblCiudad.Text = "Ciudad:";
            // 
            // lblCorreo
            // 
            this.lblCorreo.AutoSize = true;
            this.lblCorreo.Location = new System.Drawing.Point(9, 110);
            this.lblCorreo.Name = "lblCorreo";
            this.lblCorreo.Size = new System.Drawing.Size(41, 13);
            this.lblCorreo.TabIndex = 4;
            this.lblCorreo.Text = "Correo:";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(53, 42);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(148, 20);
            this.txtNombre.TabIndex = 5;
            // 
            // txtEdad
            // 
            this.txtEdad.Location = new System.Drawing.Point(53, 65);
            this.txtEdad.Name = "txtEdad";
            this.txtEdad.Size = new System.Drawing.Size(148, 20);
            this.txtEdad.TabIndex = 6;
            // 
            // txtCiudad
            // 
            this.txtCiudad.Location = new System.Drawing.Point(53, 87);
            this.txtCiudad.Name = "txtCiudad";
            this.txtCiudad.Size = new System.Drawing.Size(148, 20);
            this.txtCiudad.TabIndex = 7;
            // 
            // txtCorreo
            // 
            this.txtCorreo.Location = new System.Drawing.Point(53, 108);
            this.txtCorreo.Name = "txtCorreo";
            this.txtCorreo.Size = new System.Drawing.Size(148, 20);
            this.txtCorreo.TabIndex = 8;
            // 
            // btnAgregar
            // 
            this.btnAgregar.Location = new System.Drawing.Point(10, 153);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(64, 26);
            this.btnAgregar.TabIndex = 9;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregarEmpleado_Click);
            // 
            // btnModificar
            // 
            this.btnModificar.Location = new System.Drawing.Point(80, 153);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(59, 27);
            this.btnModificar.TabIndex = 10;
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = true;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(146, 153);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(59, 26);
            this.btnEliminar.TabIndex = 11;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // dataGridEmpleados
            // 
            this.dataGridEmpleados.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridEmpleados.BackgroundColor = System.Drawing.SystemColors.Desktop;
            this.dataGridEmpleados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridEmpleados.Location = new System.Drawing.Point(9, 240);
            this.dataGridEmpleados.Name = "dataGridEmpleados";
            this.dataGridEmpleados.RowTemplate.Height = 25;
            this.dataGridEmpleados.Size = new System.Drawing.Size(570, 162);
            this.dataGridEmpleados.TabIndex = 12;
            this.dataGridEmpleados.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridEmpleados_CellClick);
            // 
            // groupBoxBusqueda
            // 
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarNombre);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarNombre);
            this.groupBoxBusqueda.Controls.Add(this.btnBuscarNombre);
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarCiudad);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarCiudad);
            this.groupBoxBusqueda.Controls.Add(this.btnBuscarCiudad);
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarEdadMin);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarEdadMin);
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarEdadMax);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarEdadMax);
            this.groupBoxBusqueda.Controls.Add(this.btnBuscarEdad);
            this.groupBoxBusqueda.Controls.Add(this.btnBuscarNombreCiudad);
            this.groupBoxBusqueda.Controls.Add(this.btnMostrarTodos);
            this.groupBoxBusqueda.Location = new System.Drawing.Point(220, 45);
            this.groupBoxBusqueda.Name = "groupBoxBusqueda";
            this.groupBoxBusqueda.Size = new System.Drawing.Size(352, 135);
            this.groupBoxBusqueda.TabIndex = 13;
            this.groupBoxBusqueda.TabStop = false;
            this.groupBoxBusqueda.Text = "Búsqueda con Índices";
            // 
            // lblBuscarNombre
            // 
            this.lblBuscarNombre.AutoSize = true;
            this.lblBuscarNombre.Location = new System.Drawing.Point(8, 19);
            this.lblBuscarNombre.Name = "lblBuscarNombre";
            this.lblBuscarNombre.Size = new System.Drawing.Size(47, 13);
            this.lblBuscarNombre.TabIndex = 14;
            this.lblBuscarNombre.Text = "Nombre:";
            // 
            // txtBuscarNombre
            // 
            this.txtBuscarNombre.Location = new System.Drawing.Point(51, 16);
            this.txtBuscarNombre.Name = "txtBuscarNombre";
            this.txtBuscarNombre.Size = new System.Drawing.Size(111, 20);
            this.txtBuscarNombre.TabIndex = 15;
            // 
            // btnBuscarNombre
            // 
            this.btnBuscarNombre.Location = new System.Drawing.Point(214, 6);
            this.btnBuscarNombre.Name = "btnBuscarNombre";
            this.btnBuscarNombre.Size = new System.Drawing.Size(81, 26);
            this.btnBuscarNombre.TabIndex = 16;
            this.btnBuscarNombre.Text = "Buscar por Nombre";
            this.btnBuscarNombre.UseVisualStyleBackColor = true;
            // 
            // lblBuscarCiudad
            // 
            this.lblBuscarCiudad.AutoSize = true;
            this.lblBuscarCiudad.Location = new System.Drawing.Point(8, 42);
            this.lblBuscarCiudad.Name = "lblBuscarCiudad";
            this.lblBuscarCiudad.Size = new System.Drawing.Size(43, 13);
            this.lblBuscarCiudad.TabIndex = 17;
            this.lblBuscarCiudad.Text = "Ciudad:";
            // 
            // txtBuscarCiudad
            // 
            this.txtBuscarCiudad.Location = new System.Drawing.Point(51, 39);
            this.txtBuscarCiudad.Name = "txtBuscarCiudad";
            this.txtBuscarCiudad.Size = new System.Drawing.Size(111, 20);
            this.txtBuscarCiudad.TabIndex = 18;
            // 
            // btnBuscarCiudad
            // 
            this.btnBuscarCiudad.Location = new System.Drawing.Point(214, 33);
            this.btnBuscarCiudad.Name = "btnBuscarCiudad";
            this.btnBuscarCiudad.Size = new System.Drawing.Size(81, 26);
            this.btnBuscarCiudad.TabIndex = 19;
            this.btnBuscarCiudad.Text = "Buscar por Ciudad";
            this.btnBuscarCiudad.UseVisualStyleBackColor = true;
            // 
            // lblBuscarEdadMin
            // 
            this.lblBuscarEdadMin.AutoSize = true;
            this.lblBuscarEdadMin.Location = new System.Drawing.Point(5, 64);
            this.lblBuscarEdadMin.Name = "lblBuscarEdadMin";
            this.lblBuscarEdadMin.Size = new System.Drawing.Size(55, 13);
            this.lblBuscarEdadMin.TabIndex = 20;
            this.lblBuscarEdadMin.Text = "Edad Min:";
            // 
            // txtBuscarEdadMin
            // 
            this.txtBuscarEdadMin.Location = new System.Drawing.Point(66, 62);
            this.txtBuscarEdadMin.Name = "txtBuscarEdadMin";
            this.txtBuscarEdadMin.Size = new System.Drawing.Size(30, 20);
            this.txtBuscarEdadMin.TabIndex = 21;
            // 
            // lblBuscarEdadMax
            // 
            this.lblBuscarEdadMax.AutoSize = true;
            this.lblBuscarEdadMax.Location = new System.Drawing.Point(102, 64);
            this.lblBuscarEdadMax.Name = "lblBuscarEdadMax";
            this.lblBuscarEdadMax.Size = new System.Drawing.Size(58, 13);
            this.lblBuscarEdadMax.TabIndex = 22;
            this.lblBuscarEdadMax.Text = "Edad Max:";
            // 
            // txtBuscarEdadMax
            // 
            this.txtBuscarEdadMax.Location = new System.Drawing.Point(166, 62);
            this.txtBuscarEdadMax.Name = "txtBuscarEdadMax";
            this.txtBuscarEdadMax.Size = new System.Drawing.Size(31, 20);
            this.txtBuscarEdadMax.TabIndex = 23;
            // 
            // btnBuscarEdad
            // 
            this.btnBuscarEdad.Location = new System.Drawing.Point(214, 62);
            this.btnBuscarEdad.Name = "btnBuscarEdad";
            this.btnBuscarEdad.Size = new System.Drawing.Size(81, 26);
            this.btnBuscarEdad.TabIndex = 24;
            this.btnBuscarEdad.Text = "Buscar por Edad";
            this.btnBuscarEdad.UseVisualStyleBackColor = true;
            // 
            // btnBuscarNombreCiudad
            // 
            this.btnBuscarNombreCiudad.Location = new System.Drawing.Point(8, 87);
            this.btnBuscarNombreCiudad.Name = "btnBuscarNombreCiudad";
            this.btnBuscarNombreCiudad.Size = new System.Drawing.Size(154, 35);
            this.btnBuscarNombreCiudad.TabIndex = 25;
            this.btnBuscarNombreCiudad.Text = "Buscar por Nombre y Ciudad";
            this.btnBuscarNombreCiudad.UseVisualStyleBackColor = true;
            // 
            // btnMostrarTodos
            // 
            this.btnMostrarTodos.Location = new System.Drawing.Point(213, 96);
            this.btnMostrarTodos.Name = "btnMostrarTodos";
            this.btnMostrarTodos.Size = new System.Drawing.Size(81, 26);
            this.btnMostrarTodos.TabIndex = 26;
            this.btnMostrarTodos.Text = "Mostrar Todos";
            this.btnMostrarTodos.UseVisualStyleBackColor = true;
            // 
            // btnPruebaMongoDB
            // 
            this.btnPruebaMongoDB.Location = new System.Drawing.Point(10, 217);
            this.btnPruebaMongoDB.Name = "btnPruebaMongoDB";
            this.btnPruebaMongoDB.Size = new System.Drawing.Size(129, 20);
            this.btnPruebaMongoDB.TabIndex = 27;
            this.btnPruebaMongoDB.Text = "Probar MongoDB";
            this.btnPruebaMongoDB.UseVisualStyleBackColor = true;
            this.btnPruebaMongoDB.Click += new System.EventHandler(this.btnPruebaMongoDB_Click);
            // 
            // btnLimpiarColeccion
            // 
            this.btnLimpiarColeccion.Location = new System.Drawing.Point(146, 217);
            this.btnLimpiarColeccion.Name = "btnLimpiarColeccion";
            this.btnLimpiarColeccion.Size = new System.Drawing.Size(129, 20);
            this.btnLimpiarColeccion.TabIndex = 28;
            this.btnLimpiarColeccion.Text = "Limpiar Colección";
            this.btnLimpiarColeccion.UseVisualStyleBackColor = true;
            this.btnLimpiarColeccion.Click += new System.EventHandler(this.btnLimpiarColeccion_Click);
            // 
            // btnPruebaSimple
            // 
            this.btnPruebaSimple.Location = new System.Drawing.Point(320, 217);
            this.btnPruebaSimple.Name = "btnPruebaSimple";
            this.btnPruebaSimple.Size = new System.Drawing.Size(129, 20);
            this.btnPruebaSimple.TabIndex = 29;
            this.btnPruebaSimple.Text = "Prueba Simple";
            this.btnPruebaSimple.UseVisualStyleBackColor = true;
            this.btnPruebaSimple.Click += new System.EventHandler(this.btnPruebaSimple_Click);
            // 
            // btnVerificarMongo
            // 
            this.btnVerificarMongo.Location = new System.Drawing.Point(455, 217);
            this.btnVerificarMongo.Name = "btnVerificarMongo";
            this.btnVerificarMongo.Size = new System.Drawing.Size(129, 20);
            this.btnVerificarMongo.TabIndex = 30;
            this.btnVerificarMongo.Text = "Verificar MongoDB";
            this.btnVerificarMongo.UseVisualStyleBackColor = true;
            this.btnVerificarMongo.Click += new System.EventHandler(this.btnVerificarMongo_Click);
            // 
            // EmpleadosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 477);
            this.Controls.Add(this.groupBoxBusqueda);
            this.Controls.Add(this.dataGridEmpleados);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.txtCorreo);
            this.Controls.Add(this.txtCiudad);
            this.Controls.Add(this.txtEdad);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblCorreo);
            this.Controls.Add(this.lblCiudad);
            this.Controls.Add(this.lblEdad);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.btnPruebaMongoDB);
            this.Controls.Add(this.btnLimpiarColeccion);
            this.Controls.Add(this.btnPruebaSimple);
            this.Controls.Add(this.btnVerificarMongo);
            this.Name = "EmpleadosForm";
            this.Text = "Gestión de Empleados - Empresa DGMC";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridEmpleados)).EndInit();
            this.groupBoxBusqueda.ResumeLayout(false);
            this.groupBoxBusqueda.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblTitulo;
        private Label lblNombre;
        private Label lblEdad;
        private Label lblCiudad;
        private Label lblCorreo;
        private TextBox txtNombre;
        private TextBox txtEdad;
        private TextBox txtCiudad;
        private TextBox txtCorreo;
        private Button btnAgregar;
        private Button btnModificar;
        private Button btnEliminar;
        private DataGridView dataGridEmpleados;
        
        // Nuevos controles
        private GroupBox groupBoxBusqueda;
        private Label lblBuscarNombre;
        private TextBox txtBuscarNombre;
        private Button btnBuscarNombre;
        private Label lblBuscarCiudad;
        private TextBox txtBuscarCiudad;
        private Button btnBuscarCiudad;
        private Label lblBuscarEdadMin;
        private TextBox txtBuscarEdadMin;
        private Label lblBuscarEdadMax;
        private TextBox txtBuscarEdadMax;
        private Button btnBuscarEdad;
        private Button btnBuscarNombreCiudad;
        private Button btnMostrarTodos;
        private Button btnPruebaMongoDB;
        private Button btnLimpiarColeccion;
        private Button btnPruebaSimple;
        private Button btnVerificarMongo;
    }
}

