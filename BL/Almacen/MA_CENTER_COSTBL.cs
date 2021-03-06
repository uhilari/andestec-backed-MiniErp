﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Almacen;
using DA.Almacen;

namespace BL.Almacen
{
    public class MA_CENTER_COSTBL
    {
        public string Registrar(EMA_CENTER_COST ee) { if (MA_CENTER_COSTDA.GetByid(ee) == null) {return MA_CENTER_COSTDA.Insert(ee); } else {return MA_CENTER_COSTDA.Update(ee); } }
        public string Eliminar(EMA_CENTER_COST ee) { return MA_CENTER_COSTDA.Delete(ee); }
        public List<EMA_CENTER_COST> Listar(EMA_CENTER_COST ee) { return MA_CENTER_COSTDA.GetAll(ee); }
        public EMA_CENTER_COST ListarxId(EMA_CENTER_COST ee) => MA_CENTER_COSTDA.GetByid(ee);
    }
}
