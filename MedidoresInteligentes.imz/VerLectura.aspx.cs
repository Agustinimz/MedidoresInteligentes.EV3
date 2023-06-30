using MedidoresInteligentesModel.DAL;
using MedidoresInteligentesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MedidoresInteligentes.imz
{
    public partial class VerLectura : System.Web.UI.Page
    {
        private IMedidorDAL medidorDAL = new MedidorDALObjetos();
        private ILecturaDAL lecturasDAL = new LecturaDALObjetos();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                filtroMedidor();
                cargarGrilla();
                
            }
        }

        protected void cargarGrilla()
        {
            List<Lectura> lecturas = lecturasDAL.Obtener();

            // Filtrar las lecturas por el medidor seleccionado
            string medidorSeleccionado = filtroM.SelectedValue;
            if (!string.IsNullOrEmpty(medidorSeleccionado) && medidorSeleccionado != "Filtrar Medidores Añadidos")
            {
                lecturas = lecturas.Where(l => l.Medidortipo.Serie == medidorSeleccionado).ToList();
            }

            this.grillaLectura.DataSource = lecturas;
            this.grillaLectura.DataBind();
        }

        protected void filtroMedidor()
        {
            List<Medidor> medidores = medidorDAL.ObtenerMedidores();
            this.filtroM.DataSource = medidores;
            this.filtroM.DataTextField = "Tipo";
            this.filtroM.DataValueField = "Serie";
            this.filtroM.DataBind();
        }

        protected void filtroM_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarGrilla();
        }
    }
}