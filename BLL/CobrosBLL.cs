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
        public static bool Guardar(Cobros cobro)
        {
            if (!Existe(cobro.CobroId))
                return Insertar(cobro);
            else
                return Modificar(cobro);
        }

        private static bool Existe(int id)
        {
            bool Existencia = false;
            Contexto contexto = new Contexto();

            try
            {
                Existencia = contexto.Cobros.Any(x => x.CobroId == id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return Existencia;
        }

        private static bool Insertar(Cobros cobro)
        {
            bool Insertado = false;
            Contexto contexto = new Contexto();

            try
            {
                contexto.Cobros.Add(cobro);
                Insertado = (contexto.SaveChanges() > 0);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return Insertado;
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
            bool Eliminado = false;
            Contexto contexto = new Contexto();

            try
            {
                var cobro = Buscar(id);

                contexto.Entry(cobro).State = EntityState.Deleted;
                Eliminado = (contexto.SaveChanges() > 0);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return Eliminado;
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

        public static List<Cobros> GetList(Expression<Func<Cobros, bool>> cobro)
        {
            List<Cobros> Lista = new List<Cobros>();
            Contexto contexto = new Contexto();

            try
            {
                Lista = contexto.Cobros.Where(cobro).ToList();
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
    }
}
