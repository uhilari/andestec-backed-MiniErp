﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Almacen;
using BL.Reportes;
using DA.Almacen;

namespace BL.Almacen
{
    public class TRA_WAREHOUSEBL
    {
        public string Registrar(ETRA_GUIAING guia) {
            return TRA_WAREHOUSEDA.Insert(guia);
        }

        public List<ERE_LISTA01> Listar(int ide, string alm, int ayo, int mes) {
            return TRA_WAREHOUSEDA.GetAll(ide, alm, ayo, mes);
        }

        public IEnumerable<ERE_LISTA02> ListarStockxAlmacen(int ide, string alm)
        {
            return TRA_WAREHOUSEDA.GetRepStockxAlmacen(ide, alm);
        }

        public Stream ReportStockxAlmacen(int ide, string alm)
        {
            return RE_REPORTEXCEL.CreateGetRepStockxAlmacen(ide, alm);
            //return TRA_WAREHOUSEDA.GetRepStockxAlmacen(ide, alm);
        }

        public List<ERE_LISTA03> ListarDetalleStockxAlmacen(int ide, int idarticulo)
        {
            return TRA_WAREHOUSEDA.GetRepDetalleStockxAlmacen(ide, idarticulo);
        }

        public ERE_LISTA06 ListarRepVistaDocumento(int ide, int idtrans)
        {
            ERE_LISTA06 ent = new ERE_LISTA06();
            ent.Cabecera = TRA_WAREHOUSEDA.GetRepVistaDocCab(ide, idtrans);
            ent.Detalle = TRA_WAREHOUSEDA.GetRepVistaDocDet(idtrans);
            return ent;
        }

        public ERE_LISTA04 ListarRepVistaDocCab(int ide, int idDocAlm)
        {
            return TRA_WAREHOUSEDA.GetRepVistaDocCab(ide, idDocAlm);
        }

        public List<ERE_LISTA05> ListarRepVistaDocDet(int idDocAlm)
        {
            return TRA_WAREHOUSEDA.GetRepVistaDocDet(idDocAlm);
        }

        public string ActualizarCostoAlm(ETRA_WAREHOUSE_LINE ee) {
            return TRA_WAREHOUSEDA.UpdateCosto(ee);
        }


    }   

}
