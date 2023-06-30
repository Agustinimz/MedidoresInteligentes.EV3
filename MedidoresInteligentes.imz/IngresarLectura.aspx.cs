using MedidoresInteligentesModel;
using MedidoresInteligentesModel.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MedidoresInteligentes.imz
{
    public partial class IngresarLectura : System.Web.UI.Page
    {
        //Traemos la referencias de MedidiosInteModel
        private ILecturaDAL lecturaDAL = new LecturaDALObjetos();
        private IMedidorDAL medidorDAL = new MedidorDALObjetos();

        //Metodo a utulizar cuando se carga el form:
        // - Cuando es un peticion GET (!Postback)
        // - Cuando es una peticion POST (Postback)

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<Medidor> medidores = medidorDAL.ObtenerMedidores();
                this.medidorDbl.DataSource = medidores;
                this.medidorDbl.DataTextField = "Tipo";
                this.medidorDbl.DataValueField = "Serie";
                this.medidorDbl.DataBind();
            }

        }
        protected void agregarBtn_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                //Obtener los datos del formulario
                // Obtener los datos del formulario
                DateTime fechaLectura = Convert.ToDateTime(this.calendartxt.SelectedDate);
                string horaLectura = this.horatxt.Text + ":" + this.minutotxt.Text;

                // Obtiene el valor del DropDownList
                string medidorValor = this.medidorDbl.SelectedValue;
                // Obtiene el valor del texto del DropDownList
                string medidorText = this.medidorDbl.SelectedItem.Text;
                if (string.IsNullOrEmpty(medidorText) || medidorText.Trim() == "Seleccione un Medidor")
                {
                    this.mensajesError.Text = "Selecciona un medidor o añadir campo.";
                    return;
                }


                // Validamos el campo hora
                int hora;
                if (!int.TryParse(this.horatxt.Text, out hora) || hora < 0 || hora > 23)
                {
                    // Mostrar mensaje de error y detener el proceso
                    this.mensajesError.Text = "El valor de la hora debe estar entre 0 y 23.";
                    return;
                }

                // Validamos el campo minuto
                int minuto;
                if (!int.TryParse(this.minutotxt.Text, out minuto) || minuto < 0 || minuto > 59)
                {
                    // Mostrar mensaje de error y detener el proceso
                    this.mensajesError.Text = "El valor del minuto debe estar entre 0 y 59.";
                    return;
                }

                // Validamos el campo valor de consumo
                decimal valorConsumo;
                if (!decimal.TryParse(this.consumotxt.Text, out valorConsumo) || valorConsumo < 0 || valorConsumo > 600)
                {
                    // Mostrar mensaje de error y detener el proceso
                    this.mensajesError.Text = "El valor del consumo debe estar entre 0 y 600.";
                    return;
                }

                // Instanciamos medidor y lectura
                List<Medidor> medidores = medidorDAL.ObtenerMedidores();
                Medidor medidor = medidores.Find(m => m.Serie == this.medidorDbl.SelectedItem.Value);

                // Construir el objeto tipo lectura
                Lectura lectura = new Lectura()
                {
                    Medidortipo = medidor,
                    FechaLectura = fechaLectura,
                    HoraLectura = horaLectura,
                    ValorConsumo = valorConsumo
                };

                // Llamar al DAL
                lecturaDAL.Agregar(lectura);

                // Mostrar mensaje de éxito
                this.mensajesLbl.Text = "Medidor guardado correctamente";
                Response.Redirect("VerLectura.aspx");
            }
        }

    }
}