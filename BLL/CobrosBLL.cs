using LuisR_P2_AP2.Data;
using LuisR_P2_AP2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LuisR_P2_AP2.BLL
{
    public class CobrosBLL
    {
                public static bool Guardar(Cobros cobros)
        {
            if (!Existe(cobros.CobroId))//si no existe insertamos
                return Insertar(cobros);
            else
                return Modificar(cobros);

        }

        private static bool Insertar(Cobros cobros)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {

                foreach (var item in cobros.CobrosDetalle)
                {
                    
                    var auxCobro = contexto.Ventas.Find(item.VentaId);
                    if (auxCobro != null)
                    {
                        auxCobro.Balance -= item.Cobrado;
                    }
                }
                
                contexto.Cobros.Add(cobros);
                paso = contexto.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;

            }
            finally
            {
                contexto.Dispose();
            }
            return paso;
        }


        private static bool Modificar(Cobros cobros)
        {
            bool paso = false;
            var Anterior = Buscar(cobros.CobroId);
            Contexto contexto = new Contexto();

            try
            {
                //aqui borro del detalle y disminuyo 
                foreach (var item in Anterior.CobrosDetalle)
                {
                    var auxVenta = contexto.Ventas.Find(item.VentaId);
                    if (!cobros.CobrosDetalle.Exists(d => d.CobroDetalleId == item.CobroDetalleId))
                    {
                        if (auxVenta != null)
                        {
                            auxVenta.Balance -= item.Balance;
                        }

                        contexto.Entry(item).State = EntityState.Deleted;
                    }

                }

                //aqui agrego lo nuevo al detalle
                foreach (var item in cobros.CobrosDetalle)
                {
                    var auxVenta = contexto.Ventas.Find(item.VentaId);
                    if (item.CobroDetalleId == 0)
                    {
                        contexto.Entry(item).State = EntityState.Added;
                        if (auxVenta != null)
                        {
                            auxVenta.Balance += item.Balance;
                        }

                    }
                    else
                        contexto.Entry(item).State = EntityState.Modified;
                }


                contexto.Entry(cobros).State = EntityState.Modified;
                paso = contexto.SaveChanges() > 0;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return paso;
        }

        public static bool Eliminar(int id)
        {
            bool paso = false;
            var Anterior = Buscar(id);
            Contexto contexto = new Contexto();

            try
            {
                if (Existe(id))
                {
                    
                    foreach (var item in Anterior.CobrosDetalle)
                    {
                        var auxVenta = contexto.Ventas.Find(item.VentaId);
                        if (auxVenta != null)
                        {
                            auxVenta.Balance = item.Balance;
                        }
                    }

                    //aqui remueve la entidad
                    var auxCobro = contexto.Cobros.Find(id);
                    if (auxCobro != null)
                    {
                        contexto.Cobros.Remove(auxCobro);
                        paso = contexto.SaveChanges() > 0;
                    }
                }
               
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return paso;
        }

        public static Cobros Buscar(int id)
        {
            Contexto contexto = new Contexto();
            Cobros cobros; 

            try
            {
                cobros = contexto.Cobros.Where(o => o.CobroId == id).Include(d => d.CobrosDetalle).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return cobros;

        }

        public static List<Cobros> GetList(Expression<Func<Cobros, bool>> expression)
        {
            List<Cobros> lista = new List<Cobros>();
            Contexto db = new Contexto();

            try
            {
                lista = db.Cobros.Where(expression).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                db.Dispose();
            }

            return lista;
        }

        public static bool Existe(int id)
        {
            Contexto contexto = new Contexto();
            bool encontrado = false;

            try
            {
                encontrado = contexto.Cobros.Any(o => o.CobroId == id);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return encontrado;

        }

    }
}
