﻿using System;
using System.Collections.Generic;
using System.Data;
using SYSTEMCODE.Capa_de_Negocio;

namespace SYSTEMCODE.Capa_de_Datos
{
    public static class ProyectoDatos
    {
        private static Proyecto DiseniarProyecto(int posicion, DataTable tabla)
        {
            int idProyecto = Convert.ToInt32(tabla.Rows[posicion]["id_proyecto"].ToString());
            string descripcion = tabla.Rows[posicion]["descripcion"].ToString();
            string version = tabla.Rows[posicion]["version"].ToString();
            string alcance = tabla.Rows[posicion]["alcance"].ToString();
            int idResponsable = Convert.ToInt32(tabla.Rows[posicion]["id_responsable"].ToString());
            bool borrado = Convert.ToBoolean(tabla.Rows[posicion]["borrado"].ToString());

            Usuario usuarioResponsable = UsuarioDatos.ConsultarUsuarioPorID(idResponsable);

            return new Proyecto(idProyecto, descripcion, version, alcance, usuarioResponsable, borrado);
        }

        public static Proyecto ConsultarProyectoPorDescripcion(string descripcionProyecto)
        {
            string SQL = "SELECT * " +
                         "FROM Proyectos " +
                         "WHERE " +
                            "descripcion = '" + descripcionProyecto.ToString() + "'";

            DataTable tabla = GestorBD.Consultar(SQL);

            return (tabla.Rows.Count > 0) ? DiseniarProyecto(0, tabla) : null;
        }

        public static Proyecto ConsultarProyectoPorID(int idProyecto)
        {
            string SQL = "SELECT proyectos.* " +
                         "FROM Proyectos proyectos " +
                         "WHERE " +
                            "id_proyecto = " + idProyecto.ToString() + " AND " +
                            "borrado = 0";

            DataTable tabla = GestorBD.Consultar(SQL);

            return (tabla.Rows.Count > 0) ? DiseniarProyecto(0, tabla) : null;
        }

        public static IList<Proyecto> ConsultarTablaProyectos()
        {
            IList<Proyecto> listaProyectos = new List<Proyecto>();

            DataTable tabla = GestorBD.ConsultarTabla("Proyectos");
            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                listaProyectos.Add(DiseniarProyecto(i, tabla));
            }

            return listaProyectos;
        }

        public static DataTable ConsultarTablaProyectosComboBox()
        {
            string SQL = "SELECT proyectos.* " +
                        "FROM Proyectos proyectos " +
                        "WHERE borrado = 0";

            return GestorBD.Consultar(SQL);
        }

        public static IList<Proyecto> ConsultarTablaProyectosFiltro(string filtro)
        {
            string SQL = "SELECT proyectos.* " +
                         "FROM Proyectos proyectos " +
                         "WHERE descripcion LIKE '" + filtro + "%'";

            List<Proyecto> listaProyectos = new List<Proyecto>();

            DataTable tabla = GestorBD.Consultar(SQL);
            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                listaProyectos.Add(DiseniarProyecto(i, tabla));
            }

            return listaProyectos;
        }

        public static string InsertarProyecto(Proyecto proyecto)
        {
            string SQL = "INSERT INTO Proyectos " +
                         "VALUES " +
                         "(" +
                            "null, " +
                            "'" + proyecto.Descripcion.ToString() + "', " +
                            "'" + proyecto.Version.ToString() + "', " +
                            "'" + proyecto.Alcance.ToString() + "', " +
                            Convert.ToInt32(proyecto.Responsable.Id_usuario.ToString()) + ", " +
                            "0" +
                         ")";

            return GestorBD.Ejecutar(SQL);
        }

        public static string ModificarProyecto(Proyecto proyecto)
        {
            string SQL = "UPDATE Proyectos " +
                         "SET " +
                            "version = '" + proyecto.Version.ToString() + "', " +
                            "alcance = '" + proyecto.Alcance.ToString() + "', " +
                            "id_responsable = " + proyecto.Responsable.Id_usuario.ToString() + ", " +
                            "borrado = " + Convert.ToInt32(proyecto.Borrado) + " " +
                         "WHERE descripcion = '" + proyecto.Descripcion.ToString() + "'";

            return GestorBD.Ejecutar(SQL);
        }

        public static string EliminarProyecto(Proyecto proyecto)
        {
            string SQL = "UPDATE Proyectos " +
                         "SET " +
                            "borrado = 1 " +
                         "WHERE descripcion = '" + proyecto.Descripcion.ToString() + "'";

            return GestorBD.Ejecutar(SQL);
        }
    }
}