﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Ventas;
using DA.Ventas;

namespace BL.Ventas
{
    public class MA_TYPECUSTOMERBL
    {
        public string Registrar(EMA_TYPECUSTOMER ee) { if (MA_TYPECUSTOMERDA.GetByid(ee) == null) {return MA_TYPECUSTOMERDA.Insert(ee); } else { return MA_TYPECUSTOMERDA.Update(ee); } }
        public string Eliminar(EMA_TYPECUSTOMER ee) { return MA_TYPECUSTOMERDA.Delete(ee); }
        public List<EMA_TYPECUSTOMER> Listar(EMA_TYPECUSTOMER ee) { return MA_TYPECUSTOMERDA.GetAll(ee); }
        public EMA_TYPECUSTOMER ListarxId(EMA_TYPECUSTOMER ee) => MA_TYPECUSTOMERDA.GetByid(ee);
    }
}
