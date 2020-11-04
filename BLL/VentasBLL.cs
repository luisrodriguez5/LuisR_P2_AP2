using LuisR_P2_AP2.Data;
using LuisR_P2_AP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LuisR_P2_AP2.BLL
{
    public class VentasBLL
    {
        public static List<Ventas> GetList(Expression<Func<Ventas, bool>> venta)
        {
            List<Ventas> Lista = new List<Ventas>();
            Contexto contexto = new Contexto();

            try
            {
                Lista = contexto.Ventas.Where(venta).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return Lista;
        }

                public static Ventas Buscar(int id)
        {
            Contexto contexto = new Contexto();
            Ventas ventas;

            try
            {
                ventas = contexto.Ventas.Find(id);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return ventas;

        }


    }
}
