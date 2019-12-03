using System;
using System.Linq;
using System.Collections.Generic;
using CoreEscuela.Entidades;

namespace CoreEscuela.App
{
    public class Reporteador
    {
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObjEsc)
        {
            if (dicObjEsc == null)
                throw new ArgumentNullException(nameof(dicObjEsc));

            _diccionario = dicObjEsc;
        }

        public IEnumerable<Evaluacion> GetListaEvalucion()
        {
            if (_diccionario.TryGetValue(LlaveDiccionario.Evaluacion, out IEnumerable<ObjetoEscuelaBase> lista))
            {
                return lista.Cast<Evaluacion>();
            }
            else
            {
                return new List<Evaluacion>();
                //Implementar un log
            }
        }

        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(out var dummy);
        }

        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluacion> listaEval)
        {
            listaEval = GetListaEvalucion();

            return (from ev in listaEval
                    select ev.Asignatura.Nombre).Distinct();
        }



        public Dictionary<string, IEnumerable<Evaluacion>> GetDicEvaluacionAsign()
        {
            var dictaReporte = new Dictionary<string, IEnumerable<Evaluacion>>();
            var listaAsing = GetListaAsignaturas(out var listaEval);

            foreach (var asig in listaAsing)
            {
                var evalAsing = from eval in listaEval
                                where eval.Asignatura.Nombre == asig
                                select eval;

                dictaReporte.Add(asig, evalAsing);

            }

            return dictaReporte;
        }

        public Dictionary<string, IEnumerable<object>> GetPromedioAlumnoAsig()
        {
            var rta = new Dictionary<string, IEnumerable<object>>();
            var dicEvalAsig = GetDicEvaluacionAsign();

            foreach (var asig in dicEvalAsig)
            {
                var promedioAlumnos = from eval in asig.Value
                                      group eval by new 
                                      {
                                          eval.Alumno.UniqueId,
                                          eval.Alumno.Nombre
                                      }
                            into grupoEvalAlumno
                                      select new AlumnoPromedio
                                      {
                                          AlumnoId = grupoEvalAlumno.Key.UniqueId,
                                          Alumno = grupoEvalAlumno.Key.Nombre,
                                          Promedio = grupoEvalAlumno.Average(evaluacion => evaluacion.Nota)
                                      };
                rta.Add(asig.Key, promedioAlumnos);
            }

            return rta;
        }
    }
}